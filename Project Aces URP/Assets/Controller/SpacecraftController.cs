using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody), typeof(TargetableObject))]
public class SpacecraftController : MonoBehaviourPunCallbacks
{
    #region Serialized Fields

    [Header("Spacecraft Objects")]
    public WeaponsController weaponSystem;
    public CharacterHandler chosenCharacter;
    public ShipHandler chosenShip;
    [SerializeField]
    private GameObject explosionObject, gunAmmoObject, missileObject, hudCanvas;
    private Cinemachine.CinemachineVirtualCamera[] cameras;

    [HideInInspector]
    public float currentSpeed, currentHealth, brakeInput;
    #endregion

    #region Private Fields
    private PlayerHUDController HudController;
    private AbilityHandler passiveAbility, primaryAbility, secondaryAbility, aceAbility;
    private float respawnTime = 5;
    private bool 
        isAwaitingRespawn = false,
        canUsePrimary = true,
        canUseSecondary = true,
        canUseAce = false;
    private Rigidbody _rb;
    private ControlInputActions _controls;
    private GameObject ship;
    #endregion

    #region setup
    public override void OnEnable(){
        ship = Instantiate(chosenShip.shipPrefab, transform.position, transform.rotation);
        ship.transform.SetParent(this.gameObject.transform);
        currentHealth = chosenShip.maxHealth;
        if(photonView.IsMine){
            currentHealth = chosenShip.maxHealth;
            weaponSystem = ship.GetComponentInChildren<WeaponsController>();
            weaponSystem.EnableWeapons();
        
            var hud = Instantiate(hudCanvas, parent: gameObject.transform);
            HudController = hud.GetComponent<PlayerHUDController>();
            HudController.currentCraft = this;
            HudController.Activate();
            cameras = GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera>();
            _rb = GetComponent<Rigidbody>();
        }

        for (int i = 0; i < chosenCharacter.abilities.Count; i++){
            chosenCharacter.abilities[i].player = gameObject;
        }
        
        passiveAbility = chosenCharacter.abilities[0];
        primaryAbility = chosenCharacter.abilities[1];
        secondaryAbility = chosenCharacter.abilities[2];
        aceAbility = chosenCharacter.abilities[3];
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){
           currentHealth -= currentSpeed + 30;
        }
    }
    private void Eliminate(){
        isAwaitingRespawn = true;
        ship.SetActive(false);
        Instantiate(explosionObject, gameObject.transform);
        StartCoroutine(RespawnTimer());

    }
    #endregion
    
    #region targeting and camera
    public void CameraChange(){
        if(cameras[0].Priority > 1){
            cameras[0].MoveToTopOfPrioritySubqueue();
        }
        else{
            cameras[1].MoveToTopOfPrioritySubqueue();
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
        if(photonView.IsMine)
        if(isAwaitingRespawn){
            currentHealth = 0;
            return;}

        _rb.AddRelativeForce(0,0,currentSpeed, ForceMode.Acceleration);
        if (currentSpeed > chosenShip.cruiseSpeed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, chosenShip.cruiseSpeed, .001f);
        }
        currentSpeed = Mathf.Lerp(currentSpeed, chosenShip.minSpeed, brakeInput * .01f);

        if(currentHealth <= 0){
            Eliminate();
        }
    }

    public void ThrustControl(float thrustInput){
        if(!photonView.IsMine)return;
        var speed = currentSpeed + (thrustInput * chosenShip.acceleration);
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, chosenShip.minSpeed, chosenShip.maxSpeed);
    }

    public void TorqueControl(Vector2 torqueInput, float yawInput){
        if(!photonView.IsMine)return;
        var highspeedhandling = currentSpeed/chosenShip.maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * chosenShip.pitch) / highspeedhandling, yawInput * chosenShip.yaw, (torqueInput.x * chosenShip.roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce, ForceMode.Force);
    }

    public void MissileLaunch(bool missileInput){
        if(photonView.IsMine)
        weaponSystem.MissileControl(missileInput, currentSpeed);
    }
    public void GunControl(Vector2 cursorInputPosition, bool gunInput){
        if(photonView.IsMine)
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
        currentHealth = chosenShip.maxHealth;
        isAwaitingRespawn = false;
        ship.SetActive(true);

        //also teleport to spawn points using a spawn point system
    }
    #endregion

}
