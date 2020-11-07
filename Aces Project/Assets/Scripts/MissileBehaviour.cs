using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MissileBehaviour : MonoBehaviour
{    
    public int destroyTime = 10;
    public int missileSpeed;
    public GameObject explosion;
    
   
    private Transform _target;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(TimedDestroy());
        
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit, 40, 10))
        {

            _target = hit.collider.transform;
        }
        if(_target != null)
        _rb.transform.LookAt(_target);
        _rb.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 80 * missileSpeed);
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
