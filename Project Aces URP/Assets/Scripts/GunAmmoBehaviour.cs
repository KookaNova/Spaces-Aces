using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GunAmmoBehaviour : MonoBehaviour
{
    public float destroyTime = 6;
    public GameObject impactObj;

    private void Awake() {
        StartCoroutine(DestroyCounter());
    }

    public void OnCollisionEnter(Collision other) {
        var a = Instantiate(impactObj);
        a.transform.position = transform.position;
        PhotonNetwork.Destroy(this.gameObject);
    }

    private IEnumerator DestroyCounter(){
        yield return new WaitForSeconds(destroyTime);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
