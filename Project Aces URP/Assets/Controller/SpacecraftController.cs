using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(TargetableObject))]
public class SpacecraftController : MonoBehaviour, ControlInputActions.IFlightActions
{
    [Header("Spacecraft Objects")]
    public WeaponsController weaponSystem;
    public CharacterHandler chosenCharacter;
    public GameObject firstCam, shipObject, avatarUI, explosionObject, gunAmmoObject, missileObject, lockIndicator;
    public Canvas hudCanvas;
    

    [Header("Spacecraft Stats")]
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
    private Vector2 torqueInput, cameraInput, cursorInputPosition;
    private AbilityHandler passiveAbility, primaryAbility, secondaryAbility, aceAbility;
    //Utility Inputs
    private bool gunInput = false, 
        missileInput = false,
        canUsePrimary = true,
        canUseSecondary = true,
        canUseAce = false;
    private Rigidbody _rb;
    private ControlInputActions _controls;

    //Input System Setup-----------------------------------------------
    private void Awake(){
        _rb = GetComponent<Rigidbody>();
        weaponSystem = GetComponentInChildren<WeaponsController>();
        avatarUI.GetComponent<Image>().sprite = chosenCharacter.avatar;

        for (int i = 0; i < chosenCharacter.abilities.Count; i++){
            chosenCharacter.abilities[i].player = gameObject;
        }
        
        passiveAbility = chosenCharacter.abilities[0];
        primaryAbility = chosenCharacter.abilities[1];
        secondaryAbility = chosenCharacter.abilities[2];
        aceAbility = chosenCharacter.abilities[3];
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
        if(firstCam.activeSelf == true){
            firstCam.SetActive(false);
            return;
        }
        if(firstCam.activeSelf == false){
            firstCam.SetActive(true);
            return;
        }
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
    public void OnChangeTargetMode(InputAction.CallbackContext pressed){
        if(pressed.ReadValueAsButton() == true){
            weaponSystem.ChangeTargetMode();
        }
    }
    public void OnCycleTargets(InputAction.CallbackContext value)
    {
        var cycleValue = value.ReadValue<float>();
        if(cycleValue > 0.5){
            weaponSystem.CycleMainTarget();
        }
    }
    public void OnAimGun(InputAction.CallbackContext position){
        cursorInputPosition = position.ReadValue<Vector2>();
    }
    public void OnGunFire(InputAction.CallbackContext pressed){
        gunInput = pressed.ReadValueAsButton();
    }
    public void OnMissileButton(InputAction.CallbackContext pressed){
        missileInput = pressed.ReadValueAsButton();
    }
    //Player is controlled-----------------------------------------------
    private void FixedUpdate() {
        ThrustControl();
        TorqueControl();
        weaponSystem.GunControl(cursorInputPosition, gunInput, currentSpeed);
        weaponSystem.MissileControl(missileInput, currentSpeed);

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

    //Character Abilities
    public void OnPrimaryAbility(InputAction.CallbackContext context)
    {
        if(canUsePrimary){
            canUsePrimary = false;
            print("Ability Unavailable");
            primaryAbility.player = gameObject;
            StartCoroutine(DelayedAbility(primaryAbility, primaryAbility.startUpTime));
            StartCoroutine(CooldownTimer(primaryAbility.cooldownTime, "Primary"));
        }
        
        //use ability start up time to delay start


        
    }

    public void OnSecondaryAbility(InputAction.CallbackContext context)
    {

    }

    public void OnAceAbility(InputAction.CallbackContext context)
    {

    }

    public IEnumerator DelayedAbility(AbilityHandler ability, float startUpTime){
        Instantiate(ability.startUpParticle, gameObject.transform);
        yield return new WaitForSeconds(startUpTime);
        Instantiate(ability);

    }

    public IEnumerator CooldownTimer(float cooldown, string abilityType){
        yield return new WaitForSeconds(cooldown);

        if(abilityType == "Primary"){
            canUsePrimary = true;
        }
        if(abilityType == "Secondary"){
            canUseSecondary = true;
        }
        if(abilityType == "Ace"){
            canUseAce = true;
        }
        print("Ability Available");

    }
}
