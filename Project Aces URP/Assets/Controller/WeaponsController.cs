using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WeaponsController : MonoBehaviourPunCallbacks
{
    #region Serialized Fields
    public enum TargetingMode{
        TeamA,
        TeamB,
        Global
    }

    
    [SerializeField] private TargetingMode targMode = TargetingMode.TeamA;
    [SerializeField] private Canvas worldHud, overlayHud;
    [SerializeField] private Image aimReticle;
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
        gunSensitivity = 5,
        gunRange = 3000;

    [SerializeField] private Transform[] gunPosition;

    [Header("Missile Information")]
    [SerializeField] private GameObject missileType;
    [SerializeField] private float lockOnEfficiency = 2,
        lockOnRange = 2500,
        missileReload = 5;

    [SerializeField] private Transform[] missilePosition;

    public AudioSource gunCannonAudio, missileCannonAudio;
    #endregion

    #region Hidden Fields
    [HideInInspector] public List<TargetableObject> allTargetList, currentTargetSelection;
    private int currentMis = 0;
    private int missilesAvailable;
    private float lockOnModifier = 5;

    private bool canFire = true, isTargetVisible = false, missileLocked = false, canLaunchMissile = true;

    #endregion

    #region enable
    public void EnableWeapons() {
        gunCannonAudio = GetComponent<AudioSource>();
        overlayHud.transform.SetParent(null);
        missilesAvailable = missilePosition.Length;

        var l = Instantiate(lockIndicator, parent: overlayHud.transform);
        lockIndicator = l;
        lockIndicator.SetActive(false);
        for(int i = 0; i < textTargetMode.Count; i++){
            textTargetMode[i].text = ("Targeting: " + targMode.ToString());
        }
        


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
    private void LateUpdate() {
        if(photonView.IsMine){
        PositionIndicators();
        LockPosition();
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
            activeIndicators[i].SetActive(false);
        }
        currentTargetSelection.TrimExcess();
    }

    private void PositionIndicators(){
        if(activeIndicators.Count <= 0)return;
        for (int i = 0; i < activeIndicators.Count; i++){
            int layermask = 1 << 14;
            RaycastHit hit;
            Vector3 dir = currentTargetSelection[i].gameObject.transform.position - gameObject.transform.position;
            //if cast hits nothing, remove indicators
            if(!Physics.SphereCast(gameObject.transform.position, 10, dir, out hit, 7500, ~layermask)){activeIndicators[i].SetActive(false); return;}
            
            //if object is visible and not obstructed, activate indicators
            if(Physics.SphereCast(gameObject.transform.position, 10, dir, out hit, 7500, ~layermask) && currentTargetSelection[i].GetComponentInChildren<Renderer>().isVisible && hit.rigidbody.gameObject == currentTargetSelection[i].gameObject){
                activeIndicators[i].SetActive(true);
            }
            else{
                activeIndicators[i].SetActive(false);
            }

            Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(currentTargetSelection[i].transform.position);
            activeIndicators[i].transform.position = targetScreenPosition;

            Text[] dataText = activeIndicators[i].GetComponentsInChildren<Text>();
            activeIndicators[i].transform.position = new Vector3(targetScreenPosition.x, targetScreenPosition.y, 0);
            //Displays name
            dataText[0].text = currentTargetSelection[i].GetComponent<TargetableObject>().nameOfTarget;
            //Displays distance
            dataText[1].text = (Vector3.Distance(transform.position, currentTargetSelection[i].transform.position)).ToString();
        }
    }

    private void LockPosition(){
        if(currentTargetSelection.Count <= 0){lockIndicator.SetActive(false); return;}

        RaycastHit hit;
        int layermask = 1 << 14;
        Vector3 dir = currentTargetSelection[0].gameObject.transform.position - gameObject.transform.position;
        if(!Physics.SphereCast(gameObject.transform.position, 10, dir, out hit, 3500, ~layermask)){lockIndicator.SetActive(false); CycleMainTarget(); return;};

        if(Physics.SphereCast(gameObject.transform.position, 10, dir, out hit, 3500, ~layermask) && currentTargetSelection[0].GetComponentInChildren<Renderer>().isVisible && hit.rigidbody.gameObject == currentTargetSelection[0].gameObject){
            lockIndicator.SetActive(true);
            lockOnModifier += 6 * Time.deltaTime;
            Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(currentTargetSelection[0].transform.position);
            Vector3 slowMove = Vector3.MoveTowards(lockIndicator.transform.position, targetScreenPosition, lockOnEfficiency * lockOnModifier);
            lockIndicator.transform.position = new Vector3(slowMove.x, slowMove.y, 0);
            
            if(Mathf.RoundToInt(lockIndicator.transform.position.x) == Mathf.RoundToInt(targetScreenPosition.x) && Mathf.RoundToInt(lockIndicator.transform.position.y) == Mathf.RoundToInt(targetScreenPosition.y)){
                missileLocked = true;
                lockOnModifier = 5000;
            }
            else{
                missileLocked = false;
            }
        }
        else{
            CycleMainTarget();
            lockOnModifier = 6;
            lockIndicator.transform.position = Vector3.zero;
            lockIndicator.SetActive(false);
            missileLocked = false;
        }
    }

    public void CycleMainTarget(){
        if(currentTargetSelection.Count <= 0)return;
        isTargetVisible = false;
        missileLocked = false;
        lockOnModifier = 6;
        var previousTarg = currentTargetSelection[0];
        int lastIndex = currentTargetSelection.Count - 1 ;
        currentTargetSelection.Remove(previousTarg);
        currentTargetSelection.Insert(lastIndex, previousTarg);
        lockIndicator.transform.position = Vector3.zero;
    }
    #endregion

    #region weapon controls
    public void GunControl(bool gunInput, float currentSpeed){
        //aim gun position towards reticle
        Ray ray = new Ray(aimReticle.transform.position, aimReticle.transform.forward);
        for(int i = 0; i < gunPosition.Length; i++){
            gunPosition[i].LookAt(ray.GetPoint(gunRange));
        }
        
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
            var m = Instantiate(missileType.gameObject, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
            var behaviour = m.GetComponent<MissileBehaviour>();
            behaviour.currentSpeed = currentSpeed;
            behaviour.target = currentTargetSelection[0].gameObject;
            missilesAvailable--;
            UpdateHUD();
        }
        else{
            var m = Instantiate(missileType.gameObject, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
            var behaviour = m.GetComponent<MissileBehaviour>();
            behaviour.currentSpeed = currentSpeed;
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
