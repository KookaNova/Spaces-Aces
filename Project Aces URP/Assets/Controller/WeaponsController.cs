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
    public GameManager gameManager;

    [HideInInspector] public SpacecraftController owner = null;
    [Range(0,3)] [HideInInspector] public int targMode = 0; //0 = A, 1 = B, 2 = Global, 3 = Objective
    [SerializeField] private Canvas worldHud, overlayHud;
    [SerializeField] private Image aimReticle, distanceReticle;
    [SerializeField] private GameObject lockIndicator;
    [SerializeField] private ObjectIndicator objectIndicator;
    [SerializeField] private List<ObjectIndicator> activeIndicators;
    [SerializeField] private Camera fovCam;
    private Text lockedText;
    #endregion
    public TargetableObject finalTarget;

    #region Public Weapon Fields
    [Header("Gun Information")]
    [SerializeField] private GameObject ammoType;
    [SerializeField] private float fireRate = 0.1f,
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
    //public List<TargetableObject> currentTargetSelection;
    [HideInInspector] public int currentGunIndex = 0, currentMis = 0, currentTarget = 0;
    [HideInInspector] public float gunModifier = 0, missileModifier = 0, gunCharge = 0f;
    [HideInInspector] public int missilesAvailable;
    private float lockOnModifier, lockOnDefault = 0.01f;
    bool missileRecentlyFired = false;

    public bool missileLocked = false, gunOvercharged = false;
    [HideInInspector] public bool canFire = true, canLaunchMissile = true;

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
        if(owner.teamInt == 0){
            targMode = 1;
        }
        else{
            targMode = 0;
        }
    }

    public void Reset(){
        gunCharge = 0;
        canFire = true;
        missileLocked = false;
        canLaunchMissile = true;
        missilesAvailable = missilePosition.Length;
    }
    private void LateUpdate(){
        if(photonView.IsMine){
            PositionIndicators();
            LockPosition();
            AimGun();
        }
    }

    private void FixedUpdate(){
        GunTemperature();
    }
    #endregion

    #region targeting
    
    public void ChangeTargetMode(int input){
        targMode = input;
        CycleMainTarget();
    
    }

    private void GenerateIndicators(){
        if(!photonView.IsMine)return;
        if(owner.isAI)return;
        for(int i = 0; i < activeIndicators.Count; i++){ //destroy old indicators when mode is changed
            if(activeIndicators[i] == null)continue;
            Destroy(activeIndicators[i].gameObject);
        }
        activeIndicators.Clear();
        activeIndicators.TrimExcess();

        for(int i = 0; i < gameManager.allTargets.Count; i++){ //Create indicators and deactivate them, wait for object to be on camera to activate
            var a = Instantiate(objectIndicator, overlayHud.transform); 
            activeIndicators.Add(a);
            a.name = "Target_Indicator." + i;
            if(gameManager.allTargets[i].targetTeam == owner.teamInt) a.gameObject.GetComponent<Image>().color = ColorPaletteHelper.friendly;
            if(gameManager.allTargets[i].targetTeam != owner.teamInt) a.gameObject.GetComponent<Image>().color = ColorPaletteHelper.enemy;
            if(gameManager.allTargets[i].targetTeam == 2) a.gameObject.GetComponent<Image>().color = ColorPaletteHelper.global;
            if(gameManager.allTargets[i].targetTeam == 3) a.gameObject.GetComponent<Image>().color = ColorPaletteHelper.objective;
            
            a.gameObject.SetActive(false);
            a.GetComponent<Animator>().StopPlayback();
        }
    }

    private void PositionIndicators(){
        if(activeIndicators.Count != gameManager.allTargets.Count){
            GenerateIndicators();
        }
        if(gameManager.allTargets.Count <= 0 || activeIndicators.Count <= 0){ //return and try again if there are no targets
            GenerateIndicators();
            return;
        }

        for (int i = 0; i < activeIndicators.Count; i++){
            //Is the object active and on camera? If not, skip drawing the indicators.
            if(gameManager.allTargets[i] == null){
                gameManager.photonView.RPC("RemoveTarget", RpcTarget.All, i);
                gameManager.RemoveTarget(i);
                GenerateIndicators();
                return;
            }
            if(gameManager.allTargets[i].gameObject.activeInHierarchy == false || !gameManager.allTargets[i].mesh.isVisible){
                activeIndicators[i].gameObject.SetActive(false);
                continue;
            }
            if(gameManager.allTargets[i] == owner.targetableObject){
                activeIndicators[i].gameObject.SetActive(false);
                continue;
            }
            
            //raycast to check visibility
            int layermask = 1 << 14;
            RaycastHit hit;

            Vector3 origin = gameObject.transform.position + (transform.forward * 15);
            Vector3 dir = gameManager.allTargets[i].gameObject.transform.position - origin;
            Debug.DrawRay(origin, dir, Color.green);
            //if cast hits nothing, or the hit doesn't have a rigidbody, remove indicators
            

            if(!Physics.SphereCast(origin, 10, dir, out hit, 15000, ~layermask)){
                activeIndicators[i].gameObject.SetActive(false); 
                continue;
            }
            if(hit.rigidbody == null){
                activeIndicators[i].gameObject.SetActive(false); 
                continue;
            }
            //if object is not obstructed, activate indicators
            if(hit.rigidbody.gameObject == gameManager.allTargets[i].gameObject){
                activeIndicators[i].gameObject.SetActive(true);
                if(currentTarget == -1 && gameManager.allTargets[i].targetTeam.ToString() == targMode.ToString()){
                    currentTarget = i;
                }  
            }
            else{
                activeIndicators[i].gameObject.SetActive(false);
                continue;
            }
            
            //Position indicator to the indicated objects screen position
            var screen = Camera.main.WorldToScreenPoint(gameManager.allTargets[i].gameObject.transform.position);
            screen.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude;
            var targetScreenPosition = Camera.main.ScreenToWorldPoint(screen);
            activeIndicators[i].transform.position = Vector3.Slerp(activeIndicators[i].transform.position, targetScreenPosition, 60 * Time.deltaTime);
            //Animation
            if(i == currentTarget){
                activeIndicators[i].GetComponent<Animator>().StopPlayback();
            }
            if(i != currentTarget){
                activeIndicators[i].GetComponent<Animator>().StartPlayback();
            }

            activeIndicators[i].playerName.text = gameManager.allTargets[i].GetComponent<TargetableObject>().nameOfTarget; //Displays name
            activeIndicators[i].distance.text = (Vector3.Distance(transform.position, gameManager.allTargets[i].transform.position)).ToString(); //Displays distance
        }
        
    }

    private void LockPosition(){
        if(owner.isAI)return;
        if(currentTarget >= gameManager.allTargets.Count){
            currentTarget = 0;
            owner.shipBehaviour.lockOn.Stop();
            return;
        }
        if(currentTarget == -1){
            owner.shipBehaviour.lockOn.Stop();
            return;
        }
        if(gameManager.allTargets.Count <= 0){
            CycleMainTarget();
            return;
        }
        if(gameManager.allTargets[currentTarget] == owner.targetableObject){
            CycleMainTarget();
            return;
        }
        

        if(!gameManager.allTargets[currentTarget].gameObject.activeInHierarchy){
            CycleMainTarget();
            return;
        }

        Vector2 fovPosition = fovCam.WorldToViewportPoint(gameManager.allTargets[currentTarget].transform.position);

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
        Vector3 dir = gameManager.allTargets[currentTarget].gameObject.transform.position - origin;
        Debug.DrawRay(origin, dir, Color.red);
        if(!Physics.SphereCast(origin, 10, dir, out hit, 3500, ~layermask)){
            lockIndicator.gameObject.SetActive(false);
            lockedText.text = "";
            return;
        }
        else if(hit.rigidbody == null){
            CycleMainTarget();
        }
        else if(hit.rigidbody.gameObject == gameManager.allTargets[currentTarget].gameObject){
            
            lockIndicator.gameObject.SetActive(true);
            lockOnModifier += lockOnDefault * Time.fixedDeltaTime;
            

            

            var screen = Camera.main.WorldToScreenPoint(gameManager.allTargets[currentTarget].transform.position);
            screen.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude;
            var targetScreenPosition = Camera.main.ScreenToWorldPoint(screen);
            if(!owner.shipBehaviour.lockOn.isPlaying){
                owner.shipBehaviour.lockOn.Play();
            }
            Vector3 slowMove = Vector3.MoveTowards(lockIndicator.transform.position, targetScreenPosition, (lockOnEfficiency * lockOnModifier) * .25f * Time.deltaTime);
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
        //reset indicator and sounds
        owner.shipBehaviour.lockOn.Stop();
        lockedText.text = "";
        lockOnModifier = lockOnDefault;
        lockIndicator.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, (overlayHud.transform.position - Camera.main.transform.position).magnitude));
        lockIndicator.gameObject.SetActive(false);
        missileLocked = false;

        for(int i = 0; i < gameManager.allTargets.Count; i++){
            currentTarget++;
            //if current target is greater than or equal to all targets count
            if(currentTarget >= gameManager.allTargets.Count){
               currentTarget = 0;
            }
           
            if(!gameManager.allTargets[currentTarget].gameObject.activeInHierarchy || !gameManager.allTargets[i].mesh.isVisible 
            || gameManager.allTargets[currentTarget].targetTeam.ToString() != targMode.ToString() || gameManager.allTargets[currentTarget] == owner.targetableObject){
                if(i == gameManager.allTargets.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }

            //Raycast to check for obstructions
            RaycastHit hit;
            int layermask = 1 << 14;
            Vector3 origin = gameObject.transform.position + (transform.forward * 15);
            Vector3 dir = gameManager.allTargets[currentTarget].gameObject.transform.position - origin;
            //activeIndicators[currentTarget].next.gameObject.SetActive(false);
            Debug.DrawRay(origin, dir, Color.white);

            if(!Physics.SphereCast(origin, 10, dir, out hit, 15000, ~layermask)){
                if(i == gameManager.allTargets.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }
            else if(hit.rigidbody == false){
                if(i == gameManager.allTargets.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }
            else if(hit.rigidbody.gameObject == gameManager.allTargets[currentTarget].gameObject){
                
                activeIndicators[currentTarget].next.gameObject.SetActive(false);
                int n = currentTarget;
                for(int j = 0; j < gameManager.allTargets.Count; j++){
                    n++;
                    if(n >= gameManager.allTargets.Count){
                        n=0;
                    }
                    if(gameManager.allTargets[n].targetTeam.ToString() == targMode.ToString()){
                        activeIndicators[n].next.gameObject.SetActive(true);
                        break;
                    }
                }
                finalTarget = gameManager.allTargets[currentTarget];
                return;
            }
            else{
                if(i == gameManager.allTargets.Count - 1){
                    currentTarget = -1;
                }
                continue;
            }
        }
        if(currentTarget > -1){
            finalTarget = gameManager.allTargets[currentTarget];
        }
        else{
            finalTarget = null;
        }
        
    }
    #endregion

    #region weapon controls

    private void GunTemperature(){
        if(gunCharge > 0){
            gunCharge -= 0.0015f;
        }
        if(gunCharge <= 0){
            gunOvercharged = false;
        }
    }

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
            if(finalTarget != null)
            behaviour.target = finalTarget.gameObject;
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
