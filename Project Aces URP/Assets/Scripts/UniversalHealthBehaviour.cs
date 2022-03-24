using Cox.PlayerControls;
using UnityEngine;

//this could be potentially worked into the targetable object script and work as a truly universal health object
//this isn't being done currently due to that requiring another refactor of the controller code, but this may not be that tough.

[RequireComponent(typeof(TargetableObject), typeof(Collider))]
public class UniversalHealthBehaviour : MonoBehaviour
{
    GameManager gm;

    //public float shields = 0;
    public float health = 700;
    
    [SerializeField] bool countsAsKill = false, isObjective = false;
    [SerializeField] GameObject explosion;
    TargetableObject targetableObject;

    private void OnEnable() {
        gm = FindObjectOfType<GameManager>();
        targetableObject = this.GetComponent<TargetableObject>();

    }

    private void OnCollisionEnter(Collision hit) {
        if(hit.gameObject.GetComponent<GunAmmoBehaviour>()){
            var behaviour = hit.gameObject.GetComponent<GunAmmoBehaviour>();
            TakeDamage(behaviour.damageOutput, behaviour.owner, behaviour.weaponName);
        }
        if(hit.gameObject.GetComponent<MissileBehaviour>()){
            var behaviour = hit.gameObject.GetComponent<MissileBehaviour>();
            TakeDamage(behaviour.damageOutput, behaviour.owner, behaviour.weaponName);
        }
    }

    public void TakeDamage(float damage, SpacecraftController owner, string cause){
        if(damage > health){
            Destroyed(owner, cause);
            return;
        }
        else{
            health -= damage;
            owner.TargetHit();
        }
        
    }

    private void Destroyed(SpacecraftController owner, string cause){
        owner.TargetDestroyed(countsAsKill);
        gm.FeedEvent(owner, targetableObject.nameOfTarget, cause, countsAsKill);
        var ex = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        ex.transform.SetParent(null);
        gameObject.SetActive(false);
    }

}
