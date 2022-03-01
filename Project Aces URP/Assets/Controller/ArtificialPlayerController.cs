using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Cox.PlayerControls{
    /// <summary> 
    /// Creates input from context as if it were another player. This should act entirely an another player in the game, 
    /// just with inputs coming from this script instead of Input Handler.
    ///</summary>
    public class ArtificialPlayerController : SpacecraftController
    {

        public enum AiStates{
            FreeFlight,
            Follow
        }

        AiStates aiStates = AiStates.FreeFlight;

        Vector3 toTarget = Vector3.zero;
        Vector3 rot = Vector3.zero;
        bool isMaster = false;
        bool isRotating = false;
        float rotationLimit;

        private TargetableObject closestTarget;

        public override void Activate(){
            isMaster = PhotonNetwork.IsMasterClient;
            playerAudio.outputAudioMixerGroup = externalVoice;
            if(chosenCharacter == null){
                var characterOptions = Resources.FindObjectsOfTypeAll<CharacterHandler>();
                int index = Random.Range(0, characterOptions.Length);
                chosenCharacter = characterOptions[index];
            }
            if(chosenShip == null){
                var shipOptions = Resources.FindObjectsOfTypeAll<ShipHandler>();
                int index = Random.Range(0, shipOptions.Length);
                chosenShip = shipOptions[index];
                Debug.LogFormat("Ai chose character {0} and ship {1}.", chosenCharacter, chosenShip);
            }
            playerName = chosenCharacter.name + "(AI)";

            //Find respawn points. Once teams are figured out, this needs to find specific team spawn points.
            if(teamName == "A"){
                respawnPoints = FindObjectOfType<GameManager>().teamASpawnpoints;
            }
            else{
                respawnPoints = FindObjectOfType<GameManager>().teamBSpawnpoints;
            }
        
            //Instantiates the chosen ship and parents it under the controller. Then gets important info from the ship.
            if(isMaster){
                ship = PhotonNetwork.Instantiate(chosenShip.shipPrefab.name, this.transform.position, this.transform.rotation);
                ship.transform.SetParent(this.transform);
                shipBehaviour = ship.GetComponent<ShipBehaviour>();
                shipBehaviour.SetController(this);
                explosionObject = chosenShip.explosion;
                //instantiate the weapons, hud, and camera controllers.
                targetableObject = ship.GetComponent<TargetableObject>();
                gameManager.allTargets.Add(targetableObject);
                weaponSystem = ship.GetComponentInChildren<WeaponsController>();
                weaponSystem.owner = this;
                _rb = ship.GetComponent<Rigidbody>();
                var targetable = ship.GetComponent<TargetableObject>();
                targetable.nameOfTarget = playerName;
                if(teamName == "A"){
                    targetable.targetTeam = TargetableObject.TargetType.TeamA;
                    weaponSystem.targMode = WeaponsController.TargetingMode.TeamB;
                }
                else{
                    targetable.targetTeam = TargetableObject.TargetType.TeamB;
                    weaponSystem.targMode = WeaponsController.TargetingMode.TeamA;
                }

                //Activate systems after the passive modifiers are applied
                PassiveAbility();
                weaponSystem.EnableWeapons();
                currentHealth = maxHealth;
                currentShields = maxShield;
                rotationLimit = (roll+pitch)/2;

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
                StartCoroutine(DecisionTime());
            }
        }
        public override void ApplyCustomData(){
        customProperties = new Hashtable(){
            {"Team", teamName},
            {"Name", playerName},
            {"Character", chosenCharacter.name},
            {"Ship", chosenShip.name},
            {"Score", score},
            {"Kills", kills},
            {"Deaths", deaths},
        };
        gameManager.UpdateScoreBoard(this);
    }
        protected override void FixedUpdate(){
        //#Critical: If player is not local, return.
        if(photonView == null)return;
        if(!isMaster)return;

        aiStates = AiStates.Follow;
        var enemies = new List<TargetableObject>();
        for(int i = 0; i < gameManager.allTargets.Count; i++){
            if(gameManager.allTargets[i].targetTeam != targetableObject.targetTeam){
                if(enemies.Count <= i){
                    enemies.Add(gameManager.allTargets[i]);
                }
                else{
                    enemies[i] = gameManager.allTargets[i];
                    
                }
            }
            
        }

        for(int i = 0; i < enemies.Count; i++){
            if(closestTarget == null){
                closestTarget = enemies[i];
                weaponSystem.currentTarget = i;
            }
            if(closestTarget != null){
                aiStates = AiStates.Follow;
                weaponSystem.currentTarget = i;
            }
            
        }
        

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
        //AI States have an effect
        switch (aiStates){
            case AiStates.FreeFlight:
                //Calculates speed based on current thrust and clamps speed.
                thrust = 1;
                rot = Vector3.Slerp(_rb.transform.forward, toTarget, rotationLimit/60 * Time.fixedDeltaTime);
                break;
            case AiStates.Follow:
                if(closestTarget == null){
                    aiStates = AiStates.FreeFlight;
                }
                else{
                    if(isRotating){
                        toTarget = closestTarget.gameObject.transform.position - _rb.position;
                        rot = Vector3.Slerp(_rb.transform.forward, toTarget, rotationLimit/40 * Time.fixedDeltaTime);
                        if(Vector3.Distance(_rb.transform.position, closestTarget.transform.position) < 500){
                            thrust = 0;
                            if(rot.x < 3 && rot.y < 3 && rot.x > -3 && rot.y > -3){
                                weaponSystem.GunControl(true, currentSpeed);
                            }
                            else{
                                weaponSystem.GunControl(false, currentSpeed);
                            }
                        }
                        else{
                            thrust = .85f;
                            weaponSystem.GunControl(false, currentSpeed);
                        }
                        
                        if(Vector3.Distance(_rb.transform.position, closestTarget.transform.position) < 3500){
                            if(rot.x < 25 && rot.y < 25 && rot.x > -25 && rot.y > -25){
                                weaponSystem.missileLocked = true;
                                weaponSystem.MissileControl(currentSpeed);
                            }
                            else{
                                weaponSystem.missileLocked = false;
                            }

                        }
                    }
                }
                break;
        }
        var speed = thrust * maxSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, speed, (acceleration * Time.fixedDeltaTime)/45);
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        _rb.AddRelativeForce(0,0,currentSpeed, ForceMode.Acceleration);

        if(rot != Vector3.zero){
            _rb.MoveRotation(Quaternion.LookRotation(rot));
        }
        
    }

    private IEnumerator DecisionTime(){
        float randTime = Random.Range(1f,5);
        yield return new WaitForSeconds(randTime);
        toTarget = new Vector3(Random.Range(-360f,360f), Random.Range(-360f,360f), Random.Range(-360f,360f));
        if(aiStates == AiStates.FreeFlight){
            StartCoroutine(DecisionTime());
        }
        if(aiStates == AiStates.Follow){
            StartCoroutine(BreakTime());
        }
        
        
    }

    private IEnumerator BreakTime(){
        StopCoroutine(DecisionTime());
        float randTime = Random.Range(2,7);
        yield return new WaitForSeconds(randTime);
        isRotating = false;
        float ra = Random.Range(2,9);
        yield return new WaitForSeconds(ra);
        isRotating = true;
        
        if(aiStates == AiStates.FreeFlight){
            StartCoroutine(DecisionTime());
        }
        if(aiStates == AiStates.Follow){
            StartCoroutine(BreakTime());
        }
    }
}
}
