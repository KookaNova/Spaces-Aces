﻿using System;
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
    private float lockOnModifier = 5;

    private bool canFire = true, isTargetVisible = false, missileLocked = false, canLaunchMissile = true;


    public void EnableWeapons() {
        gunCannonAudio = GetComponent<AudioSource>();

        var h = Instantiate(hud);
        hud = h;
        var r = Instantiate(aimReticle, parent: hud.transform);
        aimReticle = r;
        var l = Instantiate(lockIndicator, parent: hud.transform);
        lockIndicator = l;
        lockIndicator.SetActive(false);
        textTargetMode = hud.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        textTargetMode.text = ("Targeting Mode: " + targMode.ToString());


        FindTargets();
    }
    private void LateUpdate() {
        PositionIndicators();
        LockPosition();
    }

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
        for(int i = 0; i < activeIndicators.Count; i++){
            Destroy(activeIndicators[i]);
        }
        activeIndicators.Clear();
        activeIndicators.TrimExcess();
        for(int i = 0; i < currentTargetSelection.Count; i++){
            var a = Instantiate(objectIndicator, hud.transform);
            activeIndicators.Add(a);
            activeIndicators[i].SetActive(false);
        }
        currentTargetSelection.TrimExcess();
    }

    private void PositionIndicators(){
        if(activeIndicators.Count <= 0){};
        for (int i = 0; i < activeIndicators.Count; i++){

            RaycastHit hit;
            int layermask = 1 << 14;
            Vector3 dir = currentTargetSelection[i].gameObject.transform.position - gameObject.transform.position;
            if(Physics.SphereCast(gameObject.transform.position, 25, dir, out hit, 5000, ~layermask) != true){activeIndicators[i].SetActive(false); return;}
            
            if(currentTargetSelection[i].GetComponentInChildren<Renderer>().isVisible && hit.collider.gameObject == currentTargetSelection[i].gameObject){
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
        if(Physics.SphereCast(gameObject.transform.position, 25, dir, out hit, 5000, ~layermask) != true){lockIndicator.SetActive(false); CycleMainTarget(); return;};

        if(currentTargetSelection[0].GetComponentInChildren<Renderer>().isVisible && hit.collider.gameObject == currentTargetSelection[0].gameObject){
            lockIndicator.SetActive(true);
            lockOnModifier += 5 * Time.deltaTime;
            Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(currentTargetSelection[0].transform.position);
            Vector3 slowMove = Vector3.MoveTowards(lockIndicator.transform.position, targetScreenPosition, lockOnEfficiency * lockOnModifier);
            lockIndicator.transform.position = new Vector3(slowMove.x, slowMove.y, 0);
            
            if(Mathf.RoundToInt(lockIndicator.transform.position.x) == Mathf.RoundToInt(targetScreenPosition.x) && Mathf.RoundToInt(lockIndicator.transform.position.y) == Mathf.RoundToInt(targetScreenPosition.y)){
                missileLocked = true;
                lockOnModifier = 5000;
                Debug.Log("Target Locked");
            }
            else{
                missileLocked = false;
            }
        }
        else{
            CycleMainTarget();
            lockOnModifier = 5;
            lockIndicator.transform.position = Vector3.zero;
            lockIndicator.SetActive(false);
            missileLocked = false;
        }
    }

    public void CycleMainTarget(){
        if(currentTargetSelection.Count <= 0)return;
        isTargetVisible = false;
        missileLocked = false;
        lockOnModifier = 5;
        var previousTarg = currentTargetSelection[0];
        int lastIndex = currentTargetSelection.Count - 1 ;
        currentTargetSelection.Remove(previousTarg);
        currentTargetSelection.Insert(lastIndex, previousTarg);
        lockIndicator.transform.position = Vector3.zero;
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

    public void MissileControl(bool missileInput, FloatData currentSpeed){
        StartCoroutine(MissileLaunch(missileInput, currentSpeed));
    }

    //IEnumerators

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

        if(missileLocked == true){
            var m = Instantiate(missileType.gameObject, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
            m.GetComponent<MissileBehaviour>().currentSpeed = currentSpeed.value;
            m.gameObject.GetComponent<MissileBehaviour>().target = currentTargetSelection[0].gameObject;
        }
        else{
            var m = Instantiate(missileType.gameObject, missilePosition[currentMis].position, missilePosition[currentMis].rotation);
            m.GetComponent<MissileBehaviour>().currentSpeed = currentSpeed.value;
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