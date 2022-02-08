using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cox.PlayerControls;

public class ShipBehaviour : MonoBehaviour
{
    SpacecraftController sc;

    public void SetController(SpacecraftController newController){
        sc = newController;
    }

    private void OnCollisionEnter(Collision collision) {
        //On collision with hazards or other players, damage the player, based partially on speed.
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){
           sc.TakeDamage(sc.currentSpeed * 8, null, "accident");
        }
    }
    
}
