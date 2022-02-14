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
    public class PlayerController : SpacecraftController
    {
        [SerializeField] PlayerObject playerObject; //#CRITICAL: used to assign the character and ship on players.
        CameraController cameraController; //#CRITICAL: allows the player to control the camera.
        PlayerHUDController HudController; //#CRITICAL: creates and updates the hud
        bool gamepadFound = false; Gamepad _gp; //Used to check if we can set vibrations to the gamepad.
        
        #region Setup
        public override void Activate(){
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
                }
                else{
                    //load profile data from other players into the scoreboard
                    playerName = this.photonView.Owner.NickName;
                    playerAudio.outputAudioMixerGroup = externalVoice;
                }
            
            }
            else{
                //if there is no profile data, this player is Debug Man.
                playerName = "DebugMan";
            }

            if(playerObject == null){
                Debug.LogError("SpacecraftController: OnEnable(), critical playerObject not set in the inspector.");
                return;
            }
            isAwaitingRespawn = true;
            chosenCharacter = playerObject.chosenCharacter;
            chosenShip = playerObject.chosenShip;

            
            //Find respawn points. Once teams are figured out, this needs to find specific team spawn points.
            team = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            if(team == "A"){
                respawnPoints = FindObjectOfType<GameManager>().teamASpawnpoints;
            }
            else{
                respawnPoints = FindObjectOfType<GameManager>().teamBSpawnpoints;
            }
            //Instantiates the chosen ship and parents it under the controller. Then gets important info from the ship.
            if(photonView.IsMine){
                ship = PhotonNetwork.Instantiate(chosenShip.shipPrefab.name, transform.position, transform.rotation);
                ship.transform.SetParent(this.gameObject.transform);
                shipBehaviour = ship.GetComponent<ShipBehaviour>();
                shipBehaviour.SetController(this);
                explosionObject = chosenShip.explosion;
                _rb = ship.GetComponent<Rigidbody>();

                //instantiate the weapons, hud, and camera controllers.
                HudController = ship.GetComponentInChildren<PlayerHUDController>();
                HudController.currentCraft = this;
                cameraController = ship.GetComponentInChildren<CameraController>();
                cameraController.weaponsController = weaponSystem;
                weaponSystem = ship.GetComponentInChildren<WeaponsController>();
                weaponSystem.owner = this;

                //Activate systems after the passive modifiers are applied
                PassiveAbility();
                weaponSystem.EnableWeapons();
                HudController.Activate();
                cameraController.Activate();
                gameManager.AllPlayersFindTargets();
                currentHealth = maxHealth;
                currentShields = maxShield;

                //Find the character abilities and give them info about the local player. Them apply the abilities to the player.
                for (int i = 0; i < chosenCharacter.abilities.Count; i++){
                    if(chosenCharacter.abilities[i] != null){
                        chosenCharacter.abilities[i].playerInfo = this;
                    }
                    else{
                        Debug.LogWarningFormat("{0} is missing an ability in slot {1}. This may cause errors.", chosenCharacter.name, i);
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
            }
            
            ApplyCustomData();
            VoiceLine(0);
        }
        public override void SetRumble(float chaotic, float smooth, float time)
        {
            if(gamepadFound){
                _gp.SetMotorSpeeds(chaotic, smooth);
                StartCoroutine(ResetMotorSpeeds(time));
            }
        }
        public override void Deactivate(){
        isAwaitingRespawn = true;
        currentHealth = 0;
        currentShields = 0;
        currentSpeed = 0;
        primaryAbility.canUse = true;
        secondaryAbility.canUse = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        ship.SetActive(false);

        if(previousAttacker != null){
            cameraController.FollowTarget(true, previousAttacker.gameObject);
        }

        //turn off trailer renderer

        //Set controls inactive to avoid errors when inputs are made.
        HudController.gameObject.SetActive(false);
        cameraController.gameObject.SetActive(false);
        weaponSystem.gameObject.SetActive(false);
    }



        #endregion

        #region Camera
        public void CameraChange(){
        //Change which camera is being used and tell the hud controller which variant of the HUD to display.
            if(!photonView.IsMine)return;
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
        var highspeedhandling = currentSpeed/maxSpeed + 1;
        Vector3 torqueForce  = new Vector3((torqueInput.y * pitch) / highspeedhandling, yawInput * yaw, (torqueInput.x * roll) / highspeedhandling);
        _rb.AddRelativeTorque(torqueForce, ForceMode.Force);
        if(gamepadFound){
            _gp.SetMotorSpeeds(0, (currentSpeed/maxSpeed)*.5f);
            StartCoroutine(ResetMotorSpeeds(.1f));
        }
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

    public override void Reactivate(){
        cameraController.FollowTarget(false, null);
        cameraController.gameObject.SetActive(true);
        HudController.gameObject.SetActive(true);
    }

    #region IEnumerators
    
    public IEnumerator ResetMotorSpeeds(float time){
        yield return new WaitForSecondsRealtime(time);
        _gp.SetMotorSpeeds(0,0);
    }

    #endregion


    }
}
