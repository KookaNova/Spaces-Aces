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
    public List<GameObject> activeIndicators;
    public List<TargetableObject> allTargetList, currentTargetSelection;

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

    private int currentMis = 0;

    private bool canFire = true, isTargetVisible = false, missileLocked = false, canLaunchMissile = true;


    private void OnEnable() {
        textTargetMode.text = ("Targeting Mode: " + targMode.ToString());
        gunCannonAudio = GetComponent<AudioSource>();
        FindTargets();
    }
    private void LateUpdate() {
        PositionIndicators();
        DrawLockRay();
        PositionLockIndicator();
    }

    //Targeting
    private void FindTargets(){
        allTargetList.Clear();
        var targets = GameObject.FindObjectsOfType<TargetableObject>();
        for (int i = 0; i < targets.Length; i++){
            if(targets[i] != gameObject.GetComponentInParent<TargetableObject>()){
                allTargetList.Add(targets[i]);
            }
        }
        allTargetList.TrimExcess();
        GenerateIndicators();
    }
    public void GenerateIndicators(){
        for(int i = 0; i < activeIndicators.Count; i++){
            Destroy(activeIndicators[i]);
        }
        activeIndicators.Clear();
        activeIndicators.TrimExcess();
        for(int i = 0; i < currentTargetSelection.Count; i++){
            if(currentTargetSelection[i].targetTeam.ToString() != targMode.ToString()){
                currentTargetSelection.RemoveAt(i);
            }
            if(currentTargetSelection[i].targetTeam.ToString() == targMode.ToString()){
                var a = Instantiate(objectIndicator, hud.transform);
                activeIndicators.Add(a);
            }
        }
        currentTargetSelection.TrimExcess();
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
        textTargetMode.text = ("Targeting Mode: " + targMode.ToString());
        if(allTargetList.Count > 0)
        currentTargetSelection.Clear();
        for (int i = 0; i < allTargetList.Count; i++){
            if(allTargetList[i].targetTeam.ToString() == targMode.ToString()){
                currentTargetSelection.Add(allTargetList[i]);
            }
        }
        GenerateIndicators();
    }


    private void PositionIndicators(){
        if(currentTargetSelection.Count <= 0)return;
        for (int i = 0; i < activeIndicators.Count; i++){
            if(currentTargetSelection[i] == null){
                GenerateIndicators();
                return;
            }
            RaycastHit hit;
            int layermask = 1 << 14;
            Vector3 dir = currentTargetSelection[i].gameObject.transform.position - gameObject.transform.position;
            Physics.SphereCast(gameObject.transform.position, 10, dir, out hit, 5000, ~layermask);
            Debug.DrawRay(gameObject.transform.position, dir, Color.cyan);

            if(currentTargetSelection[i].gameObject.GetComponentInChildren<Renderer>().isVisible == false || currentTargetSelection[i].gameObject.activeSelf == false || currentTargetSelection[i].gameObject != hit.collider.gameObject){
                activeIndicators[i].gameObject.SetActive(false);
                missileLocked = false;
            }
            else{
                activeIndicators[i].gameObject.SetActive(true);
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
        int lastIndex = currentTargetSelection.Count - 1 ;
        currentTargetSelection.Remove(previousTarg);
        currentTargetSelection.Insert(lastIndex, previousTarg);
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
            if(lockIndicator.transform.position == obj){
                missileLocked = true;
                Vector3 fastMove = Vector3.MoveTowards(lockIndicator.transform.position, obj, lockOnEfficiency * 500f);
                lockIndicator.transform.position = obj;
            }
        }
    }

    //Weapon Controls
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

    private void DrawLockRay(){
        if(currentTargetSelection.Count <= 0 || currentTargetSelection[0].gameObject == null){
            missileLocked = false;
            return;
        }

        if(currentTargetSelection[0].gameObject.GetComponentInChildren<Renderer>().isVisible){
            isTargetVisible = true;
            //StartCoroutine(LockOn());
            //target is visible
        }
        else{
            isTargetVisible = false;
            missileLocked = false;
            //CycleMainTarget();
            StopCoroutine(LockOn());
            lockIndicator.SetActive(false);
            //target is not visible
        }
    }

    public void MissileControl(bool missileInput, FloatData currentSpeed){
        StartCoroutine(MissileLaunch(missileInput, currentSpeed));
    }

    //IEnumerators
    private IEnumerator LockOn(){
        lockIndicator.SetActive(true);
        yield return new WaitForSeconds(lockOnEfficiency);
        missileLocked = true;
    }

    private IEnumerator FireGun(bool gunIsFiring, FloatData currentSpeed){
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

    private IEnumerator MissileReload(){
        canLaunchMissile = false;
        yield return new WaitForSeconds(missileReload);
        currentMis = 0;
        canLaunchMissile = true;

    }
    private IEnumerator MissileLaunch(bool missileInput, FloatData currentSpeed){
        if(missileInput == false)yield break;
        if(canLaunchMissile == false)yield break;
        canLaunchMissile = false;
        
        var m = Instantiate(missileType.gameObject, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
        m.GetComponent<MissileBehaviour>().currentSpeed = currentSpeed.value;


        if(missileLocked == true){
            m.gameObject.GetComponent<MissileBehaviour>().target = currentTargetSelection[0].gameObject;
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
