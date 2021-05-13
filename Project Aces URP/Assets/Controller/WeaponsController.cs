using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsController : MonoBehaviour
{
    public enum TargetingMode{
        TeamA,
        TeamB,
        Global
    }

    public TargetingMode targMode = TargetingMode.TeamA;
    public Canvas hud;
    public Image aimReticle;
    public GameObject objectIndicator, lockIndicator;
    public TMPro.TextMeshProUGUI textTargetMode;
    public List<GameObject> alltargetList, currentTargetSelection, activeIndicators;

    [Header("Gun Information")]
    public GameObject ammoType;
    public float fireRate = 0.1f,
        gunSpeed = 1000,
        gunSensitivity = 5,
        gunRange = 3;

    public Transform[] gunPosition;

    [Header("Missile Information")]
    public GameObject missileType;
    public float lockOnEfficiency = 2,
        lockOnRange = 2500,
        missileReload = 5;

    public Transform[] missilePosition;

    public AudioSource gunCannonAudio, missileCannonAudio;

    private int currentTMode = 0, currentMis = 0;

    private bool canFire = true, isTargetVisible = false, missileLocked = false, canLaunchMissile = true;


    private void OnEnable() {
        textTargetMode.text = ("Targeting Mode: " + targMode.ToString());
        gunCannonAudio = GetComponent<AudioSource>();
        GenerateIndicators();
    }
    public void ChangeTargetMode(){

        currentTMode += 1;

        if(currentTMode > 2){
            currentTMode = 0;
        }
        if(currentTMode == 0){
            targMode = TargetingMode.TeamA;
        }
        if(currentTMode == 1){
            targMode = TargetingMode.TeamB;
        }
        if(currentTMode == 2){
            targMode = TargetingMode.Global;
        }
        textTargetMode.text = ("Targeting Mode: " + targMode.ToString());

        for (int i = 0; i < alltargetList.Count; i++){
            if(alltargetList[i].GetComponent<TargetableObject>().targetTeam.ToString() == targMode.ToString()){
                currentTargetSelection.Add(alltargetList[i]);
            }
            if(alltargetList[i].GetComponent<TargetableObject>().targetTeam.ToString() != targMode.ToString()){
                currentTargetSelection.Remove(alltargetList[i]);
            }
        }

        GenerateIndicators();
    }

    private void OnTriggerEnter(Collider obj) {
        if(obj.gameObject.GetComponent<TargetableObject>() != null){
            if(currentTargetSelection.Contains(obj.gameObject))return;
            if(alltargetList.Contains(obj.gameObject)){
                currentTargetSelection.Add(obj.gameObject);
                GenerateIndicators();
            }
            else{
                alltargetList.Add(obj.gameObject);
                currentTargetSelection.Add(obj.gameObject);
                GenerateIndicators();

            }
        }
    }

    private void OnTriggerExit(Collider obj) {
        currentTargetSelection.Remove(obj.gameObject);
        GenerateIndicators();
    }

    public void GenerateIndicators(){
        for(int i = 0; i < activeIndicators.Count; i++){
            Destroy(activeIndicators[i]);
        }
        activeIndicators.Clear();
        for(int i = 0; i < currentTargetSelection.Count; i++){
            TargetableObject potentialTarget = currentTargetSelection[i].GetComponent<TargetableObject>();
            if(potentialTarget.targetTeam.ToString() != targMode.ToString()){
                currentTargetSelection.RemoveAt(i);
            }
            if(potentialTarget.targetTeam.ToString() == targMode.ToString()){
                var a = Instantiate(objectIndicator, hud.transform);
                activeIndicators.Add(a);
            }
            
        }
    }

    private void PositionIndicators(){
        for (int i = 0; i < activeIndicators.Count; i++){
            if(currentTargetSelection[i] == null){
                currentTargetSelection.RemoveAt(i);
                GenerateIndicators();
                return;
            }
            Vector3 targetPosition = Camera.main.WorldToScreenPoint(currentTargetSelection[i].transform.position);
            Text[] dataText = activeIndicators[i].GetComponentsInChildren<Text>();
            activeIndicators[i].transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z / 5);
            //Displays name
            dataText[0].text = currentTargetSelection[i].GetComponent<TargetableObject>().nameOfTarget;
            //Displays distance
            dataText[1].text = (Vector3.Distance(transform.position, currentTargetSelection[i].transform.position)).ToString();
        }
    }

    public void CycleMainTarget(){
        if(currentTargetSelection.Count <= 0)return;
        StopCoroutine(LockOn());
        isTargetVisible = false;
        missileLocked = false;
        var previousTarg = currentTargetSelection[0];
        int lastIndex = currentTargetSelection.Count - 1;
        currentTargetSelection.Remove(previousTarg);
        currentTargetSelection.Insert(lastIndex ,previousTarg);
        lockIndicator.transform.position = new Vector3(-10000,-10000,0);
    }

    private void PositionLockIndicator(){
        if(currentTargetSelection.Count <= 0){
            lockIndicator.SetActive(false);
            isTargetVisible = false;
            return;
        }
        if(isTargetVisible == false){
            lockIndicator.transform.position = Vector3.zero;
            lockIndicator.SetActive(false);
            return;
        }
        if(isTargetVisible == true){
            Vector3 obj = Camera.main.WorldToScreenPoint(currentTargetSelection[0].transform.position);
            Vector3 slowMove = Vector3.MoveTowards(lockIndicator.transform.position, obj, lockOnEfficiency * 5f);
            lockIndicator.transform.position = new Vector3(slowMove.x, slowMove.y, slowMove.z / 5);
            if(missileLocked == true && currentTargetSelection.Count > 0){
                Vector3 fastMove = Vector3.MoveTowards(lockIndicator.transform.position, obj, lockOnEfficiency * 100f);
                lockIndicator.transform.position = new Vector3(fastMove.x, fastMove.y, fastMove.z / 5);
            }
        }
        
    }

    private void DrawLockRay(){
        if(currentTargetSelection.Count <= 0){
            missileLocked = false;
            return;
        }
            RaycastHit hit;
            Vector3 dir = currentTargetSelection[0].transform.position - transform.position;
            Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if(Physics.SphereCast(offset, 1, dir, out hit, lockOnRange)){
                if(hit.collider.gameObject != currentTargetSelection[0].gameObject)return;
                print(hit.collider.gameObject.name);
                isTargetVisible = true;
                StartCoroutine(LockOn());
                //target is visible
            }
            else{
                print("Cancel Lock");
                isTargetVisible = false;
                missileLocked = false;
                //CycleMainTarget();
                StopCoroutine(LockOn());
                lockIndicator.SetActive(false);
                //target is not visible
        }
    }
    private IEnumerator LockOn(){
        lockIndicator.SetActive(true);
        yield return new WaitForSeconds(lockOnEfficiency);
        missileLocked = true;
    }

    private void FixedUpdate(){
        DrawLockRay();
    }

    private void LateUpdate() {
        PositionIndicators();
        PositionLockIndicator();
    }

    public void GunControl(Vector2 cursorInputPosition, bool gunInput, FloatData currentSpeed){
        //limit reticle position and speed
        Vector2 reticleDirection = new Vector2();
        var viewWidth = Camera.main.scaledPixelWidth;
        var viewHeight = Camera.main.scaledPixelHeight;
        reticleDirection = new Vector2(Mathf.Lerp(aimReticle.transform.position.x, cursorInputPosition.x, gunSensitivity*Time.deltaTime),
            Mathf.Lerp(aimReticle.transform.position.y, cursorInputPosition.y, gunSensitivity*Time.deltaTime));
        var x = Mathf.Clamp(reticleDirection.x, viewWidth/gunRange, viewWidth - viewWidth/gunRange);
        var y = Mathf.Clamp(reticleDirection.y, viewHeight/gunRange, viewHeight - viewHeight/gunRange);
        reticleDirection = new Vector2(x,y);

        aimReticle.transform.position = reticleDirection;
        //aim gun position towards reticle
        Ray ray = Camera.main.ScreenPointToRay(aimReticle.transform.position);
        for(int i = 0; i < gunPosition.Length; i++){
            gunPosition[i].LookAt(ray.GetPoint(3000));
        }
        
        if(canFire == true && gunInput == true){
            StartCoroutine(FireGun(gunInput, currentSpeed));
        }
    }

    public IEnumerator FireGun(bool gunIsFiring, FloatData currentSpeed){
        canFire = false;
        while(gunIsFiring){
            for(int i = 0; i < gunPosition.Length; i++){
                var g = Instantiate(ammoType);
                gunCannonAudio.Play();
                g.transform.position = gunPosition[i].position;
                g.transform.rotation = gunPosition[i].rotation;
                g.GetComponent<Rigidbody>().velocity = gunPosition[i].transform.forward * gunSpeed;
                yield return new WaitForSeconds(fireRate);
            }
            gunIsFiring = false;
        }
        canFire = true;
    }

    public void MissileControl(bool missileInput, FloatData currentSpeed){
        StartCoroutine(MissileLaunch(missileInput, currentSpeed));
    }

    public IEnumerator MissileReload(){
        canLaunchMissile = false;
        yield return new WaitForSeconds(missileReload);
        currentMis = 0;
        canLaunchMissile = true;

    }
    public IEnumerator MissileLaunch(bool missileInput, FloatData currentSpeed){
        if(missileInput == false)yield break;
        if(canLaunchMissile == false)yield break;
        canLaunchMissile = false;
        
        var m = Instantiate(missileType.gameObject, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
        m.GetComponent<MissileBehaviour>().currentSpeed = currentSpeed.value;


        if(missileLocked == true){
            m.gameObject.GetComponent<MissileBehaviour>().target = currentTargetSelection[0].transform;
        }

        yield return new WaitForSeconds(.5f);
        currentMis += 1;
        if(currentMis > missilePosition.Length - 1){
            StartCoroutine(MissileReload());
            yield break;
        }
        canLaunchMissile = true;


    }
}
