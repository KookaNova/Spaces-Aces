using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cox.PlayerControls;
using Photon.Pun;

public class ShipBehaviour : MonoBehaviourPun
{
    SpacecraftController sc;
    public TargetableObject targetableObject;
    GameManager gm;
    public AudioSource collisionSound, gearsResolving, missileWarning, lockOn;
    public bool isTurning;

    private void Awake() {
        targetableObject = GetComponent<TargetableObject>();
        gm = FindObjectOfType<GameManager>();
        gm.photonView.RPC("AddTarget", RpcTarget.AllBuffered ,targetableObject.gameObject.GetPhotonView().ViewID, targetableObject.targetTeam);
        
    }

    public void SetController(SpacecraftController newController){
        sc = newController;
    }

    private void OnCollisionEnter(Collision collision) {
        //On collision with hazards or other players, damage the player, based partially on speed.
        if(sc == null)return;
        float ran = Random.Range(.8f, 1.2f);
        if(this.isActiveAndEnabled){
            collisionSound.pitch = ran;
            collisionSound.Play();
        }
        
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){
           sc.TakeDamage(sc.currentSpeed * 8, null, "accident");
           if(collision.gameObject.GetComponent<UniversalHealthBehaviour>()){
               var behaviour = collision.gameObject.GetComponent<UniversalHealthBehaviour>();
               behaviour.TakeDamage(sc.currentSpeed, sc, "Collision");
           }
        }
    }

    private void Update() {
        if(isTurning){

        }
        else{
            ResolveGearsTurning();
        }
    }

    public void ResolveGearsTurning(){
        if(!this.isActiveAndEnabled)return;
        isTurning = false;
        float ran = Random.Range(.8f, 1.2f);
        gearsResolving.pitch = ran;
        gearsResolving.Play();
    }
    
}
