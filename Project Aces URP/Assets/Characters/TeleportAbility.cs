using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Controller/Ability/Teleport")]
public class TeleportAbility : AbilityHandler
{
    private Rigidbody rb;

    //On Activate() we teleport the character forward and start a cooldown.
    public override IEnumerator Activate(){
        if(player == null) {
            //Checks if player info was received.
            Debug.LogError("Teleport: Activate(), player is null. Something went wrong in either SpacecraftController or AbilityHandler");
            yield break;
        }
        if(!canUse)yield break;

        //Plays startup effect or animation if added to the script
        Instantiate(startUpParticle, player.transform);
        yield return new WaitForSeconds(startUpTime);

        if(!isActive){
            Debug.Log("teleport");
            isActive = true;
            canUse = false;
            
            rb = player.GetComponent<Rigidbody>();
            Vector3 moveVec = rb.transform.forward * 1500;
            rb.MovePosition(rb.transform.position + moveVec);
            Instantiate(endParticle, player.transform);
            isActive = false;
            yield break;
        }
    }
    
}

