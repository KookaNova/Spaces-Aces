using System.Collections;
using UnityEngine;

public class TeleportAbility : AbilityHandler
{
    private Rigidbody rb;

    //On Activate() we teleport the character forward and start a cooldown.
    public override IEnumerator Activate(){
        if(playerInfo == null) {
            //Checks if player info was received.
            Debug.LogError("Teleport: Activate(), player is null. Something went wrong in either SpacecraftController or AbilityHandler");
            yield break;
        }
        if(!canUse)yield break;
        canUse = false;
        playerInfo.VoiceLine(1);
        //Plays startup effect or animation if added to the script
        var fx = Instantiate(startUpEffect, playerInfo.ship.transform);
        yield return new WaitForSecondsRealtime(startUpTime);

        Debug.Log("teleport");
            
        rb = playerInfo.ship.gameObject.GetComponent<Rigidbody>();
        Vector3 moveVec = rb.transform.forward * 1500;
        rb.MovePosition(rb.transform.position + moveVec);
        Instantiate(endEffect, playerInfo.ship.transform);
        playerInfo.CoolDownAbility(cooldownTime, this);

    }
    
}

