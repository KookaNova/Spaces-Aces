using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;


namespace Cox.PlayerControls{
/// <summary> Receives inputs from either the player's controller and translates them into actions
/// in the Cox.PlayerControls.SpacecraftController.
///</summary>
public class InputHandler : MonoBehaviourPunCallbacks, ControlInputActions.IFlightActions
{
    float thrustInput, yawInput, brakeInput;
    int targetMode = 0;
    bool gunInput = false, missileInput = false;
        
    SpacecraftController spacecraft;
    ControlInputActions _controls;
    Vector2 torqueInput, cursorInput;

    public override void OnEnable() {
        cursorInput = new Vector2 (Screen.width / 2, Screen.height / 2);
        spacecraft = GetComponentInChildren<SpacecraftController>();
        _controls = new ControlInputActions();
        _controls.Flight.SetCallbacks(this);
        _controls.Flight.Enable();
        Cursor.visible = false;
    }
    public override void OnDisable() {
        _controls.Flight.Disable();
    }
    //Update, Sends input data for rigidbody, camera rotation, and gun use.
    private void FixedUpdate() {
        if(photonView.IsMine)
        if(brakeInput > 0){
            spacecraft.BrakeControl();
        }
        if(thrustInput > 0){
            spacecraft.ThrustControl();
        }
        spacecraft.TorqueControl(torqueInput, yawInput);
        spacecraft.GunControl(gunInput);
        spacecraft.RotateCamera(cursorInput);
    }

    //Inputs
    public void OnMenuButton(InputAction.CallbackContext context){
        spacecraft.MenuButton();
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
    public void OnGunFire(InputAction.CallbackContext pressed){
        gunInput = pressed.ReadValueAsButton();
    }
    public void OnCameraChange(InputAction.CallbackContext context){
        spacecraft.CameraChange();
    }
    public void OnCycleTargets(InputAction.CallbackContext pressed){
        if(pressed.ReadValueAsButton()){
            spacecraft.CycleTargets();
        }
        
    }
    public void OnMissileButton(InputAction.CallbackContext pressed){
        spacecraft.MissileLaunch();
    }
    public void OnPrimaryAbility(InputAction.CallbackContext pressed){
        if(pressed.ReadValueAsButton()){
        }
        spacecraft.PrimaryAbility();
    }
    public void OnSecondaryAbility(InputAction.CallbackContext pressed){
        if(pressed.ReadValueAsButton())
        spacecraft.SecondaryAbility();
    }
    public void OnAceAbility(InputAction.CallbackContext pressed){
        if(pressed.ReadValueAsButton())
        spacecraft.AceAbility();
    }
    public void OnCameraStick(InputAction.CallbackContext stickInput){
        cursorInput = stickInput.ReadValue<Vector2>();
    }
    public void OnCameraMouse(InputAction.CallbackContext deltaInput){
        cursorInput = deltaInput.ReadValue<Vector2>();
    }

    public void OnTargetModeAdd(InputAction.CallbackContext pressed)
    {
        if(photonView.IsMine)
        if(pressed.ReadValueAsButton())targetMode += 1;
        if(targetMode > 2)targetMode = 0;
        spacecraft.ChangeTargetMode(targetMode);
    }

    public void OnTargetModeSub(InputAction.CallbackContext pressed)
    {
        if(photonView.IsMine)
        if(pressed.ReadValueAsButton())targetMode -= 1;
        if(targetMode > 2)targetMode = 0;
        if(targetMode < 0)targetMode = 2;
        spacecraft.ChangeTargetMode(targetMode);
    }

    public void OnCameraTargetLock(InputAction.CallbackContext context)
    {
        if(photonView.IsMine)
        spacecraft.CameraLockTarget();
    }
}
}
