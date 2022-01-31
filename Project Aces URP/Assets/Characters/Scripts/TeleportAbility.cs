using System.Collections;
using UnityEngine;

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
        canUse = false;
        player.VoiceLine(1);
        //Plays startup effect or animation if added to the script
        Instantiate(startUpParticle, player.transform);
        yield return new WaitForSecondsRealtime(startUpTime);

        Debug.Log("teleport");
            
        rb = player.gameObject.GetComponent<Rigidbody>();
        Vector3 moveVec = rb.transform.forward * 1500;
        rb.MovePosition(rb.transform.position + moveVec);
        Instantiate(endParticle, player.gameObject.transform);
        player.CoolDownAbility(cooldownTime, this);

    }
    
}

