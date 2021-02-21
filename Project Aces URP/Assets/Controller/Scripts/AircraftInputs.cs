using UnityEngine;
using UnityEngine.InputSystem;


public class AircraftInputs : MonoBehaviour, ControlInputActions.IFlightActions
{
    public AircraftData aircraftData;
    
    private float thrustInput, yawInput, brakeInput;
    private Vector2 torqueInput, cameraInput;
    private Rigidbody _rb;
    private ControlInputActions controls;
    
    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new ControlInputActions();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.Flight.SetCallbacks(this);
        }
        controls.Flight.Enable();
    }

    private void Update()
    {
        SendInputData();
    }
    
    public void OnMenuButton(InputAction.CallbackContext pressed)
    {
        print("Menu button pressed");
    }

    public void OnThrust(InputAction.CallbackContext value)
    {
        thrustInput = value.ReadValue<float>();

    }

    public void OnBrake(InputAction.CallbackContext value)
    {
        brakeInput = value.ReadValue<float>();

    }

    public void OnYaw(InputAction.CallbackContext value)
    {
        yawInput = value.ReadValue<float>();
    }

    public void OnTorque(InputAction.CallbackContext value)
    {
        torqueInput = value.ReadValue<Vector2>();
    }
    
    private void SendInputData()
    {
        aircraftData.UpdateInputData(cameraInput, torqueInput, thrustInput, yawInput, brakeInput);
    }
}
