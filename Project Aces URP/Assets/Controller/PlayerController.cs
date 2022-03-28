using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;


namespace Cox.PlayerControls{
    /// <summary> Component required for inputs to cause an object to move as the spaceship. This component is also
    /// required to use the WeaponsController, helps fill in the HudController, and supplies inputs to the CameraController.
    /// These things all work together to create the complex player controller we have. 
    /// Input Controller >> SpacraftController >>instantiates>> Ship Prefab with WeaponsController, HUDController, and CameraController </summary>
    public class PlayerController : SpacecraftController
    {
        [SerializeField] PlayerObject playerObject; //#CRITICAL: used to assign the character and ship on players.
        CameraController cameraController; //#CRITICAL: allows the player to control the camera.
        PlayerHUDController HudController; //#CRITICAL: creates and updates the hud
        bool gamepadFound = false; Gamepad _gp; //Used to check if we can set vibrations to the gamepad.
        
        #region Setup
        [PunRPC]
        public override void Activate(){
            //Find respawn points. Once teams are figured out, this needs to find specific team spawn points.
            teamInt = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            if(teamInt == 0){
                respawnPoints = FindObjectOfType<GameManager>().teamASpawnpoints;
            }
            else{
                respawnPoints = FindObjectOfType<GameManager>().teamBSpawnpoints;
            }

            if(this.photonView.Owner != null){
                if(photonView.IsMine){
                    //Check for gamepad
                    if(Gamepad.current != null){
                        _gp = Gamepad.current;
                        gamepadFound = true;
                    }
                    //Load profile data into the scoreboard
                    PlayerProfileData data = SaveData.LoadProfile();
                    playerName = data.profileName;
                    this.photonView.Owner.NickName = playerName;
                    playerAudio.outputAudioMixerGroup = localVoice;
                    StartCoroutine(PlayTimer());
                    if(playerObject == null){
                        Debug.LogError("SpacecraftController: OnEnable(), critical playerObject not set in the inspector.");
                        return;
                    }
                    chosenCharacter = playerObject.chosenCharacter;
                    chosenShip = playerObject.chosenShip;

                    //Instantiates the chosen ship and parents it under the controller. Then gets important info from the ship
                    ship = PhotonNetwork.Instantiate(chosenShip.shipPrefab.name, transform.position, transform.rotation);
                    PhotonNetwork.SendAllOutgoingCommands();
                    ship.GetPhotonView().RPC("SetController", RpcTarget.AllBuffered, photonView.ViewID);
                    explosionObject = chosenShip.explosion;
                    _rb = ship.GetComponent<Rigidbody>();
                    //instantiate the weapons, hud, and camera controllers.
                    weaponSystem = ship.GetComponentInChildren<WeaponsController>();
                    HudController = ship.GetComponentInChildren<PlayerHUDController>();
                    cameraController = ship.GetComponentInChildren<CameraController>();
                    weaponSystem.owner = this;
                    HudController.owner = this;
                    cameraController.weaponsController = weaponSystem;
                    //Activate systems after the passive modifiers are applied
                    PassiveAbility();
                    weaponSystem.EnableWeapons();
                    HudController.Activate();
                    cameraController.Activate();
                    currentHealth = maxHealth;
                    currentShields = maxShield;
                    //Find the character abilities and give them info about the local player. Them apply the abilities to the player.
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
                        CoolDownAbility(aceAbility.cooldownTime, aceAbility);
                    }
                    isAwaitingRespawn = false;
                    ApplyCustomData();
                    VoiceLine(0);
                }
                else{
                    //load profile data from other players into the scoreboard
                    playerName = this.photonView.Owner.NickName;
                    playerAudio.outputAudioMixerGroup = externalVoice;
                }
            }
        }
        protected override void SetRumble(float chaotic, float smooth, float time){
            if(gamepadFound){
                _gp.SetMotorSpeeds(chaotic, smooth);
                StartCoroutine(ResetMotorSpeeds(time));
            }
        }
        #endregion

        protected override void FixedUpdate(){
            //#Critical: If player is not local, return.
            if(photonView == null)return;
            if(!photonView.IsMine)return;
            shipPosition = _rb.transform.position;
            shipRotation = _rb.transform.rotation.eulerAngles;

            //keep abilities updated and active if needed, even if the player is eliminated.
            if(primaryAbility.isUpdating){
                primaryAbility.OnUpdate(this);
            }
            if(secondaryAbility.isUpdating){
                secondaryAbility.OnUpdate(this);
            }
            if(aceAbility.isUpdating){
                aceAbility.OnUpdate(this);
            }

            HudController.MissileAlertActive(missileChasing);
            HudController.WarningAlertActive(missileChasing);
            HudController.CautionAlertActive(missileTracking);

            if(missileChasing){
                if(missileClose){
                    shipBehaviour.missileWarning.Play();
                    shipBehaviour.missileWarning.SetScheduledEndTime(AudioSettings.dspTime + 0.2f);
                    //shipBehaviour.missileWarning.Stop();
                    /*if(!shipBehaviour.missileClose.isPlaying){
                        shipBehaviour.missileClose.Play();
                    }*/
                }
                else{
                    //shipBehaviour.missileClose.Stop();
                    if(!shipBehaviour.missileWarning.isPlaying){
                        shipBehaviour.missileWarning.Play();
                        shipBehaviour.missileWarning.SetScheduledEndTime(AudioSettings.dspTime + 1f);
                    }
                }
            }
            else{
                if(shipBehaviour.missileWarning.isPlaying){
                    shipBehaviour.missileWarning.Stop();
                }
                /*if(shipBehaviour.missileClose.isPlaying){
                    shipBehaviour.missileClose.Stop();
                }*/
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

        #region Camera
        public void CameraChange(){
        //Change which camera is being used and tell the hud controller which variant of the HUD to display.
            if(!photonView.IsMine)return;
            cameraController.ChangeCamera();
            if(cameraController.currentCamera == 0){
                HudController.ThirdPersonHudSetInactive();
                HudController.OverlaySetActive(true);
            }
            if(cameraController.currentCamera == 1){
                HudController.FirstPersonHudSetInactive();
                HudController.OverlaySetActive(true);
            }
            if(cameraController.currentCamera > 1){
                HudController.OverlaySetActive(false);
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
            if(photonView.IsMine)
            weaponSystem.ChangeTargetMode(input);
        }
        public void CycleTargets(){
            //Tells the WeaponsController to cycle the current missile target.
            if(photonView.IsMine)
            weaponSystem.CycleMainTarget();
        }
    
    #endregion

    #region RigidBody Inputs
    //Take inputs and convert them to speed in FixedUpdate()
    public void ThrustControl(){
        if(!photonView.IsMine)return;
        thrust += .01f;
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
        thrust -= .01f;
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
        
        if(torqueInput == Vector2.zero){
            shipBehaviour.ResolveGearsTurning();
        }
        else{
            shipBehaviour.isTurning = true;
        }
        var highspeedhandling = currentSpeed/maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * pitch) / highspeedhandling, yawInput * yaw, (torqueInput.x * roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce, ForceMode.Force);
    }
    #endregion
    #region Weapon Inputs
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
            if(gamepadFound)
            StartCoroutine(ResetMotorSpeeds(0.01f));
    }
    #endregion

    protected override void PlayerDamage(){
        HudController.DamageAlertActive(true);
    }

    public override void TargetHit(){
        Debug.Log("Target Hit!");
        HudController.HitAlertActive(true);

    }

    public override void TargetDestroyed(bool isKill){
        Debug.Log("Target Destroyed!");
        HudController.EliminatedAlertActive(true);
        if(isKill){
            kills++;
            ApplyCustomData();
            VoiceLine(8);
        }

    }

    public override void LowHealth(){
        Debug.Log("Spacecraft: LowHealth() called");
        HudController.HealthAlertActive(true);
        SetRumble(.5f,.3f,1);
        VoiceLine(10);
        //Do something related to low health
    }
    [PunRPC]
    protected override void Deactivate(int viewID){
        currentHealth = 0;
        currentShields = 0;
        currentSpeed = 0;
        isAwaitingRespawn = true;
        


        PhotonView.Find(viewID).gameObject.SetActive(false);

        if(photonView.IsMine){
            primaryAbility.canUse = true;
            secondaryAbility.canUse = true;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        if(previousAttacker != null){
            cameraController.FollowTarget(true, previousAttacker.gameObject);
        }

        //turn off trailer renderer

        //Set controls inactive to avoid errors when inputs are made.
        HudController.OverlaySetActive(false);
        cameraController.gameObject.SetActive(false);
        weaponSystem.gameObject.SetActive(false);
    }

    public override void Reactivate(){
        HudController.OverlaySetActive(true);
        cameraController.FollowTarget(false, null);
        cameraController.gameObject.SetActive(true);
        weaponSystem.Reset();

        HudController.HealthAlertActive(false);
        HudController.CautionAlertActive(false);
        HudController.WarningAlertActive(false);
        HudController.MissileAlertActive(false);
        HudController.DamageAlertActive(false);
        HudController.HitAlertActive(false);
        HudController.MissileAlertActive(false);
    }

    #region IEnumerators
    
    public IEnumerator ResetMotorSpeeds(float time){
        yield return new WaitForSecondsRealtime(time);
        _gp.SetMotorSpeeds(0,0);
    }

    private IEnumerator PlayTimer(){
        yield return new WaitForSecondsRealtime(1);
        elapsedTime++;
        
        yield return new WaitForEndOfFrame();
        ApplyCustomData();
        StartCoroutine(PlayTimer());
    }

    #endregion


    }
}
