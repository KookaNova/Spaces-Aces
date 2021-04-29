using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class SpacecraftController : MonoBehaviour, ControlInputActions.IFlightActions
{
    
     [Header("Aircraft Stats")]
     public FloatData currentSpeed;
     public float acceleration = 10,
        minSpeed = 10,
        cruiseSpeed = 150,
        maxSpeed = 200,
        roll = 5, 
        pitch = 7, 
        yaw = 3;
    //player inputs 
    private float thrustInput, yawInput, brakeInput;
    private Vector2 torqueInput, cameraInput;

    [Header("Explosion Type")]
    public GameObject deathSpot;
    private Rigidbody _rb;
    private ControlInputActions _controls;

    private void Awake(){
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable() 
    {

         if (_controls == null)
        {
            _controls = new ControlInputActions();
            /*Tell the "gameplay" action map that we want to get told about
            when actions get triggered.*/
            _controls.Flight.SetCallbacks(this);
        }
        _controls.Flight.Enable();
    }
    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard"))
        {
            print("crash");

           var deathLocation = transform.position;
           Object.Instantiate(deathSpot, deathLocation, transform.rotation);

           Destroy(gameObject);
        }
    }

    private void OnDisable() {
        _controls.Flight.Disable();
    }

    public void OnBrake(InputAction.CallbackContext value)
    {
        brakeInput = value.ReadValue<float>();
    }

    public void OnMenuButton(InputAction.CallbackContext context)
    {
        print("Menu button pressed");
    }

    public void OnThrust(InputAction.CallbackContext value)
    {
        thrustInput = value.ReadValue<float>();
    }

    public void OnTorque(InputAction.CallbackContext value)
    {
        torqueInput = value.ReadValue<Vector2>();
    }

    public void OnYaw(InputAction.CallbackContext value)
    {
        yawInput = value.ReadValue<float>();
    }

    //Movement is made

    private void FixedUpdate() 
    {
        ThrustControl();
        TorqueControl();

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
}
