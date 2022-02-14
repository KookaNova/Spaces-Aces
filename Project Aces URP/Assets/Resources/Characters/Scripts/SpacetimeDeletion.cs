using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cox.PlayerControls;
[CreateAssetMenu]
public class SpacetimeDeletion : AbilityHandler
{
    [SerializeField] private SpacetimeDeletionBehaviour deletionOrb;
    private MissileBehaviour missile;
    public override IEnumerator Activate()
    {
        if(playerInfo == null) {
            //Checks if player info was received.
            Debug.LogError("Teleport: Activate(), player is null. Something went wrong in either SpacecraftController or AbilityHandler");
            yield break;
        }
        if(isActive){
            ActiveAction();
            yield break;
        }
        if(!canUse)yield break;
        

        var weapons = playerInfo.weaponSystem;
        GameObject m =  PhotonNetwork.Instantiate(weapons.missileType.gameObject.name, weapons.missilePosition[weapons.currentMis].position, weapons.missilePosition[weapons.currentMis].rotation);
        missile = m.GetComponent<MissileBehaviour>();
        missile.owner = playerInfo;
        missile.currentSpeed = playerInfo.GetComponent<SpacecraftController>().currentSpeed + 400;
        missile.explosion = deletionOrb.gameObject;
        if(weapons.missileLocked){
            missile.target = weapons.currentTargetSelection[weapons.currentTarget].gameObject;
        }
        isActive = true;
        isUpdating = true;

        /*
        var obj = PhotonNetwork.Instantiate(deletionOrb.name, playerInfo.ship.transform.position, playerInfo.ship.transform.rotation);
        Debug.Log(deletionOrb.name);
        deletionOrb.owner = playerInfo;
        deletionOrb.startUp = startUpTime;
        obj.transform.parent = playerInfo.ship.transform;

        playerInfo.CoolDownAbility(cooldownTime, this);*/

        

        //spawn object with trigger
        //object can control the rest probably

    }

    public void ActiveAction(){
        missile.EndUse();
        canUse = false;
        isActive = false;
        isUpdating = false;
        playerInfo.CoolDownAbility(cooldownTime, this);

    }

     public override void OnUpdate(){
        if(missile == null){
            isActive = false;
            canUse = false;
            isUpdating = false;
            playerInfo.CoolDownAbility(cooldownTime, this);
        }

    }


}
