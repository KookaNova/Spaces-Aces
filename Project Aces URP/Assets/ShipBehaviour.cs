using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cox.PlayerControls;

public class ShipBehaviour : MonoBehaviour
{
    SpacecraftController sc;
    public AudioSource collisionSound, gearsResolving, missileWarning, missileClose;
    public bool isTurning;

    public void SetController(SpacecraftController newController){
        sc = newController;
    }

    private void OnCollisionEnter(Collision collision) {
        //On collision with hazards or other players, damage the player, based partially on speed.
        if(sc == null)return;
        
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){
           sc.TakeDamage(sc.currentSpeed * 8, null, "accident");
        }
        float ran = Random.Range(.8f, 1.2f);
        collisionSound.pitch = ran;
        collisionSound.Play();
    }

    private void Update() {
        if(isTurning){

        }
        else{
            ResolveGearsTurning();
        }
    }

    public void ResolveGearsTurning(){
        isTurning = false;
        float ran = Random.Range(.8f, 1.2f);
        gearsResolving.pitch = ran;
        gearsResolving.Play();
    }
    
}
