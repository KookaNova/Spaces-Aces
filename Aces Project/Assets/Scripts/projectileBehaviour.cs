using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class projectileBehaviour : MonoBehaviour
{   
    public int destroyTime = 3;
    public float x, y, z;
    public GameObject explosion;

    private void Start()
    {
        StartCoroutine(TimedDestroy());
    }

    void OnCollisionEnter()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    private IEnumerator TimedDestroy()
    {

        yield return new WaitForSeconds(destroyTime); Destroy(gameObject);

    }
}
