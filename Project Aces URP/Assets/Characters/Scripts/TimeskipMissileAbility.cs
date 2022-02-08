using System.Collections;
using UnityEngine;
using Cox.PlayerControls;
using Photon.Pun;

public class TimeskipMissileAbility : AbilityHandler
{
    private MissileBehaviour missile = null;

    //On Activate() we create a missile and it tracks the target.
    public override IEnumerator Activate(){
        if(playerInfo == null) {
            //Checks if player info was received.
            Debug.LogError("Teleport: Activate(), player is null. Something went wrong in either SpacecraftController or AbilityHandler");
            yield break;
        }
        if(!canUse){
            yield break;
        }
        if(isActive){
            ActiveAction();
            yield break;
        }
        else{
            Debug.Log("Timeskip Missile: Launch");
            var weapons = playerInfo.weaponSystem;

            //launches missile
            GameObject m =  PhotonNetwork.Instantiate(weapons.missileType.gameObject.name, weapons.missilePosition[weapons.currentMis].position, weapons.missilePosition[weapons.currentMis].rotation);
            missile = m.GetComponent<MissileBehaviour>();
            missile.owner = playerInfo;
            missile.currentSpeed = playerInfo.GetComponent<SpacecraftController>().currentSpeed;
            
            if(weapons.missileLocked){
                missile.target = weapons.currentTargetSelection[weapons.currentTarget].gameObject;
            }
            
            isActive = true;
        }
    } 

    private void ActiveAction(){
        canUse = false;
        isActive = false;
        if(missile == null){
            return;
        }

        if(missile.target != null){
            missile.gameObject.transform.LookAt(missile.target.transform.position + (missile.target.transform.forward), Vector3.up);
            
        }
        missile.missProbability -= 1.5f;
        missile.turningLimit *= 3;
        missile.missileSpeed *= 1.2f;
        Vector3 moveVec = missile.transform.forward * 400;
        missile.rb.MovePosition(missile.rb.transform.position + moveVec);
        Instantiate(endEffect, missile.transform.position, missile.transform.rotation);
        missile.gameObject.GetComponentInChildren<TrailRenderer>().widthMultiplier *= 5;
        Debug.Log("Timeskip Missile: Skip!");
        
        playerInfo.CoolDownAbility(cooldownTime, this);
        playerInfo.VoiceLine(2);
        

    }

    public override void OnUpdate(){
        if(missile == null){
            isActive = false;
            canUse = false;
        }
        if(missile.missileMissed){
            isActive = false;
            canUse = false;
            playerInfo.CoolDownAbility(cooldownTime, this);
        }

    }
    
}

