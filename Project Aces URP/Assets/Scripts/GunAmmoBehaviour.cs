using System.Collections;
using UnityEngine;

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
        Destroy(this.gameObject);
    }

    private IEnumerator DestroyCounter(){
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
