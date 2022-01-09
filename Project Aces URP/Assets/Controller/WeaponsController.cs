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
    #region Serialized Fields
    public enum TargetingMode{
        TeamA,
        TeamB,
        Global
    }
    [HideInInspector] public SpacecraftController owner = null;

    [HideInInspector] public TargetingMode targMode = TargetingMode.TeamA;
    [SerializeField] private Canvas worldHud, overlayHud;
    [SerializeField] private Image aimReticle, distanceReticle;
    [SerializeField] private GameObject objectIndicator, lockIndicator;
    [SerializeField] private List<TMPro.TextMeshProUGUI> textTargetMode, missileCountText;
    [SerializeField] private List<GameObject> activeIndicators;
    [SerializeField] private Camera fovCam;
    #endregion

    #region Public Weapon Fields
    [Header("Gun Information")]
    [SerializeField] private GameObject ammoType;
    [SerializeField] private float fireRate = 0.1f,
        gunSpeed = 1000,
        gunRange = 3000;

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
    public List<TargetableObject> allTargetList, currentTargetSelection;
    [HideInInspector] public int currentMis = 0, currentTarget = 0;
    [HideInInspector] public float gunModifier = 0, missileModifier = 0;
    private int missilesAvailable;
    private float lockOnModifier, lockOnDefault = 0.01f;

    public bool missileLocked = false;
    private bool canFire = true, isTargetVisible = false, canLaunchMissile = true;

    #endregion

    #region enable
    public void EnableWeapons() {
        currentTarget = 0;
        lockOnModifier = lockOnDefault;
        overlayHud.transform.SetParent(null);
        missilesAvailable = missilePosition.Length;

        var l = Instantiate(lockIndicator, parent: overlayHud.transform);
        lockIndicator = l;
        lockIndicator.SetActive(false);
        


        FindTargets();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        FindTargets();
    }
    private void UpdateHUD(){
        for(int i = 0; i < missileCountText.Count; i++){
            missileCountText[i].text = missilesAvailable.ToString();
        }
    }
    
    #endregion

    #region targeting
    private void LateUpdate()
    {
        if(photonView.IsMine){
        PositionIndicators();
        LockPosition();
        AimGun();
        }
    }
    
    private void FindTargets(){
        allTargetList.Clear();
        var targets = GameObject.FindObjectsOfType<TargetableObject>();
        for (int i = 0; i < targets.Length; i++){
            if(targets[i] != this.GetComponentInParent<TargetableObject>()){
                allTargetList.Add(targets[i]);
            }
        }
        allTargetList.TrimExcess();
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
        for(int i = 0; i < textTargetMode.Count; i++){
            textTargetMode[i].text = ("Targeting " + targMode.ToString());
        }
        CleanTargetSelection();
    
    }
    private void CleanTargetSelection(){
        if(allTargetList.Count > 0)
        currentTargetSelection.Clear();
        for (int i = 0; i < allTargetList.Count; i++){
            if(allTargetList[i].targetTeam.ToString() == targMode.ToString()){
                currentTargetSelection.Add(allTargetList[i]);
            }
        }
        GenerateIndicators();
    }

    private void GenerateIndicators(){
        //destroy old indicators when mode is changed
        for(int i = 0; i < activeIndicators.Count; i++){
            Destroy(activeIndicators[i]);
        }
        activeIndicators.Clear();
        activeIndicators.TrimExcess();

        //Create indicators and deactivate them, wait for object to be on camera to activate
        for(int i = 0; i < currentTargetSelection.Count; i++){
            var a = Instantiate(objectIndicator, overlayHud.transform);
            activeIndicators.Add(a);
            a.name = "Target_Indicator." + i;
            a.SetActive(false);
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
            if(currentTargetSelection[i].gameObject.activeInHierarchy == false || !currentTargetSelection[i].GetComponentInChildren<MeshRenderer>().isVisible){
                activeIndicators[i].SetActive(false);
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
                activeIndicators[i].SetActive(false); 
                Debug.LogFormat("WeaponsSystem: PositionIndicators(), target {0} not visible. Indicator is inactive.", currentTargetSelection[i].nameOfTarget);
                continue;
            }
            if(hit.rigidbody == null){
                activeIndicators[i].SetActive(false); 
                Debug.LogFormat("WeaponsSystem: PositionIndicators(), No rigidbody found on target {0}. Indicator is inactive.", currentTargetSelection[i].name);
                continue;
            }
            
            //if object is not obstructed, activate indicators
            if(hit.rigidbody.gameObject == currentTargetSelection[i].gameObject){
                activeIndicators[i].SetActive(true);
                if(currentTarget == -1){
                currentTarget = i;
                }  
            }
            else{
                activeIndicators[i].SetActive(false);
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

            //Write text for name and distance
            Text[] dataText = activeIndicators[i].GetComponentsInChildren<Text>();
            //Displays name
            dataText[0].text = currentTargetSelection[i].GetComponent<TargetableObject>().nameOfTarget;
            //Displays distance
            dataText[1].text = (Vector3.Distance(transform.position, currentTargetSelection[i].transform.position)).ToString();
        }
        
    }

    private void LockPosition(){
        if(currentTarget >= currentTargetSelection.Count){
            currentTarget = 0;
            return;
        }
        if(currentTarget == -1){
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
            lockIndicator.SetActive(false);
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
            lockIndicator.SetActive(false);
            return;
        }
        else if(hit.rigidbody == null){
            CycleMainTarget();
            Debug.LogFormat("WeaponsSystem: LockPosition(), No rigidbody found on target {0}. Indicator is inactive.", currentTargetSelection[currentTarget].name);
        }
        else if(hit.rigidbody.gameObject == currentTargetSelection[currentTarget].gameObject){
            
            lockIndicator.SetActive(true);
            lockOnModifier += lockOnDefault * Time.deltaTime;

            

            var screen = Camera.main.WorldToScreenPoint(currentTargetSelection[currentTarget].transform.position);
            screen.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude;
            var targetScreenPosition = Camera.main.ScreenToWorldPoint(screen);

            Vector3 slowMove = Vector3.MoveTowards(lockIndicator.transform.position, targetScreenPosition, (lockOnEfficiency * lockOnModifier) * Time.deltaTime);
            lockIndicator.transform.position = slowMove;

            
            
            if(lockIndicator.transform.position == targetScreenPosition){
                missileLocked = true;
                lockOnModifier = 5000;
            }
            else{
                missileLocked = false;
            }
        }
        else{
            CycleMainTarget();
            
        }
    }

    public void CycleMainTarget(){

        lockOnModifier = lockOnDefault;
        lockIndicator.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, (overlayHud.transform.position - Camera.main.transform.position).magnitude));
        lockIndicator.SetActive(false);
        missileLocked = false;

        for(int i = 0; i < currentTargetSelection.Count; i++){
            currentTarget++;

            if(currentTarget >= currentTargetSelection.Count){
               currentTarget = 0;
            }
           
            if(!currentTargetSelection[i].GetComponentInChildren<MeshRenderer>().isVisible || !currentTargetSelection[currentTarget].gameObject.activeInHierarchy){
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
        distanceReticle.transform.position = position;


        for(int i = 0; i < gunPosition.Length; i++){
            gunPosition[i].LookAt(hitPoint);
        }
    }

    public void GunControl(bool gunInput, float currentSpeed){
        
        if(canFire == true && gunInput == true){
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
            for(int i = 0; i < gunPosition.Length; i++){
                var g = PhotonNetwork.Instantiate(ammoType.name, gunPosition[i].position, gunPosition[i].rotation);
                gunCannonAudio.Play();
                g.GetComponent<Rigidbody>().velocity = gunPosition[i].transform.forward * gunSpeed;
                var behaviour = g.GetComponent<GunAmmoBehaviour>();
                behaviour.damageOutput += gunModifier;
                behaviour.owner = owner;
                yield return new WaitForSeconds(fireRate);
            }
            gunIsFiring = false;
        }
        canFire = true;
    }

    private IEnumerator MissileReload(){
        canLaunchMissile = false;
        yield return new WaitForSeconds(missileReload);
        missilesAvailable ++;
        UpdateHUD();
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
            UpdateHUD();
        }
        else{
            var m = PhotonNetwork.Instantiate(missileType.gameObject.name, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
            var behaviour = m.GetComponent<MissileBehaviour>();
            behaviour.currentSpeed = currentSpeed;
            behaviour.damageOutput += missileModifier;
            missilesAvailable--;
            UpdateHUD();
        }
        StartCoroutine(MissileReload());

        yield return new WaitForSeconds(.5f);
        currentMis++;
        if(currentMis > missilePosition.Length - 1){
            currentMis = 0;
        }
        canLaunchMissile = true;
    }
    #endregion
}
}
