using System.Collections;
using UnityEngine;
using Photon.Pun;


namespace Cox.PlayerControls{
/// <summary> Component required for inputs to cause an object to move as the spaceship. This component is also
/// required to use the WeaponsController, helps fill in the HudController, and supplies inputs to the CameraController.
/// These things all work together to create the complex player controller we have. 
/// Input Controller >> SpacraftController >>instantiates>> Ship Prefab with WeaponsController, HUDController, and CameraController </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpacecraftController : MonoBehaviourPunCallbacks
{

    [Header("Spacecraft Objects")]
    [SerializeField] PlayerObject playerObject; //used to set character and ship
    [SerializeField] GameObject menuPrefab; 
    [SerializeField] GameObject explosionObject, gunAmmoObject, missileObject;

    //Items obtained from character selection
    [HideInInspector]
    public CharacterHandler chosenCharacter;
    private AbilityHandler passiveAbility, primaryAbility, secondaryAbility, aceAbility;
    //Items obtained from Ship selection
    [HideInInspector]
    public ShipHandler chosenShip;
    private WeaponsController weaponSystem;
    private CameraController cameraController;
    private PlayerHUDController HudController;

   


    [HideInInspector]
    //Public so they can display on HUD using HUD Controller
    public float currentSpeed, currentShields, currentHealth, thrust = 0;
    private float respawnTime = 5;
    private bool 
        isAwaitingRespawn = false,
        isShieldRecharging = false,
        canUsePrimary = true,
        canUseSecondary = true,
        canUseAce = false;
    
    
    private Rigidbody _rb;
    private ControlInputActions _controls;
    private GameObject ship, menu;
    
    //Points are stored and retrieved from GameManager
    private Transform[] respawnPoints;

    #region setup
    public override void OnEnable(){
        if(playerObject == null){
            Debug.LogError("SpacecraftController: OnEnable(), critical playerObject not set in the inspector.");
            return;
        }
        

        chosenCharacter = playerObject.chosenCharacter;
        chosenShip = playerObject.chosenShip;

        //Find respawn points. Once teams are figured out, this needs to find specific team spawn points.
        respawnPoints = FindObjectOfType<GameManager>().teamASpawnpoints;
        //Instantiates the chosen ship and parents it under the controller. Then gets important info from the ship.
        ship = Instantiate(chosenShip.shipPrefab, transform.position, transform.rotation);
        ship.transform.SetParent(this.gameObject.transform);
        currentHealth = chosenShip.maxHealth;

        //If the photon view belongs to the local player...
        if(photonView.IsMine){
            //instantiate the menuPrefab, activate the weapons, hud, and camera controllers.
            menu = Instantiate(menuPrefab);
            currentHealth = chosenShip.maxHealth;
            currentShields = chosenShip.maxShield;
            weaponSystem = ship.GetComponentInChildren<WeaponsController>();
            weaponSystem.EnableWeapons();

            HudController = ship.GetComponentInChildren<PlayerHUDController>();
            HudController.currentCraft = this;
            HudController.Activate();

            cameraController = ship.GetComponentInChildren<CameraController>();
            cameraController.weaponsController = weaponSystem;
            cameraController.Activate();

            
            
            _rb = GetComponent<Rigidbody>();
        }

        //Find the character abilities and give them info about the local player. Them apply the abilities to the player.
        for (int i = 0; i < chosenCharacter.abilities.Count; i++){
            chosenCharacter.abilities[i].player = gameObject;
        }
        
        passiveAbility = chosenCharacter.abilities[0];
        primaryAbility = chosenCharacter.abilities[1];
        secondaryAbility = chosenCharacter.abilities[2];
        aceAbility = chosenCharacter.abilities[3];
    }

    public void MenuButton(){
        /*if(menu.activeSelf == false)
            menu.SetActive(true);
        else
            menu.SetActive(false);*/
    }
    #endregion
    
    #region targeting and camera
    public void CameraChange(){
        //Change which camera is being used and tell the hud controller which variant of the HUD to display.
        if(photonView.IsMine)
        cameraController.ChangeCamera();
        if(cameraController.currentCamera == 0){
            HudController.ThirdPersonHudSetInactive();
        }
        if(cameraController.currentCamera == 1){
            HudController.FirstPersonHudSetInactive();
        }
        if(cameraController.currentCamera > 1){
            HudController.HudSetInactive();
        }
    }

    public void RotateCamera(Vector2 cursorInputPosition){
        //Tells the camera controller to rotate the camera using player input
        if(!photonView.IsMine)return;
        cameraController.RotateCamera(cursorInputPosition);
    }

    public void CameraLockTarget(){
        //Tells the camera controller to look at the current target.
        if(photonView.IsMine)
        cameraController.CameraLockTarget();
    }
    public void ChangeTargetMode(int input){
        //Tells the WeaponsController which team to target.
        weaponSystem.ChangeTargetMode(input);
    }
    public void CycleTargets(){
        //Tells the WeaponsController to cycle the current missile target.
        weaponSystem.CycleMainTarget();
    }
    #endregion
    
    private void FixedUpdate(){
        //If player is not local, return.
        if(!photonView.IsMine)return;
        //If player is waiting to respawn, keep health at zero and return.
        if(isAwaitingRespawn){
            currentHealth = 0;
            return;
        }
        

        //Calculates speed based on current thrust and clamps speed.
        thrust = Mathf.Clamp01(thrust);
        var speed = thrust * chosenShip.maxSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, speed, (chosenShip.acceleration * Time.fixedDeltaTime)/45);
        currentSpeed = Mathf.Clamp(currentSpeed, chosenShip.minSpeed, chosenShip.maxSpeed);

        _rb.AddRelativeForce(0,0,currentSpeed, ForceMode.Acceleration);
       
        //activate health states
        if(currentShields <= 0){
            NoShield();
        }

        if(currentHealth <= chosenShip.maxHealth /*will be percentage*/){
            //LowHealth();
        }

        if(currentHealth <= 0){
            Eliminate();
        }

        //while shield is recharging, increment shield by recharge rate. If shields are maxed, stop recharging.
        while(isShieldRecharging){
            currentShields += chosenShip.shieldRechargeRate;
            if(currentShields >= chosenShip.maxShield){
                currentShields = chosenShip.maxShield;
                isShieldRecharging = false;
            }
        }
    }
    #region RigidBody Inputs
    //Take inputs and convert them to speed in FixedUpdate()
    public void ThrustControl(){
        if(!photonView.IsMine)return;
        thrust += .02f;
    }
    public void BrakeControl(){
        if(!photonView.IsMine)return;
        thrust -= .02f;
    }

    //Take vector2 and convert it to pitch, roll, and yaw. Then add that to the rigidbody as torque.
    public void TorqueControl(Vector2 torqueInput, float yawInput){
        if(!photonView.IsMine)return;
        var highspeedhandling = currentSpeed/chosenShip.maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * chosenShip.pitch) / highspeedhandling, yawInput * chosenShip.yaw, (torqueInput.x * chosenShip.roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce, ForceMode.Force);
    }
    #endregion

    #region Weapons Inputs
    //Send inputs from input handler to WeaponsController
    public void MissileLaunch(){
        if(!photonView.IsMine)return;
        if(!isAwaitingRespawn)
            weaponSystem.MissileControl(currentSpeed);
    }
    public void GunControl(bool gunInput){
        if(!photonView.IsMine)return;
        if(!isAwaitingRespawn)
            weaponSystem.GunControl(gunInput, currentSpeed);
    }

    #endregion

    #region Damage
    
    public void TakeDamage(float damage){
        isShieldRecharging = false;
        if(currentShields > 0){
            currentShields -= damage;
        }
        else{
            currentHealth -= damage;
        }
        //Stops the previous attempt to recharge shields and then retries;
        StopCoroutine(ShieldRechargeTimer());
        StartCoroutine(ShieldRechargeTimer());

    }

    private void OnCollisionEnter(Collision collision) {
        //On collision with hazards or other players, damage the player, based partially on speed.
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){
           TakeDamage(currentSpeed * 8);
        }
    }

    #endregion

    #region Player Health States

    public void NoShield(){
        Debug.Log("Spacecraft: NoShield() called");
        //Do something when shields are gone
    }

    public void LowHealth(){
        Debug.Log("Spacecraft: LowHealth() called");
        //Do something different related to low health
    }

    public void Eliminate(){
        //This happens when the players health reaches zero or they leave the arena.
        isAwaitingRespawn = true;
        currentHealth = 0;
        currentShields = 0;
        currentSpeed = 0;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        
        Instantiate(explosionObject, gameObject.transform);

        //Set controls inactive to avoid errors when inputs are made.
        HudController.gameObject.SetActive(false);
        cameraController.gameObject.SetActive(false);
        weaponSystem.gameObject.SetActive(false);
        ship.SetActive(false);
        StartCoroutine(RespawnTimer());

    }

    public void SpawnPlayer(){
        //Set health back to max and no longer awaiting respawn
        currentHealth = chosenShip.maxHealth;
        currentShields = chosenShip.maxShield;
        isAwaitingRespawn = false;

        //Find a random spawn point to respawn at
        int randInt = Random.Range(0, respawnPoints.Length - 1);
        gameObject.transform.position = respawnPoints[randInt].position;
        gameObject.transform.rotation = respawnPoints[randInt].rotation;

        //Set Ship Active. Ship is now completely respawned.
        ship.SetActive(true);
        weaponSystem.gameObject.SetActive(true);
        cameraController.gameObject.SetActive(true);
        HudController.gameObject.SetActive(true);
    }
    #endregion

    #region Character Abilities
    private void PassiveAbility(){
        Debug.Log("Spacecraft: PassiveAbility() called");
        //Seek and apply passives
    }
    public void PrimaryAbility(){
        if(canUsePrimary){
            canUsePrimary = false;
            primaryAbility.player = gameObject;
            StartCoroutine(primaryAbility.Activate());
            
            StartCoroutine(CooldownTimer(primaryAbility.cooldownTime, "Primary"));
        }
    }
    public void SecondaryAbility(){
        Debug.Log("Spacecraft: SecondaryAbility() called");
        //Seek and apply Secondary Ability
    }
    public void AceAbility(){
        Debug.Log("Spacecraft: AceAbility() called");
        //Ace Ability
    }
    #endregion

    #region IEnumerators
    //Delay used when abilities have startup time.
    
    //Delay used when abilities need to cool down.
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
    
    public IEnumerator ShieldRechargeTimer(){
        yield return new WaitForSecondsRealtime(8);
        isShieldRecharging = true;

    }

    public IEnumerator RespawnTimer(){
        yield return new WaitForSeconds(respawnTime);
       SpawnPlayer();

        //also teleport to spawn points using a spawn point system
    }
    #endregion

}
}
