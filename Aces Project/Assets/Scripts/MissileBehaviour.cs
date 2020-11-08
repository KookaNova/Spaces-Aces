using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MissileBehaviour : MonoBehaviour
{    
    public int destroyTime = 10;
    public int missileSpeed;
    public GameObject explosion;

    public Transform target = null;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(TimedDestroy());
        StartCoroutine(TargetForce());


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
    private IEnumerator Force()
    {
        _rb.AddRelativeForce(0,0, 350 * missileSpeed, ForceMode.Acceleration);
        yield return new WaitForSeconds(.1f);
        StartCoroutine(AimedForce());
    }
    private IEnumerator AimedForce()
    {

        _rb.transform.LookAt(target);
        _rb.AddRelativeForce(0,0, 350 * missileSpeed, ForceMode.Acceleration);
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Force());
    }
    private IEnumerator TargetForce()
    {
        yield return new WaitForSeconds(.3f);
        StartCoroutine(AimedForce());

    }
}
