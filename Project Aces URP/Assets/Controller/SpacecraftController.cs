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
public abstract class SpacecraftController : MonoBehaviourPunCallbacks
{
    #region Fields
    #region Critical Fields
    [HideInInspector] protected GameManager gameManager; //#CRITICAL: handles gamemodes, kill feed, score, and tracking all players in the scene.
    [HideInInspector] public CharacterHandler chosenCharacter = null; //#CRITICAL: fills ability data and adds values to ship data.
    [HideInInspector] public ShipHandler chosenShip = null; //#CRITICAL: fills ship data and is required for the the ship to spawn in.
    [HideInInspector] public string playerName, teamName; //#CRITICAL: required for choosing spawn points and filling name data.
    protected Transform[] respawnPoints; //#CRITICAL: required for spawning the player into the game.
    #endregion
    
    #region Fields from character and ship choice
    protected AbilityHandler primaryAbility, secondaryAbility, aceAbility; //Obtained from character selection
    protected GameObject explosionObject; 
    protected Rigidbody _rb;
    protected ShipBehaviour shipBehaviour;
    public TargetableObject targetableObject;
    protected AudioSource playerAudio;
    protected AudioMixerGroup localVoice, externalVoice;
    [HideInInspector] public GameObject ship;
    [HideInInspector] public WeaponsController weaponSystem;
    [HideInInspector] public float maxHealth, 
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
        lockSpeed
    ;
    #endregion
    
    #region Fields updated during play
    //Public so they can display on HUD using HUD Controller
    [HideInInspector] public float currentSpeed, currentShields, currentHealth, thrust = 0;
    protected float respawnTime = 5;
    protected bool isAwaitingRespawn = false, isShieldRecharging = false;
    
    //For Multiplayer.
    protected SpacecraftController previousAttacker;
    protected Hashtable customProperties;
    protected string team = "A";
    private int kills = 0;
    private int deaths = 0;
    private int score = 0;

    #endregion
    #endregion

    #region Functionality

    #region setup    
    public override void OnEnable(){
        gameManager = FindObjectOfType<GameManager>();
        playerAudio = GetComponent<AudioSource>();
        Activate();
    }
    protected virtual void Activate(){}
    /// <summary>
    ///0=Start Match | 1=Primary | 2=Secondary | 3=Ace | 4=EnemyBeingTargeted
    ///5=MissileTypeFired | 6=ThisBeingTargeted | 7=MissileIncoming | 8=EnemyEliminated
    ///9=ShieldsDown | 10=LowHealth | 11=SelfEliminated | 12=Respawn
    /// </summary>
    public void VoiceLine(int index){
        if(chosenCharacter == null)return;
        playerAudio.clip = chosenCharacter.voiceLines[index].audio;
        playerAudio.Play();
        gameManager.Subtitle(chosenCharacter.voiceLines[index]);
    }
    protected virtual void Deactivate(){
        //player controller overrides this
        isAwaitingRespawn = true;
        currentHealth = 0;
        currentShields = 0;
        currentSpeed = 0;
        primaryAbility.canUse = true;
        secondaryAbility.canUse = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        ship.SetActive(false);

        //turn off trailer renderer
    }
    public virtual void SetRumble(float chaotic, float smooth, float time){}

    public void AddScore(int addedScore){
        score += addedScore;
        ApplyCustomData();
    }

    public void ApplyCustomData(){
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
    }
    #endregion
    
    
    protected virtual void FixedUpdate(){
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
        if(aceAbility.isUpdating){
            aceAbility.OnUpdate();
        }

        //#Critical: if player is waiting to respawn, return.
        if(isAwaitingRespawn){
            return;
        }

        //if shield is recharging, increment shield by recharge rate. If shields are maxed, stop recharging.
        if(isShieldRecharging){
            currentShields = Mathf.MoveTowards(currentShields, maxShield, shieldRechargeRate * Time.deltaTime);
            if(currentShields >= maxShield){
                currentShields = maxShield;
                isShieldRecharging = false;
            }
        }
        
        //Calculates speed based on current thrust and clamps speed.
        thrust = Mathf.Clamp01(thrust);
        var speed = thrust * maxSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, speed, (acceleration * Time.fixedDeltaTime)/45);
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        _rb.AddRelativeForce(0,0,currentSpeed, ForceMode.Acceleration);
    }

    #region Damage
    public void TakeDamage(float damage, SpacecraftController attacker, string cause){
        isShieldRecharging = false;
        
        SetRumble(.75f, 1, .25f);

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
        if(healthPercentage <= .25f){
            LowHealth();
        }
        //Stops the previous attempt to recharge shields and then retries;
        StopCoroutine(ShieldRechargeTimer());
        StartCoroutine(ShieldRechargeTimer());

    }

    #endregion

    #region Player Health States

    public void NoShield(){
        Debug.Log("Spacecraft: NoShield() called");
        SetRumble(0, .2f, 1f);
        VoiceLine(9);
        //Do something when shields are gone
    }

    public void LowHealth(){
        Debug.Log("Spacecraft: LowHealth() called");
        SetRumble(.5f,.3f,1);
        VoiceLine(10);
        //Do something related to low health
    }

    /// <summary>
    /// When a player's health reaches zero or they exit the arena boundaries, this function is called. The variable "attacker" can be null.
    /// </summary>
    public void Eliminate(SpacecraftController attacker, string cause){
        deaths++;
        VoiceLine(11);
        SetRumble(1,1,.25f);
        Instantiate(explosionObject, gameObject.transform);
        previousAttacker = attacker;

        if(attacker != null){
            gameManager.FeedEvent(attacker, this, cause, true);
            attacker.TargetDestroyed(true);
        }
        else{
            gameManager.FeedEvent(this, this, cause, true);
        }
        
        
        StartCoroutine(RespawnTimer());
        ApplyCustomData();
        Deactivate();
    }

    

    public void TargetDestroyed(bool isKill){
        Debug.Log("Target Destroyed!");
        if(isKill){
            kills++;
            ApplyCustomData();
            VoiceLine(8);
        }
        

    }

    public void SpawnPlayer(){
        //Find a random spawn point to respawn at
        int randInt = Random.Range(0, respawnPoints.Length - 1);
        ship.transform.position = respawnPoints[randInt].position;
        ship.transform.rotation = respawnPoints[randInt].rotation;
        //Set health back to max and no longer awaiting respawn
        currentHealth = maxHealth;
        currentShields = maxShield;
        isAwaitingRespawn = false;
        

        //Set Ship Active. Ship is now completely respawned.
        ship.SetActive(true);
        weaponSystem.gameObject.SetActive(true);
        Reactivate();
        VoiceLine(12);
    }
    public virtual void Reactivate(){}
    #endregion

    #region Character Abilities
    public void PassiveAbility(){
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
        if(photonView.IsMine)
        if(primaryAbility.canUse){
            StartCoroutine(primaryAbility.Activate());
        }
    }
    public void SecondaryAbility(){
        if(photonView.IsMine)
        if(secondaryAbility.canUse){
            StartCoroutine(secondaryAbility.Activate());
            
        }
    }
    public void AceAbility(){
        if(photonView.IsMine)
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
    #endregion
    #endregion

}
}
