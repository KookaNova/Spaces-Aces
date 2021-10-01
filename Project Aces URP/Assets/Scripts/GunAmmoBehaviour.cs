using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
public class GunAmmoBehaviour : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 6, damageOutput = 223;
    public GameObject impactObj;

    private void Awake() {
        StartCoroutine(DestroyCounter());
    }

    public void OnCollisionEnter(Collision obj) {
        var impact = Instantiate(impactObj);
        if(obj.gameObject.GetComponent<SpacecraftController>()){
            obj.gameObject.GetComponent<SpacecraftController>().TakeDamage(damageOutput);
        }
        impact.transform.position = transform.position;
        PhotonNetwork.Destroy(this.gameObject);
    }

    private IEnumerator DestroyCounter(){
        yield return new WaitForSeconds(destroyTime);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
}
