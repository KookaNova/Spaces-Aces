using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class InputHandler : MonoBehaviourPunCallbacks, ControlInputActions.IFlightActions
{
    float thrustInput, yawInput, brakeInput;
    int targetMode = 0;
    bool gunInput = false, missileInput = false;
        
    SpacecraftController spacecraft;
    ControlInputActions _controls;
    Vector2 torqueInput, cameraInput, cursorInputPosition;

    public override void OnEnable() {
        cursorInputPosition = new Vector2 (Screen.width / 2, Screen.height / 2);
        spacecraft = GetComponentInChildren<SpacecraftController>();
        _controls = new ControlInputActions();
        _controls.Flight.SetCallbacks(this);
        _controls.Flight.Enable();
        Cursor.visible = false;
    }
    public override void OnDisable() {
        _controls.Flight.Disable();
    }
    //Update
    private void FixedUpdate() {
        if(photonView.IsMine)
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
        if(photonView.IsMine)
        spacecraft.CameraChange();
    }
    public void OnCycleTargets(InputAction.CallbackContext pressed){
        if(photonView.IsMine)
        spacecraft.CycleTargets();
    }
    public void OnMissileButton(InputAction.CallbackContext pressed){
        missileInput = pressed.ReadValueAsButton();
    }
    public void OnPrimaryAbility(InputAction.CallbackContext context){
        if(photonView.IsMine)
        spacecraft.PrimaryAbility();
    }
    public void OnSecondaryAbility(InputAction.CallbackContext context){
        if(photonView.IsMine)
        spacecraft.SecondaryAbility();
    }
    public void OnAceAbility(InputAction.CallbackContext context){
        if(photonView.IsMine)
        spacecraft.AceAbility();
    }
    public void OnStickMouseOverride(InputAction.CallbackContext stickInput){
        cursorInputPosition = cursorInputPosition + stickInput.ReadValue<Vector2>();
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
}
