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
    public GameObject targetIndicator;
    public TMPro.TextMeshProUGUI textTargetMode;
    public List<GameObject> targetList, activeIndicators;

    [Header("Gun Information")]
    public GameObject ammoType;
    public float fireRate = 0.1f,
        gunSpeed = 1000,
        gunSensitivity = 5,
        gunRange = 3;

    public Transform[] gunPosition;

    [Header("Missile Information")]
    public GameObject missileType;
    public float lockOnEfficiency;

    public Transform[] missilePosition;

    private int currentTMode = 0, currentTarget = 0;

    private bool canFire = true, isTargetVisible = false, missileLocked = false;


    private void Awake() {
        textTargetMode.text = ("Targeting Mode: " + targMode.ToString());
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
        GenerateIndicators();
    }

    private void OnTriggerEnter(Collider obj) {
        if(obj.GetComponent<TargetableObject>() == null) return;
        targetList.Add(obj.gameObject);
        GenerateIndicators();
    }

    private void OnTriggerExit(Collider obj) {
        if(targetList.Contains(obj.gameObject)){
            targetList.Remove(obj.gameObject);
            GenerateIndicators();
        }
    }

    public void GenerateIndicators(){
        for(int i = 0; i < activeIndicators.Count; i++){
            Destroy(activeIndicators[i]);
        }
        activeIndicators.Clear();
        for(int i = 0; i < targetList.Count; i++){
            TargetableObject potentialTarget = targetList[i].GetComponent<TargetableObject>();
            if(potentialTarget.targetTeam.ToString() != targMode.ToString())return;
            var a = Instantiate(targetIndicator, hud.transform);
            activeIndicators.Add(a);
        }
    }

    private void PositionIndicators(){
        for (int i = 0; i < activeIndicators.Count; i++){
            if(targetList[i] == null){
                targetList.Remove(targetList[i].gameObject);
                GenerateIndicators();
                return;
            }
            Vector3 targetPosition = Camera.main.WorldToScreenPoint(targetList[i].transform.position);
            Text distanceText = activeIndicators[i].GetComponentInChildren<Text>();
            activeIndicators[i].transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z / 5);
            distanceText.text = (Vector3.Distance(transform.position, targetList[i].transform.position)).ToString();
        }

        if(currentTarget > targetList.Count){
            currentTarget = 0;
        }

        //lockIndicator.transform.position = Camera.main.WorldToScreenPoint(targetObj[currentTarget].position);
    }


    private void FixedUpdate() {
        PositionIndicators();

        RaycastHit hit;

        if(targetList.Count > 0){
            if (Physics.SphereCast(transform.position, 50, targetList[currentTarget].transform.position , out hit, 20))
        {

        }
        else
        {
            missileLocked = false;
        }
        
        Debug.DrawLine(transform.position ,targetList[currentTarget].transform.position, Color.red);
        }

    }

    private IEnumerator LockOn(int currentTarget){
        if (isTargetVisible == false) yield break;
        yield return new WaitForSeconds(lockOnEfficiency);
        if (isTargetVisible == false) yield break;
        missileLocked = true;
    }

    public void GunControl(Vector2 cursorInputPosition, bool gunIsFiring, FloatData currentSpeed){
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
            gunPosition[i].LookAt(ray.GetPoint(10000f));
        }
        
        if(canFire == true && gunIsFiring == true){
            StartCoroutine(FireGun(gunIsFiring, currentSpeed));
        }
    }

    public IEnumerator FireGun(bool gunIsFiring, FloatData currentSpeed){
        canFire = false;
        while(gunIsFiring){
            for(int i = 0; i < gunPosition.Length; i++){
                var g = Instantiate(ammoType);
                g.transform.position = gunPosition[i].position;
                g.transform.rotation = gunPosition[i].rotation;
                g.GetComponent<Rigidbody>().velocity = gunPosition[i].transform.forward *(currentSpeed.value + gunSpeed);
                yield return new WaitForSeconds(fireRate);
            }
            gunIsFiring = false;
        }
        canFire = true;
    }
}
