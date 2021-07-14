using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MissileBehaviour : MonoBehaviour
{
    public int destroyTime = 10, missileSpeed = 800;
    public float seeking = 20, currentSpeed;
    public GameObject explosion, ignoredObj;
    public GameObject target = null;
    private Rigidbody _rb;
    private Collider _col;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _col.enabled = false;
        _rb.AddRelativeForce(0,0,currentSpeed + 300, ForceMode.Impulse);

        StartCoroutine(Missile());
        StartCoroutine(WakeCollider());
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        
        if(target != null)
        {
            if(Physics.SphereCast(gameObject.transform.position, 5, gameObject.transform.forward, out hit)){
                if(hit.collider.gameObject != target.gameObject){
                    target = null;
                    return;
                }
            }
            print(target);
            var toTarget = target.transform.position - _rb.position;
            var targetRotation = Quaternion.LookRotation(toTarget);
            
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, seeking);
        }
        else{
            _rb.rotation = _rb.rotation;
        }
        _rb.AddRelativeForce(0,0,missileSpeed, ForceMode.Acceleration);
    }


    private void OnCollisionEnter()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    private IEnumerator WakeCollider(){
        yield return new WaitForSeconds(.5f);
        _col.enabled = true;
        
    }

    private IEnumerator Missile()
    {
        if (target != null)
        {
            _rb.transform.LookAt(target.transform);
        }
        yield return new WaitForSeconds(destroyTime); Destroy(gameObject);

    }
}
