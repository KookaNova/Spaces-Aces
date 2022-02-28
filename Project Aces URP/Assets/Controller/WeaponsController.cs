using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Cox.PlayerControls{
///<summary> Receives information from Cox.PlayerControls.SpacecraftController in order to allow the player to target
/// and attack enemies with missiles and guns. </summary>
public class WeaponsController : MonoBehaviourPunCallbacks
{

    //code for finding the next target needs work. Consider adding a variable for it that's more consistent.

    #region Serialized Fields
    public enum TargetingMode{
        TeamA,
        TeamB,
        Global
    }
    GameManager gameManager;

    [HideInInspector] public SpacecraftController owner = null;
    [HideInInspector] public TargetingMode targMode = TargetingMode.TeamA;
    [SerializeField] private Canvas worldHud, overlayHud;
    [SerializeField] private Image aimReticle, distanceReticle;
    [SerializeField] private GameObject lockIndicator;
    [SerializeField] private ObjectIndicator objectIndicator;
    [SerializeField] private List<ObjectIndicator> activeIndicators;
    [SerializeField] private Camera fovCam;
    private Text lockedText;
    #endregion

    #region Public Weapon Fields
    [Header("Gun Information")]
    [SerializeField] private GameObject ammoType;
    [SerializeField] private float fireRate = 0.1f,
        gunSpeed = 1000,
        gunRange = 3000,
        chargeLossSpeed = .01f;

    [SerializeField] private Transform[] gunPosition;

    [Header("Missile Information")]
    public GameObject missileType;
    public float lockOnEfficiency = 2,
        lockOnRange = 2750,
        missileReload = 5;

    [SerializeField] public Transform[] missilePosition;

    public AudioSource gunCannonAudio, missileCannonAudio;
    #endregion

    #region Hidden Fields
    public List<TargetableObject> currentTargetSelection;
    [HideInInspector] public int currentGunIndex = 0, currentMis = 0, currentTarget = 0;
    [HideInInspector] public float gunModifier = 0, missileModifier = 0, gunCharge = 0f;
    [HideInInspector] public int missilesAvailable;
    private float lockOnModifier, lockOnDefault = 0.01f;
    bool missileRecentlyFired = false;

    public bool missileLocked = false, gunOvercharged = false;
    [HideInInspector] public bool canFire = true, canLaunchMissile = true;
    bool isTargetVisible = false;

    #endregion

    #region enable
    public void EnableWeapons() {
        gameManager = FindObjectOfType<GameManager>();
        currentTarget = 0;
        lockOnModifier = lockOnDefault;
        overlayHud.transform.SetParent(null);
        missilesAvailable = missilePosition.Length;

        var l = Instantiate(lockIndicator, parent: overlayHud.transform);
        lockIndicator = l;
        lockIndicator.gameObject.SetActive(false);
        lockedText = lockIndicator.GetComponentInChildren<Text>();

        //Spawns the player targeting the enemy
        if(owner.targetableObject.targetTeam == TargetableObject.TargetType.TeamA){
            targMode = TargetingMode.TeamB;
        }
        else{
            targMode = TargetingMode.TeamA;
        }
    }

    public void Reset(){
        gunCharge = 0;
        missileLocked = false;
        canLaunchMissile = true;
        missilesAvailable = missilePosition.Length;
    }
    
    #endregion

    #region targeting
    private void LateUpdate(){
        if(photonView.IsMine){
            PositionIndicators();
            LockPosition();
            AimGun();
        }
    }

    private void FixedUpdate(){
        if(gunCharge > 0){
            gunCharge -= 0.0015f;
            Debug.Log("gunCharge = " + gunCharge);
        }
        if(gunCharge <= 0){
            gunOvercharged = false;
        }
    }
    
    public void FindTargets(){
        for (int i = 0; i < gameManager.allTargets.Count; i++){
            if(gameManager.allTargets[i] == null){
                gameManager.allTargets.RemoveAt(i);
                
                continue;
            }
            if(gameManager.allTargets[i] != this.GetComponentInParent<TargetableObject>()){
                gameManager.allTargets.Add(gameManager.allTargets[i]);
            }
        }
        gameManager.allTargets.TrimExcess();

        Debug.Log(gameManager.allTargets.Count);
        GenerateIndicators();
    }
    public void ChangeTargetMode(int input){
        if(input == 0){
            targMode = TargetingMode.TeamA;
        }
        if(input == 1){
            targMode = TargetingMode.TeamB;
        }
        if(input == 2){
            targMode = TargetingMode.Global;
        }
        
        CleanTargetSelection();
    
    }
    private void CleanTargetSelection(){
        currentTargetSelection.Clear();
        for (int i = 0; i < gameManager.allTargets.Count; i++){
            if(gameManager.allTargets[i].targetTeam.ToString() == targMode.ToString()){
                if(gameManager.allTargets[i] == owner.targetableObject){
                    continue;
                }
                currentTargetSelection.Add(gameManager.allTargets[i]);
            }
        }
        GenerateIndicators();
    }

    private void GenerateIndicators(){
        Debug.Log("Generating Indicators");
        if(!photonView.IsMine)return;
        //destroy old indicators when mode is changed
        for(int i = 0; i < activeIndicators.Count; i++){
            if(activeIndicators[i] == null)continue;
            Destroy(activeIndicators[i].gameObject);
        }
        activeIndicators.Clear();
        activeIndicators.TrimExcess();

        //Create indicators and deactivate them, wait for object to be on camera to activate
        for(int i = 0; i < currentTargetSelection.Count; i++){
            var a = Instantiate(objectIndicator, overlayHud.transform);
            activeIndicators.Add(a);
            a.name = "Target_Indicator." + i;
            if(currentTargetSelection[i].targetTeam == owner.targetableObject.targetTeam) a.gameObject.GetComponent<Image>().color = Color.blue;
            a.gameObject.SetActive(false);
            a.GetComponent<Animator>().StopPlayback();
        }
        currentTargetSelection.TrimExcess(); 
    }

    private void PositionIndicators(){
        if(currentTargetSelection.Count <= 0){
            return;
        }

        for (int i = 0; i < activeIndicators.Count; i++){
            //Is the object active and on camera? If not, skip drawing the indicators.
            if(currentTargetSelection[i].gameObject == null){
                FindTargets();
            }
            if(currentTargetSelection[i].gameObject.activeInHierarchy == false || !currentTargetSelection[i].GetComponentInChildren<MeshRenderer>().isVisible){
                activeIndicators[i].gameObject.SetActive(false);
                continue;
            }
            
            //raycast
            int layermask = 1 << 14;
            RaycastHit hit;

            Vector3 origin = gameObject.transform.position + (transform.forward * 15);
            Vector3 dir = currentTargetSelection[i].gameObject.transform.position - origin;
            Debug.DrawRay(origin, dir, Color.green);
            
            
            

            
            //if cast hits nothing, or the hit doesn't have a rigidbody, remove indicators
            if(!Physics.SphereCast(origin, 10, dir, out hit, 15000, ~layermask)){
                activeIndicators[i].gameObject.SetActive(false); 
                //Debug.LogFormat("WeaponsSystem: PositionIndicators(), target {0} not visible. Indicator is inactive.", currentTargetSelection[i].nameOfTarget);
                continue;
            }
            if(hit.rigidbody == null){
                activeIndicators[i].gameObject.SetActive(false); 
                //Debug.LogFormat("WeaponsSystem: PositionIndicators(), No rigidbody found on target {0}. Indicator is inactive.", currentTargetSelection[i].name);
                continue;
            }
            
            //if object is not obstructed, activate indicators
            if(hit.rigidbody.gameObject == currentTargetSelection[i].gameObject){
                activeIndicators[i].gameObject.SetActive(true);
                if(currentTarget == -1){
                    currentTarget = i;
                    if((i+1) < currentTargetSelection.Count){
                        activeIndicators[currentTarget].next.gameObject.SetActive(false);
                        activeIndicators[i+1].next.gameObject.SetActive(true);
                    }
                    else{
                        activeIndicators[currentTarget].next.gameObject.SetActive(false);
                        activeIndicators[0].next.gameObject.SetActive(true);
                    }
                }  
            }
            else{
                activeIndicators[i].gameObject.SetActive(false);
                continue;
            }
            
            //Position indicator to the indicated objects screen position

            var screen = Camera.main.WorldToScreenPoint(currentTargetSelection[i].transform.position);
            screen.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude;
            var targetScreenPosition = Camera.main.ScreenToWorldPoint(screen);
            activeIndicators[i].transform.position = targetScreenPosition;

            if(i == currentTarget){
                activeIndicators[i].GetComponent<Animator>().StopPlayback();
            }
            if(i != currentTarget){
                activeIndicators[i].GetComponent<Animator>().StartPlayback();
            }
            //Displays name
            activeIndicators[i].playerName.text = currentTargetSelection[i].GetComponent<TargetableObject>().nameOfTarget;
            //Displays distance
            activeIndicators[i].distance.text = (Vector3.Distance(transform.position, currentTargetSelection[i].transform.position)).ToString();
            activeIndicators[i].next.gameObject.SetActive(false);
            if((currentTarget + 1) < currentTargetSelection.Count){
                activeIndicators[currentTarget+1].next.gameObject.SetActive(true);
            }
            else{
                activeIndicators[0].next.gameObject.SetActive(true);
            }
            
        }
        
    }

    private void LockPosition(){
        if(currentTarget >= currentTargetSelection.Count){
            currentTarget = 0;
            owner.shipBehaviour.lockOn.Stop();
            return;
        }
        if(currentTarget == -1){
            owner.shipBehaviour.lockOn.Stop();
            return;
        }
        if(currentTargetSelection.Count <= 0){
            CycleMainTarget();
            return;
        }

        if(!currentTargetSelection[currentTarget].gameObject.activeInHierarchy){
            CycleMainTarget();
            return;
        }

        Vector2 fovPosition = fovCam.WorldToViewportPoint(currentTargetSelection[currentTarget].transform.position);

        if(fovPosition.x >= 1 || fovPosition.x <= 0 || fovPosition.y >= 1 || fovPosition.x <= 0){

            lockOnModifier = lockOnDefault;
            lockIndicator.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, (overlayHud.transform.position - Camera.main.transform.position).magnitude));
            lockIndicator.gameObject.SetActive(false);
            owner.shipBehaviour.lockOn.Stop();
            missileLocked = false;
            //Debug.Log("Target not in FOV");
            return;
        }

        RaycastHit hit;
        int layermask = 1 << 14;

        Vector3 origin = gameObject.transform.position + (transform.forward * 15);
        Vector3 dir = currentTargetSelection[currentTarget].gameObject.transform.position - origin;
        Debug.DrawRay(origin, dir, Color.red);
        if(!Physics.SphereCast(origin, 10, dir, out hit, 3500, ~layermask)){
            lockIndicator.gameObject.SetActive(false);
            lockedText.text = "";
            return;
        }
        else if(hit.rigidbody == null){
            CycleMainTarget();
        }
        else if(hit.rigidbody.gameObject == currentTargetSelection[currentTarget].gameObject){
            
            lockIndicator.gameObject.SetActive(true);
            lockOnModifier += lockOnDefault * Time.fixedDeltaTime;
            

            

            var screen = Camera.main.WorldToScreenPoint(currentTargetSelection[currentTarget].transform.position);
            screen.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude;
            var targetScreenPosition = Camera.main.ScreenToWorldPoint(screen);
            if(!owner.shipBehaviour.lockOn.isPlaying){
                owner.shipBehaviour.lockOn.Play();
            }
            Vector3 slowMove = Vector3.MoveTowards(lockIndicator.transform.position, targetScreenPosition, (lockOnEfficiency * lockOnModifier) * .25f * Time.fixedDeltaTime);
            lockIndicator.transform.position = slowMove;

            if(lockIndicator.transform.position == targetScreenPosition && !missileLocked){
                owner.VoiceLine(4);
            }
            if(lockIndicator.transform.position == targetScreenPosition){
                missileLocked = true;
                lockOnModifier = 5000;
                lockedText.text = "LOCKED";
                owner.shipBehaviour.lockOn.SetScheduledEndTime(AudioSettings.dspTime + 0.02f);
            }
            else{
                missileLocked = false;
                owner.shipBehaviour.lockOn.SetScheduledEndTime(AudioSettings.dspTime + 1);
                lockedText.text = "";
            }
        }
        else{
            CycleMainTarget();
            
        }
    }

    public void CycleMainTarget(){
        owner.shipBehaviour.lockOn.Stop();
        lockedText.text = "";
        lockOnModifier = lockOnDefault;
        lockIndicator.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, (overlayHud.transform.position - Camera.main.transform.position).magnitude));
        lockIndicator.gameObject.SetActive(false);
        missileLocked = false;

        for(int i = 0; i < currentTargetSelection.Count; i++){
            currentTarget++;

            if(currentTarget >= currentTargetSelection.Count){
               currentTarget = 0;
            }
           
            if(!currentTargetSelection[currentTarget].gameObject.activeInHierarchy || !currentTargetSelection[i].GetComponentInChildren<MeshRenderer>().isVisible){
                Debug.Log("target not visible, incrementing");
                if(i == currentTargetSelection.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }

            RaycastHit hit;
            int layermask = 1 << 14;
            Vector3 origin = gameObject.transform.position + (transform.forward * 15);
            Vector3 dir = currentTargetSelection[currentTarget].gameObject.transform.position - origin;
            //activeIndicators[currentTarget].next.gameObject.SetActive(false);
            
            Debug.DrawRay(origin, dir, Color.white);

            if(!Physics.SphereCast(origin, 10, dir, out hit, 15000, ~layermask)){
                if(i == currentTargetSelection.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }
            else if(hit.rigidbody == false){
                if(i == currentTargetSelection.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }
            else if(hit.rigidbody.gameObject == currentTargetSelection[currentTarget].gameObject){
                return;
            }
            else{
                if(i == currentTargetSelection.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }

        }
    }
    #endregion

    #region weapon controls

    private void AimGun(){
        //aim gun position towards reticle
        Ray ray = new Ray(aimReticle.transform.position, aimReticle.transform.forward);
        var hitPoint = ray.GetPoint(gunRange);
        //var p = Camera.main.WorldToScreenPoint(hitPoint);

        var screen = Camera.main.WorldToScreenPoint(hitPoint);
        screen.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude;
        var position = Camera.main.ScreenToWorldPoint(screen);
        distanceReticle.transform.position = Vector3.Slerp(distanceReticle.transform.position, position, 15 * Time.fixedDeltaTime);


        for(int i = 0; i < gunPosition.Length; i++){
            gunPosition[i].LookAt(hitPoint);
        }
    }

    public void GunControl(bool gunInput, float currentSpeed){
        if(canFire && gunInput && !gunOvercharged){
            StartCoroutine(FireGun(gunInput, currentSpeed));
        }
    }

    public void MissileControl(float currentSpeed){
        StartCoroutine(MissileLaunch(currentSpeed));
    }
    #endregion

    #region IEnumerators

    private IEnumerator FireGun(bool gunIsFiring, float currentSpeed){
        canFire = false;
        
        while(gunIsFiring){
            var g = PhotonNetwork.Instantiate(ammoType.name, gunPosition[currentGunIndex].position, gunPosition[currentGunIndex].rotation);
            gunCharge += chargeLossSpeed;
            float inv = (0.001f + gunCharge)/5;
            if(gunCharge >= 1f){
                gunOvercharged = true;
            }
            gunCannonAudio.Play();
            g.GetComponent<Rigidbody>().velocity = gunPosition[currentGunIndex].transform.forward * gunSpeed;
            var behaviour = g.GetComponent<GunAmmoBehaviour>();
            behaviour.damageOutput += gunModifier;
            behaviour.owner = owner;
            yield return new WaitForSeconds(fireRate + inv);
            gunIsFiring = false;
            currentGunIndex++;
            if(currentGunIndex >= gunPosition.Length){
                currentGunIndex = 0;
            }
        }
        canFire = true;
    }

    private IEnumerator MissileReload(){
        canLaunchMissile = false;
        yield return new WaitForSeconds(missileReload);
        missilesAvailable ++;
        canLaunchMissile = true;

    }
    private IEnumerator MissileLaunch(float currentSpeed){
        if(missilesAvailable <= 0)canLaunchMissile = false;
        if(canLaunchMissile == false)yield break;
        canLaunchMissile = false;

        if(missileLocked == true){
            var m = PhotonNetwork.Instantiate(missileType.gameObject.name, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
            var behaviour = m.GetComponent<MissileBehaviour>();
            behaviour.currentSpeed = currentSpeed;
            behaviour.owner = owner;
            behaviour.target = currentTargetSelection[currentTarget].gameObject;
            behaviour.damageOutput += missileModifier;
            missilesAvailable--;
        }
        else{
            var m = PhotonNetwork.Instantiate(missileType.gameObject.name, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
            var behaviour = m.GetComponent<MissileBehaviour>();
            behaviour.currentSpeed = currentSpeed;
            behaviour.damageOutput += missileModifier;
            missilesAvailable--;
        }
        StartCoroutine(MissileReload());
        if(!missileRecentlyFired){
            owner.VoiceLine(5);
            missileRecentlyFired = true;
        }
        
        yield return new WaitForSeconds(.5f);
        currentMis++;
        if(currentMis > missilePosition.Length - 1){
            currentMis = 0;
        }
        canLaunchMissile = true;
        yield return new WaitForSeconds(2);
        missileRecentlyFired = false;
        
    }
    #endregion
}
}
