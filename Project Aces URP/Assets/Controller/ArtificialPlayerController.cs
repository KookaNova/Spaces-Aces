using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
    /// <summary> 
    /// Creates input from context as if it were another player. This should act entirely an another player in the game, 
    /// just with inputs coming from this script instead of Input Handler.
    ///</summary>
    public class ArtificialPlayerController : SpacecraftController
    {
        bool isMaster = false;

        protected override void Activate(){
            isAwaitingRespawn = true;
            isMaster = true;
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
            playerName = chosenCharacter.name;

            //Find respawn points. Once teams are figured out, this needs to find specific team spawn points.
            if(team == "A"){
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
                if(team == "A"){
                    targetable.targetTeam = TargetableObject.TargetType.TeamA;
                }
                else{
                    targetable.targetTeam = TargetableObject.TargetType.TeamB;
                }

                //Activate systems after the passive modifiers are applied
                PassiveAbility();
                weaponSystem.EnableWeapons();
                currentHealth = maxHealth;
                currentShields = maxShield;

                //Find the character abilities and give them info about the local player. Them apply the abilities to the player.
                for (int i = 0; i < chosenCharacter.abilities.Count; i++){
                    if(chosenCharacter.abilities[i] == null){
                        Debug.LogWarningFormat("{0} is missing an ability in slot {1}. This may cause errors.", chosenCharacter.name, i);
                    }
                    else{
                        chosenCharacter.abilities[i].playerInfo = this;
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
                ApplyCustomData();
                VoiceLine(0);
            }
        }
        protected override void FixedUpdate(){
        //#Critical: If player is not local, return.
        if(photonView == null)return;
        if(!isMaster)return;

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
    }
}
