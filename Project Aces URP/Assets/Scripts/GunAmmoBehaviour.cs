using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
public class GunAmmoBehaviour : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 6, colliderDelay = .1f;
    public float damageOutput = 223;
    public GameObject impactObj;

    private Collider thisCollider;

    private void Awake() {
        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = false;
        StartCoroutine(StartUp());
    }

    public void OnCollisionEnter(Collision obj) {
        if(impactObj != null){
            var impact = Instantiate(impactObj);
            impact.transform.position = transform.position;
        }
        
        if(obj.gameObject.GetComponent<SpacecraftController>()){
            obj.gameObject.GetComponent<SpacecraftController>().TakeDamage(damageOutput);
        }
        PhotonNetwork.Destroy(this.gameObject);
    }

    private IEnumerator StartUp(){
        yield return new WaitForSeconds(colliderDelay);
        thisCollider.enabled = true;

        yield return new WaitForSeconds(destroyTime);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
}
