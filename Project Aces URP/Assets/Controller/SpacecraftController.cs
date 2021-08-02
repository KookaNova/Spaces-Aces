using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody), typeof(TargetableObject))]
public class SpacecraftController : MonoBehaviourPunCallbacks
{
    #region Public Fields

    [Header("Spacecraft Objects")]
    public WeaponsController weaponSystem;
    public CharacterHandler chosenCharacter;
    public ShipHandler chosenShip;
    public GameObject firstCam, avatarUI, explosionObject, gunAmmoObject, missileObject, lockIndicator;
    public Canvas hudCanvas;
    public FloatData currentSpeed, currentHealth;
    public float brakeInput;
    #endregion

    #region Private Serializable Fields
    [Header("Spacecraft Stats")]
    [SerializeField]
    private float acceleration = 10;
    [SerializeField]
    private float
        minSpeed = 10,
        cruiseSpeed = 150,
        maxSpeed = 200,
        roll = 5, 
        pitch = 7, 
        yaw = 3,
        maxHealth = 100;
        
    #endregion

    #region Private Fields
    private AbilityHandler passiveAbility, primaryAbility, secondaryAbility, aceAbility;
    private float respawnTime = 5;
    private bool isAwaitingRespawn = false,
        canUsePrimary = true,
        canUseSecondary = true,
        canUseAce = false;
    private Rigidbody _rb;
    private ControlInputActions _controls;
    #endregion

    #region setup
    private void Awake(){
        var ship = Instantiate(chosenShip.shipPrefab, transform.position, transform.rotation, gameObject.transform);
        currentHealth.value = maxHealth;
        weaponSystem = ship.GetComponentInChildren<WeaponsController>();
        weaponSystem.EnableWeapons();
        firstCam = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject;
        _rb = GetComponent<Rigidbody>();
        avatarUI.GetComponent<Image>().sprite = chosenCharacter.avatar;

        for (int i = 0; i < chosenCharacter.abilities.Count; i++){
            chosenCharacter.abilities[i].player = gameObject;
        }
        
        passiveAbility = chosenCharacter.abilities[0];
        primaryAbility = chosenCharacter.abilities[1];
        secondaryAbility = chosenCharacter.abilities[2];
        aceAbility = chosenCharacter.abilities[3];
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy Player")){
           currentHealth.value -= currentSpeed.value + 30;
        }
    }
    private void Eliminate(){
        isAwaitingRespawn = true;
        firstCam.SetActive(false);
        Instantiate(explosionObject, gameObject.transform);
        StartCoroutine(RespawnTimer());

    }
    #endregion
    
    #region targeting and camera
    public void CameraChange(){
        if(firstCam.activeSelf == true){
            firstCam.SetActive(false);
            return;
        }
        if(firstCam.activeSelf == false){
            firstCam.SetActive(true);
            return;
        }
    }
    public void ChangeTargetMode(int input){
        weaponSystem.ChangeTargetMode(input);
    }
    public void CycleTargets(){
        weaponSystem.CycleMainTarget();
    }
    #endregion
    
    #region Player Control
    private void FixedUpdate(){
        if(isAwaitingRespawn)return;

        _rb.AddRelativeForce(0,0,currentSpeed.value);
        if (currentSpeed.value > cruiseSpeed)
        {
            currentSpeed.value = Mathf.Lerp(currentSpeed.value, cruiseSpeed, .001f);
        }
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, minSpeed, brakeInput * .01f);

        if(currentHealth.value <= 0){
            Eliminate();
        }
    }

    public void ThrustControl(float thrustInput){
        var speed = currentSpeed.value + (thrustInput * acceleration);
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, speed, Time.deltaTime);
        currentSpeed.value = Mathf.Clamp(currentSpeed.value, minSpeed, maxSpeed);
    }

    public void TorqueControl(Vector2 torqueInput, float yawInput){
        var highspeedhandling = currentSpeed.value/maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * pitch) / highspeedhandling, yawInput * yaw, (torqueInput.x * roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce);
    }

    public void MissileLaunch(bool missileInput){
        weaponSystem.MissileControl(missileInput, currentSpeed);
    }
    public void GunControl(Vector2 cursorInputPosition, bool gunInput){
        weaponSystem.GunControl(cursorInputPosition, gunInput, currentSpeed);
    }
    #endregion

    #region Character Abilities
    private void PassiveAbility(){}
    public void PrimaryAbility(){
        if(canUsePrimary){
            canUsePrimary = false;
            primaryAbility.player = gameObject;
            StartCoroutine(DelayedAbility(primaryAbility, primaryAbility.startUpTime));
            StartCoroutine(CooldownTimer(primaryAbility.cooldownTime, "Primary"));
        }
        //use ability start up time to delay start
    }
    public void SecondaryAbility(){}
    public void AceAbility(){}
    #endregion

    #region IEnumerators
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
    }
    public IEnumerator RespawnTimer(){
        yield return new WaitForSeconds(respawnTime);
        currentHealth.value = maxHealth;
        isAwaitingRespawn = false;

        //also teleport to spawn points using a spawn point system
    }
    #endregion

}
