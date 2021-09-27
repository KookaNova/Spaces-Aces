using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class SpacecraftController : MonoBehaviourPunCallbacks
{
    #region Serialized Fields
    [Header("Spacecraft Objects")]
    public WeaponsController weaponSystem;
    public CharacterHandler chosenCharacter;
    public ShipHandler chosenShip;
    private CameraController cameraController;
    [SerializeField]
    private GameObject menuPrefab, explosionObject, gunAmmoObject, missileObject;
    [HideInInspector]
    public float thrust = 0;

    [HideInInspector]
    public float currentSpeed, currentHealth;
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
    private GameObject ship, menu;
    private Transform[] respawnPoints;
    #endregion

    #region setup
    public override void OnEnable(){
        respawnPoints = FindObjectOfType<GameManager>().teamASpawnpoints;
        ship = Instantiate(chosenShip.shipPrefab, transform.position, transform.rotation);
        ship.transform.SetParent(this.gameObject.transform);
        currentHealth = chosenShip.maxHealth;
        if(photonView.IsMine){
            menu = Instantiate(menuPrefab);
            currentHealth = chosenShip.maxHealth;
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

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){
           currentHealth -= currentSpeed + 1700;
        }
    }
    private void Eliminate(){
        isAwaitingRespawn = true;
        currentHealth = 0;
        Instantiate(explosionObject, gameObject.transform);
        ship.SetActive(false);
        StartCoroutine(RespawnTimer());

    }
    #endregion
    
    #region targeting and camera
    public void CameraChange(){
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
    public void CameraLockTarget(){
        if(photonView.IsMine)
        cameraController.CameraLockTarget();
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
        if(!photonView.IsMine)return;
        if(isAwaitingRespawn){
            currentHealth = 0;
            return;
            }
        
        thrust = Mathf.Clamp(thrust, -1, 1);
        var speed = currentSpeed + (thrust * chosenShip.acceleration);
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, chosenShip.minSpeed, chosenShip.maxSpeed);

        _rb.AddRelativeForce(0,0,currentSpeed, ForceMode.Acceleration);
        if (currentSpeed > chosenShip.cruiseSpeed)
        {
            //currentSpeed = Mathf.Lerp(currentSpeed, chosenShip.cruiseSpeed, .001f);
        }

        if(currentHealth <= 0){
            Eliminate();
        }
    }

    public void ThrustControl(){
        if(!photonView.IsMine)return;
        thrust += .025f;
    }
    public void BrakeControl(){
        if(!photonView.IsMine)return;
        thrust -= .035f;
    }

    public void TorqueControl(Vector2 torqueInput, float yawInput){
        if(!photonView.IsMine)return;
        var highspeedhandling = currentSpeed/chosenShip.maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * chosenShip.pitch) / highspeedhandling, yawInput * chosenShip.yaw, (torqueInput.x * chosenShip.roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce, ForceMode.Force);
    }

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
    public void RotateCamera(Vector2 cursorInputPosition){
        if(!photonView.IsMine)return;
        cameraController.RotateCamera(cursorInputPosition);
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
        int randInt = Random.Range(0, respawnPoints.Length - 1);
        gameObject.transform.position = respawnPoints[randInt].position;
        gameObject.transform.rotation = respawnPoints[randInt].rotation;
        ship.SetActive(true);

        //also teleport to spawn points using a spawn point system
    }
    #endregion

}
