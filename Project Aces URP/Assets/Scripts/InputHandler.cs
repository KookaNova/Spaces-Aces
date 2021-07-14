using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, ControlInputActions.IFlightActions
{
    float thrustInput, yawInput, brakeInput;
    int targetMode = 0;
    bool gunInput = false, missileInput = false;
        
    SpacecraftController spacecraft;
    ControlInputActions _controls;
    Vector2 torqueInput, cameraInput, cursorInputPosition;

    private void OnEnable() {
        cursorInputPosition = new Vector2 (Screen.width / 2, Screen.height / 2);
        spacecraft = GetComponent<SpacecraftController>();
        _controls = new ControlInputActions();
        _controls.Flight.SetCallbacks(this);
        _controls.Flight.Enable();
        Cursor.visible = false;
    }
    private void OnDisable() {
        _controls.Flight.Disable();
    }
    //Update
    private void FixedUpdate() {
        spacecraft.brakeInput = brakeInput;
        spacecraft.ThrustControl(thrustInput);
        spacecraft.TorqueControl(torqueInput, yawInput);
        spacecraft.GunControl(cursorInputPosition, gunInput);
        spacecraft.MissileLaunch(missileInput);
    }

    //Inputs
    public void OnMenuButton(InputAction.CallbackContext context)
    {}
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
    public void OnAimGun(InputAction.CallbackContext position){
        cursorInputPosition = position.ReadValue<Vector2>();
    }
    public void OnCameraChange(InputAction.CallbackContext context){
        spacecraft.CameraChange();
    }
    public void OnChangeTargetMode(InputAction.CallbackContext pressed){
        if(pressed.ReadValueAsButton())targetMode += 1;
        if(targetMode > 2)targetMode = 0;
        spacecraft.ChangeTargetMode(targetMode);
    }
    public void OnCycleTargets(InputAction.CallbackContext context){
        spacecraft.CycleTargets();
    }
    public void OnMissileButton(InputAction.CallbackContext pressed){
        missileInput = pressed.ReadValueAsButton();
    }
    public void OnPrimaryAbility(InputAction.CallbackContext context){
        spacecraft.PrimaryAbility();
    }
    public void OnSecondaryAbility(InputAction.CallbackContext context){
        spacecraft.SecondaryAbility();
    }
    public void OnAceAbility(InputAction.CallbackContext context){
        spacecraft.AceAbility();
    }
    public void OnStickMouseOverride(InputAction.CallbackContext stickInput){
        cursorInputPosition = cursorInputPosition + stickInput.ReadValue<Vector2>();
    }
}
