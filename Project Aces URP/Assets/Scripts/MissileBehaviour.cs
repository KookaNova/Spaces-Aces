using System.Collections;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MissileBehaviour : MonoBehaviour
{
    public int destroyTime = 10, missileSpeed = 800;
    public float seeking = 20, currentSpeed, damageOutput = 1000;
    public GameObject explosion;
    public GameObject target = null;
    private Rigidbody _rb;
    private Collider _col;
    private bool startCasting = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _col.enabled = false;
        _rb.velocity = transform.forward * (currentSpeed + missileSpeed);

        StartCoroutine(Missile());
        StartCoroutine(WakeCollider());
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        
        if(target != null)
        {
            if(startCasting == true){
                if(Physics.SphereCast(gameObject.transform.position, 75, gameObject.transform.forward, out hit)){
                    if(hit.collider.gameObject != target.gameObject){
                        target = null;
                        return;
                    }
                }
                else{
                    target = null;
                    return;
                }
            }

            print(target);
            var toTarget = target.transform.position - _rb.position;
            var targetRotation = Quaternion.LookRotation(toTarget);
            
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, seeking * 100 * Time.deltaTime);
        }
        else{
            _rb.rotation = _rb.rotation;
        }
        _rb.AddRelativeForce(0,0,currentSpeed + missileSpeed, ForceMode.Acceleration);
    }


    private void OnCollisionEnter(Collision obj)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        if(obj.gameObject.GetComponent<SpacecraftController>()){
            obj.gameObject.GetComponent<SpacecraftController>().currentHealth -= 1000;
        }
        PhotonNetwork.Destroy(gameObject);
    }


    private IEnumerator WakeCollider(){
        yield return new WaitForSeconds(.5f);
        _col.enabled = true;
        startCasting = true;
        
    }

    private IEnumerator Missile()
    {
        if (target != null)
        {
            _rb.transform.LookAt(target.transform);
        }
        yield return new WaitForSeconds(destroyTime); PhotonNetwork.Destroy(gameObject);

    }
}
