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

    [PunRPC]
    public void SetController(int scID){
        sc = PhotonView.Find(scID).gameObject.GetComponent<SpacecraftController>();
        transform.SetParent(sc.gameObject.transform);
        sc = GetComponentInParent<SpacecraftController>();
        targetableObject = GetComponent<TargetableObject>();
        targetableObject.targetTeam = sc.teamInt;
        targetableObject.nameOfTarget = sc.playerName;
        sc.shipBehaviour = this;
        sc.targetableObject = this.targetableObject;
        gm = FindObjectOfType<GameManager>();
        gm.photonView.RPC("AddTarget", RpcTarget.AllBuffered ,targetableObject.gameObject.GetPhotonView().ViewID, targetableObject.targetTeam);
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
            sc.photonView.RPC("TakeDamage", RpcTarget.All, sc.currentSpeed * 8, -1, "accident");
            if(collision.gameObject.GetComponent<UniversalHealthBehaviour>()){
                var behaviour = collision.gameObject.GetComponent<UniversalHealthBehaviour>();
                behaviour.photonView.RPC("TakeDamage", RpcTarget.All, sc.currentSpeed, sc.photonView.ViewID, "Collision");
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
