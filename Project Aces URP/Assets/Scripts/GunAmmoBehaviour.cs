using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
public class GunAmmoBehaviour : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 6;
    public float damageOutput = 223;
    public GameObject impactObj;

    private void Awake() {
        StartCoroutine(DestroyCounter());
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

    private IEnumerator DestroyCounter(){
        yield return new WaitForSeconds(destroyTime);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
}
