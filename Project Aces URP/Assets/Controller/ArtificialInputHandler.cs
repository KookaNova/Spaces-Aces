using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
    /// <summary> 
    /// Creates input from context as if it were another player. This should act entirely an another player in the game, 
    /// just with inputs coming from this script instead of Input Handler.
    ///</summary>
    public class ArtificialInputHandler : SpacecraftController
    {
        public override void Activate(){
            isAwaitingRespawn = true;
            playerAudio.outputAudioMixerGroup = externalVoice;
            if(chosenCharacter == null){
                var characterOptions = Resources.FindObjectsOfTypeAll<CharacterHandler>();
                int index = Random.Range(0, characterOptions.Length);
                chosenCharacter = characterOptions[index];
                Debug.Log("Ai chose character " + chosenCharacter.name);
            }
            if(chosenShip == null){
                var shipOptions = Resources.FindObjectsOfTypeAll<ShipHandler>();
                int index = Random.Range(0, shipOptions.Length);
                chosenShip = shipOptions[index];
                Debug.Log("Ai chose ship " + chosenShip.name);
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
            if(PhotonNetwork.IsMasterClient){
                ship = PhotonNetwork.Instantiate(chosenShip.shipPrefab.name, transform.position, transform.rotation);
                ship.transform.SetParent(this.gameObject.transform);
                shipBehaviour = ship.GetComponent<ShipBehaviour>();
                shipBehaviour.SetController(this);
                explosionObject = chosenShip.explosion;
                //instantiate the weapons, hud, and camera controllers.
                weaponSystem = ship.GetComponentInChildren<WeaponsController>();
                weaponSystem.owner = this;
                _rb = ship.GetComponent<Rigidbody>();

                //Activate systems after the passive modifiers are applied
                PassiveAbility();
                weaponSystem.EnableWeapons();
                gameManager.AllPlayersFindTargets();
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
    }
}
