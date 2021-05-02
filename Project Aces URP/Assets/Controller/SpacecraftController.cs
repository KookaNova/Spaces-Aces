using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class SpacecraftController : MonoBehaviour, ControlInputActions.IFlightActions
{
    [Header("Spacecraft Objects")]
    public GameObject firstCam;
    public GameObject shipObject, explosionObject, gunAmmoObject, missileObject, lockIndicator;
    public Canvas hudCanvas;
    public Image aimReticle;
    public Transform[] gunPosition, missilePosition;
    public Transform[] targetObj;

    [Header("Spacecraft Stats")]
    public FloatData currentSpeed;
    public float acceleration = 10,
        minSpeed = 10,
        cruiseSpeed = 150,
        maxSpeed = 200,
        roll = 5, 
        pitch = 7, 
        yaw = 3,
        fireRate = 0.5f,
        gunSpeed = 1000,
        gunSensitivity = 1,
        gunRange = 3,
        lockDistance = 3000;
    //player inputs 
    private float thrustInput, yawInput, brakeInput;
    private Vector2 torqueInput, cameraInput, aimReticlePosition;
    //Utility Inputs
    private bool canFire = true, gunIsFiring = false;
    private Rigidbody _rb;
    private ControlInputActions _controls;

    //Input System Setup-----------------------------------------------
    private void Awake(){
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable() {
        _controls = new ControlInputActions();
        _controls.Flight.SetCallbacks(this);
        _controls.Flight.Enable();
        Cursor.visible = false;
    }
    private void OnDisable() {
        _controls.Flight.Disable();
    }
    //Collision---------------------------------------------------------
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy Player")){
            print("crash");

           var deathLocation = transform.position;
           Object.Instantiate(explosionObject, deathLocation, transform.rotation);

           Destroy(gameObject);
        }
    }
    //Input Actions-----------------------------------------------------
    
    public void OnMenuButton(InputAction.CallbackContext context){}
    public void OnCameraChange(InputAction.CallbackContext context){
        firstCam.SetActive(context.ReadValueAsButton());
    }
    public void OnBrake(InputAction.CallbackContext value){
        brakeInput = value.ReadValue<float>();
    }
    public void OnThrust(InputAction.CallbackContext value){
        thrustInput = value.ReadValue<float>();
    }
    public void OnTorque(InputAction.CallbackContext value){
        torqueInput = value.ReadValue<Vector2>();
    }
    public void OnYaw(InputAction.CallbackContext value){
        yawInput = value.ReadValue<float>();
    }
     public void OnAimGun(InputAction.CallbackContext position){
        aimReticlePosition = position.ReadValue<Vector2>();
    }
    public void OnGunFire(InputAction.CallbackContext pressed){
        gunIsFiring = pressed.ReadValueAsButton();
        if(gunIsFiring && canFire == true)
        StartCoroutine(FireGun());
    }

    //Player is controlled-----------------------------------------------
    private void FixedUpdate() {
        ThrustControl();
        TorqueControl();
        GunControl();

        _rb.AddRelativeForce(0,0,currentSpeed.value);

        if (currentSpeed.value > cruiseSpeed)
        {
            currentSpeed.value = Mathf.Lerp(currentSpeed.value, cruiseSpeed, .001f);
        }
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, minSpeed, brakeInput * .01f);
    }

    private void ThrustControl(){
        var speed = currentSpeed.value + (thrustInput * acceleration);
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, speed, Time.deltaTime);
        currentSpeed.value = Mathf.Clamp(currentSpeed.value, minSpeed, maxSpeed);
    }

    private void TorqueControl(){
         var highspeedhandling = currentSpeed.value/maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * pitch) / highspeedhandling, yawInput * yaw, (torqueInput.x * roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce);
    }

    private void Targeting(){
        int currentTarget = 0;
        if(currentTarget > targetObj.Length){
            currentTarget = 0;
        }
        lockIndicator.transform.position = Camera.main.WorldToScreenPoint(targetObj[currentTarget].position);
    }

    private void GunControl(){
        //limit reticle position and speed
        Vector2 reticleDirection = new Vector2();
        var viewWidth = Camera.main.scaledPixelWidth;
        var viewHeight = Camera.main.scaledPixelHeight;
        reticleDirection = new Vector2(Mathf.Lerp(aimReticle.transform.position.x, aimReticlePosition.x, gunSensitivity*Time.deltaTime),
            Mathf.Lerp(aimReticle.transform.position.y, aimReticlePosition.y, gunSensitivity*Time.deltaTime));
        var x = Mathf.Clamp(reticleDirection.x, viewWidth/gunRange, viewWidth - viewWidth/gunRange);
        var y = Mathf.Clamp(reticleDirection.y, viewHeight/gunRange, viewHeight - viewHeight/gunRange);
        reticleDirection = new Vector2(x,y);

        aimReticle.transform.position = reticleDirection;
        //aim gun position towards reticle
        Ray ray = Camera.main.ScreenPointToRay(aimReticle.transform.position);
        for(int i = 0; i < gunPosition.Length; i++){
            gunPosition[i].LookAt(ray.GetPoint(10000f));
        }
    }

    IEnumerator FireGun(){
        canFire = false;
        while(gunIsFiring){
            for(int i = 0; i < gunPosition.Length; i++){
                var g = Instantiate(gunAmmoObject);
                g.transform.position = gunPosition[i].position;
                g.transform.rotation = gunPosition[i].rotation;
                g.GetComponent<Rigidbody>().velocity = gunPosition[i].transform.forward *(currentSpeed.value + gunSpeed);
                yield return new WaitForSeconds(fireRate);
            }
        }
        canFire = true;
    }
}
