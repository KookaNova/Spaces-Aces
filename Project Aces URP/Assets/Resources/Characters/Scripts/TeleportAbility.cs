using System.Collections;
using UnityEngine;
using Cox.PlayerControls;

public class TeleportAbility : AbilityHandler
{
    private Rigidbody rb;

    //On Activate() we teleport the character forward and start a cooldown.

    public override IEnumerator Activate(SpacecraftController owner){
        if(owner == null) {
            //Checks if player info was received.
            Debug.LogError("Teleport: Activate(), player is null. Something went wrong in either SpacecraftController or AbilityHandler");
            yield break;
        }
        if(!canUse)yield break;
        canUse = false;
        owner.VoiceLine(1);
        //Plays startup effect or animation if added to the script
        var fx = Instantiate(startUpEffect, owner.ship.transform);
        yield return new WaitForSecondsRealtime(startUpTime);

        Debug.Log("teleport");
            
        rb = owner.ship.gameObject.GetComponent<Rigidbody>();
        Vector3 moveVec = rb.transform.forward * 1500;
        rb.MovePosition(rb.transform.position + moveVec);
        Instantiate(endEffect, owner.ship.transform);
        owner.CoolDownAbility(cooldownTime, this);

    }
    
}

