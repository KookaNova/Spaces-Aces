using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
public class GunAmmoBehaviour : MonoBehaviourPun
{
    [HideInInspector] public SpacecraftController owner = null;

    [SerializeField] private float destroyTime = 6, colliderDelay = .1f;
    
    public float damageOutput = 223, speed = 1000;
    public GameObject impactObj;

    private Collider thisCollider;

    private void Awake() {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = false;
        StartCoroutine(StartUp());
        
    }

    public void OnCollisionEnter(Collision obj) {
        if(impactObj != null){
            var impact = Instantiate(impactObj);
            impact.transform.position = transform.position;
        }
        
        if(obj.gameObject.GetComponentInParent<SpacecraftController>()){
            obj.gameObject.GetComponentInParent<SpacecraftController>().TakeDamage(damageOutput, owner, "gun");
            owner.TargetHit();
        }
        if(photonView.IsMine) PhotonNetwork.Destroy(this.gameObject);
    }

    private IEnumerator StartUp(){
        yield return new WaitForSeconds(colliderDelay);
        thisCollider.enabled = true;

        yield return new WaitForSeconds(destroyTime);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
}
