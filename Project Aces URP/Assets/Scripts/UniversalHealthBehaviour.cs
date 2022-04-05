using Cox.PlayerControls;
using UnityEngine;
using Photon.Pun;

//this could be potentially worked into the targetable object script and work as a truly universal health object
//this isn't being done currently due to that requiring another refactor of the controller code, but this may not be that tough.

[RequireComponent(typeof(TargetableObject), typeof(Collider))]
public class UniversalHealthBehaviour : MonoBehaviourPun
{
    GameManager gm;
    //public float shields = 0;
    public float health = 700;
    
    [SerializeField] bool countsAsKill = false, isObjective = false;
    [SerializeField] GameObject explosion;
    public TargetableObject targetableObject;

    private void OnEnable() {
        gm = FindObjectOfType<GameManager>();
        targetableObject = this.GetComponent<TargetableObject>();

    }

    private void OnCollisionEnter(Collision hit) {
        if(hit.gameObject.GetComponent<GunAmmoBehaviour>()){
            var behaviour = hit.gameObject.GetComponent<GunAmmoBehaviour>();
            TakeDamage(behaviour.damageOutput, behaviour.owner.photonView.ViewID, behaviour.weaponName);
        }
        if(hit.gameObject.GetComponent<MissileBehaviour>()){
            var behaviour = hit.gameObject.GetComponent<MissileBehaviour>();
            TakeDamage(behaviour.damageOutput, behaviour.owner.photonView.ViewID, behaviour.weaponName);
        }
    }
    [PunRPC]
    public void TakeDamage(float damage, int ownerID, string cause){
        if(damage > health){
            Destroyed(ownerID, cause);
            return;
        }
        else{
            health -= damage;
            var owner = PhotonView.Find(ownerID).gameObject.GetComponent<SpacecraftController>();
            owner.photonView.RPC("TargetHit", RpcTarget.All);
        }
        
    }
    [PunRPC]
    private void Destroyed(int ownerID, string cause){
        var owner = PhotonView.Find(ownerID).gameObject.GetComponent<SpacecraftController>();
        owner.TargetDestroyed(countsAsKill);
        gm.photonView.RPC("FeedEvent", Photon.Pun.RpcTarget.All, owner.photonView.ViewID, photonView.ViewID, cause, false);
        var ex = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        ex.transform.SetParent(null);
        gameObject.SetActive(false);
    }

}
