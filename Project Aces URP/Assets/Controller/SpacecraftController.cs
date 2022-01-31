using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using Hashtable = ExitGames.Client.Photon.Hashtable;


namespace Cox.PlayerControls{
/// <summary> Component required for inputs to cause an object to move as the spaceship. This component is also
/// required to use the WeaponsController, helps fill in the HudController, and supplies inputs to the CameraController.
/// These things all work together to create the complex player controller we have. 
/// Input Controller >> SpacraftController >>instantiates>> Ship Prefab with WeaponsController, HUDController, and CameraController </summary>
public class SpacecraftController : MonoBehaviourPunCallbacks
{
    bool gamepadFound = false;
    Gamepad _gp;

    [HideInInspector] public string playerName, teamName;
    
    private GameManager gameManager;

    #region  Spacecraft Variables
    [Header("Spacecraft Objects")]
    [SerializeField] PlayerObject playerObject; //used to set character and ship
    [SerializeField] GameObject menuPrefab; 
    [SerializeField] GameObject explosionObject, gunAmmoObject, missileObject;

    //Items obtained from character selection
    [HideInInspector]
    public CharacterHandler chosenCharacter;
    private AbilityHandler primaryAbility, secondaryAbility, aceAbility;
    //Items obtained from Ship selection
    [HideInInspector]
    public ShipHandler chosenShip;
    [HideInInspector]
    public WeaponsController weaponSystem;
    private CameraController cameraController;
    private PlayerHUDController HudController;

    //Floats edited by character passives
    [HideInInspector]
    public float maxHealth, 
        maxShield,
        shieldRechargeRate,
        acceleration,
        minSpeed,
        maxSpeed,
        roll, 
        pitch, 
        yaw,
        gunDamage,
        missileDamage,
        lockSpeed;

    #endregion

    #region HUD Variables
    [HideInInspector]
    //Public so they can display on HUD using HUD Controller
    public float currentSpeed, currentShields, currentHealth, thrust = 0;


    #endregion

    #region Multiplayer Variables
    Hashtable customProperties;
    int kills = 0;
    int deaths = 0;
    int score = 0;

    #endregion



    private float respawnTime = 5;
    private bool 
        isAwaitingRespawn = false,
        isShieldRecharging = false;
    private Rigidbody _rb;
    private ControlInputActions _controls;
    private GameObject ship, menu;
    
    //Points are stored and retrieved from GameManager
    private Transform[] respawnPoints;

    //Audio
    private AudioSource playerAudio;
    [SerializeField] private AudioMixerGroup localVoice, externalVoice;

    #region setup
    public override void OnEnable(){
        gameManager = FindObjectOfType<GameManager>();
        playerAudio = GetComponent<AudioSource>();
        

        if(this.photonView.Owner != null){
            if(photonView.IsMine){
                if(Gamepad.current != null){
                    _gp = Gamepad.current;
                    gamepadFound = true;
                    }

                PlayerProfileData data = SaveData.LoadProfile();
                playerName = data.profileName;
                this.photonView.Owner.NickName = playerName;
                playerAudio.outputAudioMixerGroup = localVoice;
            }
            else{
                playerName = this.photonView.Owner.NickName;
                playerAudio.outputAudioMixerGroup = externalVoice;
            }
            
        }
        else{
            playerName = "DebugMan";
        }
       
        isAwaitingRespawn = true;
        menuPrefab.SetActive(false);
        if(playerObject == null){
            Debug.LogError("SpacecraftController: OnEnable(), critical playerObject not set in the inspector.");
            return;
        }
        

        chosenCharacter = playerObject.chosenCharacter;
        chosenShip = playerObject.chosenShip;

        //Find respawn points. Once teams are figured out, this needs to find specific team spawn points.
        if((string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == "A"){
            respawnPoints = FindObjectOfType<GameManager>().teamASpawnpoints;
        }
        else{
            respawnPoints = FindObjectOfType<GameManager>().teamBSpawnpoints;
        }
        
        //Instantiates the chosen ship and parents it under the controller. Then gets important info from the ship.
        ship = Instantiate(chosenShip.shipPrefab, transform.position, transform.rotation);
        ship.transform.SetParent(this.gameObject.transform);
        

        if(photonView == null)return;
        //If the photon view belongs to the local player...
        if(photonView.IsMine){
            //instantiate the menuPrefab, weapons, hud, and camera controllers.
            menu = Instantiate(menuPrefab);
            weaponSystem = ship.GetComponentInChildren<WeaponsController>();
            weaponSystem.owner = this;
            
            HudController = ship.GetComponentInChildren<PlayerHUDController>();
            HudController.currentCraft = this;
            
            cameraController = ship.GetComponentInChildren<CameraController>();
            cameraController.weaponsController = weaponSystem;

            _rb = ship.GetComponent<Rigidbody>();
            //_rb = GetComponent<Rigidbody>();



            //Activate systems after the passive modifiers are applied
            PassiveAbility();
            weaponSystem.EnableWeapons();
            HudController.Activate();
            cameraController.Activate();
            currentHealth = maxHealth;
            currentShields = maxShield;

        }

        //Find the character abilities and give them info about the local player. Them apply the abilities to the player.
        for (int i = 0; i < chosenCharacter.abilities.Count; i++){
            if(chosenCharacter.abilities[i] == null){
                Debug.LogWarningFormat("{0} is missing an ability in slot {1}. This may cause errors.", chosenCharacter.name, i);
            }
            else{
                chosenCharacter.abilities[i].player = this;
            }
            
        }
        if(chosenCharacter.abilities[0] != null){
            primaryAbility = chosenCharacter.abilities[0];
            primaryAbility.canUse = true;
            primaryAbility.isActive = false;
            primaryAbility.isUpdating = false;
        }
        if(chosenCharacter.abilities[1] != null){
            secondaryAbility = chosenCharacter.abilities[1];
            secondaryAbility.canUse = true;
            secondaryAbility.isActive = false;
            secondaryAbility.isUpdating = false;
        }
        if(chosenCharacter.abilities[2] != null){
            aceAbility = chosenCharacter.abilities[2];
            aceAbility.canUse = false;
            aceAbility.isActive = false;
            aceAbility.isUpdating = false;
        }

        isAwaitingRespawn = false;
        HudController.IsLowHealth(false);

        ApplyCustomData();

        VoiceLine(0);

    }
    /// <summary>
    ///0=Start Match | 1=Primary | 2=Secondary | 3=Ace | 4=EnemyBeingTargeted,
    ///5=MissileTypeFired | 6=ThisBeingTargeted | 7=MissileIncoming | 8=EnemyEliminated, 
    ///9=ShieldsDown | 10=LowHealth | 11=SelfEliminated | 12=Respawn
    /// </summary>
    public void VoiceLine(int index){
        playerAudio.clip = chosenCharacter.voiceLines[index].audio;
        playerAudio.Play();
        gameManager.Subtitle(chosenCharacter.voiceLines[index]);
    }

    public void AddScore(int addedScore){
        score += addedScore;
        ApplyCustomData();
    }

    private void ApplyCustomData(){
        customProperties = new Hashtable(){
            {"Name", playerName},
            {"Character", chosenCharacter.name},
            {"Ship", chosenShip.name},
            {"Score", score},
            {"Kills", kills},
            {"Deaths", deaths},
        };
        PhotonNetwork.SetPlayerCustomProperties(customProperties);
        
    }

    public void MenuButton(){
        menu.SetActive(!menu.activeSelf);
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

    public void RotateCamera(Vector2 cursorInputPosition, bool isMouse){
        //Tells the camera controller to rotate the camera using player input
        if(!photonView.IsMine)return;
        cameraController.RotateCamera(cursorInputPosition, isMouse);
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
        //#Critical: If player is not local, return.
        if(photonView == null)return;
        if(!photonView.IsMine)return;

        //keep abilities updated and active if needed, even if the player is eliminated.
        if(primaryAbility.isUpdating){
            primaryAbility.OnUpdate();
        }
        if(secondaryAbility.isUpdating){
            secondaryAbility.OnUpdate();
        }
        /*if(aceAbility.isUpdating){
            aceAbility.OnUpdate(); ///not implemented
        }*/

        //If player is waiting to respawn, return.
        if(isAwaitingRespawn){
            return;
        }
        

        //Calculates speed based on current thrust and clamps speed.
        thrust = Mathf.Clamp01(thrust);
        var speed = thrust * maxSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, speed, (acceleration * Time.fixedDeltaTime)/45);
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        _rb.AddRelativeForce(0,0,currentSpeed, ForceMode.Acceleration);

        //if shield is recharging, increment shield by recharge rate. If shields are maxed, stop recharging.
        if(isShieldRecharging){
            currentShields = Mathf.MoveTowards(currentShields, maxShield, shieldRechargeRate * Time.deltaTime);
            if(currentShields >= maxShield){
                currentShields = maxShield;
                isShieldRecharging = false;
            }
        }

    }
    #region RigidBody Inputs
    //Take inputs and convert them to speed in FixedUpdate()
    public void ThrustControl(){
        if(!photonView.IsMine)return;
        thrust += .02f;
        if(gamepadFound){
            _gp.SetMotorSpeeds(thrust/3, thrust);
            StartCoroutine(ResetMotorSpeeds(.1f));
        }

        if(currentSpeed >= maxSpeed/2){
            //highSpeed(true);
        }
    }
    public void BrakeControl(){
        if(!photonView.IsMine)return;
        thrust -= .02f;
        if(gamepadFound && thrust <.65f){
            _gp.SetMotorSpeeds(0, thrust);
            StartCoroutine(ResetMotorSpeeds(.1f));
        }
        if(gamepadFound && thrust >.65f){
            _gp.SetMotorSpeeds(thrust/2, thrust);
            StartCoroutine(ResetMotorSpeeds(.1f));
        }

        if(currentSpeed <= maxSpeed/2){
            //HighSpeed(false);
        }
    }

    //Take vector2 and convert it to pitch, roll, and yaw. Then add that to the rigidbody as torque.
    public void TorqueControl(Vector2 torqueInput, float yawInput){
        if(!photonView.IsMine)return;
        var highspeedhandling = currentSpeed/maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * pitch) / highspeedhandling, yawInput * yaw, (torqueInput.x * roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce, ForceMode.Force);
        if(gamepadFound){
            _gp.SetMotorSpeeds(0, (currentSpeed/maxSpeed)*.5f);
            StartCoroutine(ResetMotorSpeeds(.1f));
        }
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
            StartCoroutine(ResetMotorSpeeds(0.01f));
    }

    #endregion

    #region Damage
    
    public void TakeDamage(float damage, SpacecraftController attacker, string cause){
        isShieldRecharging = false;
        if(gamepadFound){
            _gp.SetMotorSpeeds(.75f, 1);
            StartCoroutine(ResetMotorSpeeds(.25f));
        }
        if(currentShields > 0){
            currentShields -= damage;
        }
        else{
            NoShield();
            currentHealth -= damage;
        }
        if(currentHealth < 0){
            Eliminate(attacker, cause);
        }
        var healthPercentage = (maxHealth - currentHealth)/maxHealth;
        if(currentHealth <= .25f){
            LowHealth();
        }
        //Stops the previous attempt to recharge shields and then retries;
        StopCoroutine(ShieldRechargeTimer());
        StartCoroutine(ShieldRechargeTimer());

    }

    private void OnCollisionEnter(Collision collision) {
        //On collision with hazards or other players, damage the player, based partially on speed.
        print("Collision");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){
           TakeDamage(currentSpeed * 8, null, "accident");
        }
    }

    #endregion

    #region Player Health States

    public void NoShield(){
        Debug.Log("Spacecraft: NoShield() called");
        if(gamepadFound){
            _gp.SetMotorSpeeds(0, .2f);
            StartCoroutine(ResetMotorSpeeds(1f));
        }
        VoiceLine(9);
        //Do something when shields are gone
    }

    public void LowHealth(){
        Debug.Log("Spacecraft: LowHealth() called");
        HudController.IsLowHealth(true);
        if(gamepadFound){
            _gp.SetMotorSpeeds(.5f, .3f);
            StartCoroutine(ResetMotorSpeeds(1f));
        }
        VoiceLine(10);

        //Do something different related to low health
    }

    public async void Eliminate(SpacecraftController attacker, string cause){
        deaths++;
        VoiceLine(11);
        if(gamepadFound){
            _gp.SetMotorSpeeds(1, 1);
            StartCoroutine(ResetMotorSpeeds(.25f));
        }

        if(attacker == null){
            gameManager.FeedEvent(this, this, cause, true);
        }
        else{
            gameManager.FeedEvent(attacker, this, cause, true);
            attacker.TargetDestroyed(true);
            cameraController.FollowTarget(true, attacker.gameObject);
        }
        
        //This happens when the players health reaches zero or they leave the arena.
        isAwaitingRespawn = true;
        currentHealth = 0;
        currentShields = 0;
        currentSpeed = 0;
        primaryAbility.canUse = true;
        secondaryAbility.canUse = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        
        Instantiate(explosionObject, gameObject.transform);

        //Set controls inactive to avoid errors when inputs are made.
        HudController.gameObject.SetActive(false);
        cameraController.gameObject.SetActive(false);
        weaponSystem.gameObject.SetActive(false);
        ship.SetActive(false);
        StartCoroutine(RespawnTimer());

        ApplyCustomData();

    }

    public  void TargetDestroyed(bool isKill){
        Debug.Log("Target Destroyed!");
        if(isKill){
            kills++;
            ApplyCustomData();
        }
        VoiceLine(8);

    }

    public void SpawnPlayer(){
        //Find a random spawn point to respawn at
        cameraController.FollowTarget(false, null);
        int randInt = Random.Range(0, respawnPoints.Length - 1);
        this.gameObject.transform.position = respawnPoints[randInt].position;
        this.gameObject.transform.rotation = respawnPoints[randInt].rotation;
        //Set health back to max and no longer awaiting respawn
        currentHealth = maxHealth;
        currentShields = maxShield;
        isAwaitingRespawn = false;
        HudController.IsLowHealth(false);

        //Set Ship Active. Ship is now completely respawned.
        ship.SetActive(true);
        weaponSystem.gameObject.SetActive(true);
        cameraController.gameObject.SetActive(true);
        HudController.gameObject.SetActive(true);
        VoiceLine(12);
    }
    #endregion

    #region Character Abilities
    private void PassiveAbility(){
        //Adds passive modifiers from the chosen character
        maxHealth = chosenShip.maxHealth + chosenCharacter.health;
        maxShield = chosenShip.maxShield + chosenCharacter.shield;
        shieldRechargeRate = chosenShip.shieldRechargeRate + chosenCharacter.shieldRechargeRate;
        acceleration = chosenShip.acceleration + chosenCharacter.acceleration;
        minSpeed = chosenShip.minSpeed + chosenCharacter.minSpeed;
        maxSpeed = chosenShip.maxSpeed + chosenCharacter.maxSpeed;
        roll = chosenShip.roll + chosenCharacter.roll;
        pitch = chosenShip.pitch + chosenCharacter.pitch;
        yaw = chosenShip.yaw + chosenCharacter.yaw;
        //weapons based modifiers
        weaponSystem.gunModifier += chosenCharacter.gunDamage;
        weaponSystem.missileModifier += chosenCharacter.missileDamage;
        weaponSystem.lockOnEfficiency += chosenCharacter.lockOnEfficiency;
        weaponSystem.missileReload += chosenCharacter.missileReload;
        
        
    }
    public void PrimaryAbility(){
        if(primaryAbility.canUse){
            StartCoroutine(primaryAbility.Activate());
        }
    }
    public void SecondaryAbility(){
        if(secondaryAbility.canUse){
            StartCoroutine(secondaryAbility.Activate());
            
        }
    }
    public void AceAbility(){
        Debug.Log("Spacecraft: AceAbility() called");
        //Ace Ability
    }
    //Handles ability cooldown
    public void CoolDownAbility(float coolDown, AbilityHandler ability){
        StartCoroutine(CooldownTimer(coolDown, ability));
    }

    #endregion

    #region IEnumerators
    //Delay used when abilities have startup time.
    
    //Delay used when abilities need to cool down.
    public IEnumerator CooldownTimer(float cooldown, AbilityHandler ability){
        yield return new WaitForSecondsRealtime(cooldown);
        ability.canUse = true;
        ability.isActive = false;
    }
    
    public IEnumerator ShieldRechargeTimer(){
        yield return new WaitForSecondsRealtime(8);
        isShieldRecharging = true;

    }

    public IEnumerator RespawnTimer(){
        yield return new WaitForSecondsRealtime(respawnTime);
       SpawnPlayer();
    }

    public IEnumerator ResetMotorSpeeds(float time){
        yield return new WaitForSecondsRealtime(time);
        if(gamepadFound)
        _gp.SetMotorSpeeds(0,0);
    }
    #endregion

}
}
