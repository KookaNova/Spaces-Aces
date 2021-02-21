using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{   
    public int destroyTime = 3;
    public float x, y, z;
    public GameObject explosion;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
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