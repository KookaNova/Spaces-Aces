using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class projectileBehaviour : MonoBehaviour
{   
    public float x, y, z;

    private void Start()
    {
        StartCoroutine(TimedDestroy());
    }

    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }

    private IEnumerator TimedDestroy()
    {

        yield return new WaitForSeconds(2); Destroy(gameObject);

    }
}
