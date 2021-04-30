using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class SpacecraftController : MonoBehaviour, ControlInputActions.IFlightActions
{
    [Header("Spacecraft Objects")]
    public GameObject shipObject;
    public GameObject explosionObject;
    public GameObject gunAmmoObject;
    public GameObject missileObject;
    public Image gunReticle, aimRange;
    public Camera playerCamera;
    public Transform[] gunPosition, missilePosition;
    public Transform targetObj;

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
        gunSpeed = 1000;
    //player inputs 
    private float thrustInput, yawInput, brakeInput;
    private Vector2 torqueInput, cameraInput, screenOut, mousePosition;
    private bool canFire = true, gunIsFiring = false;

    private Rigidbody _rb;
    private ControlInputActions _controls;

    //Input System Setup-----------------------------------------------
    private void Awake(){
        _rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }
    private void OnEnable() {
        _controls = new ControlInputActions();
        _controls.Flight.SetCallbacks(this);
        _controls.Flight.Enable();
    }
    private void OnDisable() {
        _controls.Flight.Disable();
    }
    //Collision---------------------------------------------------------
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard")){
            print("crash");

           var deathLocation = transform.position;
           Object.Instantiate(explosionObject, deathLocation, transform.rotation);

           Destroy(gameObject);
        }
    }
    //Input Actions-----------------------------------------------------
    
    public void OnBrake(InputAction.CallbackContext value){
        brakeInput = value.ReadValue<float>();
    }
    public void OnMenuButton(InputAction.CallbackContext context){
        print("Menu button pressed");
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
         mousePosition = position.ReadValue<Vector2>();
    }
    public void OnGunFire(InputAction.CallbackContext pressed){
        gunIsFiring = pressed.ReadValueAsButton();
        if(gunIsFiring && canFire == true)
        StartCoroutine(FireGun());
    }

    //Player is controlled-----------------------------------------------
    private void FixedUpdate() 
    {
        ThrustControl();
        TorqueControl();

        gunReticle.transform.position = mousePosition;

        aimRange.transform.position = playerCamera.WorldToScreenPoint(targetObj.position);


        _rb.AddRelativeForce(0,0,currentSpeed.value);

        //shift speed to certain defaults
        if (gameObject.transform.localRotation.x < -60 && gameObject.transform.localRotation.x > -135)
        {
            currentSpeed.value = Mathf.Lerp(currentSpeed.value, minSpeed, .1f);
        }
        
        if (currentSpeed.value > cruiseSpeed)
        {
            currentSpeed.value = Mathf.Lerp(currentSpeed.value, cruiseSpeed, .001f);
        }
        
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, minSpeed, brakeInput * .01f);
    }

    private void ThrustControl()
    {
        var speed = currentSpeed.value + (thrustInput * acceleration);
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, speed, .005f);
        currentSpeed.value = Mathf.Clamp(currentSpeed.value, minSpeed, maxSpeed);

    }

    private void TorqueControl()
    {
         var highspeedhandling = currentSpeed.value/maxSpeed + 1;
        Vector3 torque  = new Vector3((torqueInput.y * pitch) / highspeedhandling, yawInput * yaw, (torqueInput.x * roll) / highspeedhandling);
        _rb.AddRelativeTorque(torque);
    }

    IEnumerator FireGun(){
        canFire = false;
        while(gunIsFiring){
            for(int i = 0; i < gunPosition.Length; i++){
                var g = Instantiate(gunAmmoObject, gunPosition[i].transform);
                g.GetComponent<Rigidbody>().velocity = gunPosition[i].transform.forward * gunSpeed;
                print("fire " + i);
                yield return new WaitForSeconds(fireRate);
            }
        }
        canFire = true;
    }
}
