using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MissileBehaviour : MonoBehaviour
{
    public int destroyTime = 10, missileSpeed = 650;
    public float seeking = 3, closeSeeking = 1, currentSpeed;
    public GameObject explosion, ignoredObj;
    public Transform target = null;
    private Rigidbody _rb;
    private Collider _col;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _col.enabled = false;
        _rb.AddRelativeForce(0,0,currentSpeed + 300, ForceMode.VelocityChange);

        StartCoroutine(Missile());
        StartCoroutine(WakeCollider());
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            var toTarget = target.position - _rb.position;
            var targetRotation = Quaternion.LookRotation(toTarget);
            
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, seeking);
        }
        _rb.AddRelativeForce(0,0,missileSpeed, ForceMode.Acceleration);
    }

    private void OnCollisionEnter()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider obj)
    {
        if(target == null)return;
        if(obj.gameObject == target.gameObject){
            seeking = closeSeeking;
            missileSpeed = missileSpeed - 300;
            StartCoroutine(LockBreak());
        }
    }

    private IEnumerator WakeCollider(){
        yield return new WaitForSeconds(.25f);
        _col.enabled = true;
    }

    private IEnumerator LockBreak(){
        yield return new WaitForSeconds(.25f);
        target = null;
    }

    private IEnumerator Missile()
    {
        if (target != null)
        {
            _rb.transform.LookAt(target);
        }
        yield return new WaitForSeconds(destroyTime); Destroy(gameObject);

    }
}
