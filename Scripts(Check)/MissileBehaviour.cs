using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MissileBehaviour : MonoBehaviour
{    
    public int destroyTime = 10, missileSpeed = 650;
    public float seeking = 3, closeSeeking = 1;
    public GameObject explosion;
    public Transform target = null;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(TimedDestroy());
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            var toTarget = target.position - _rb.position;
            var targetRotation = Quaternion.LookRotation(toTarget);
            
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, seeking);
        }
        _rb.AddRelativeForce(0,0, missileSpeed, ForceMode.Acceleration);
    }

    void OnCollisionEnter()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        seeking = closeSeeking;
    }

    private void OnTriggerExit(Collider other)
    {
        seeking = 0;
    }

    private IEnumerator TimedDestroy()
    {
        if (target != null)
        {
            _rb.transform.LookAt(target);
        }
        yield return new WaitForSeconds(destroyTime); Destroy(gameObject);

    }
}
