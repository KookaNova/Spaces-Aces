using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MissileBehaviour : MonoBehaviour
{
    public int destroyTime = 10;
    public float missileSpeed = 800, turningLimit = 20, missProbability = 2, currentSpeed, damageOutput = 1000;
    [SerializeField]
    private GameObject explosion;
    [HideInInspector]
    public GameObject target = null;
    public Rigidbody rb;
    private Collider _col;
    private bool startCasting = false;
    private TrailRenderer trail;

    [HideInInspector]public bool missileHit = false, missileMissed = false;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _col.enabled = false;
        rb.velocity = transform.forward * (currentSpeed + missileSpeed);

        StartCoroutine(Missile());
        StartCoroutine(WakeCollider());
        
    }

    private void Update()
    {
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (gameObject.transform.forward * 2500), Color.red);
        RaycastHit hit;
        
        if(target != null)
        {
            if(startCasting == true){
                if(Physics.SphereCast(gameObject.transform.position, missProbability, gameObject.transform.forward, out hit)){
                    if(hit.rigidbody == null || hit.rigidbody.gameObject != target.gameObject){
                        target = null;
                        Debug.Log("MissileBehaviour: Spherecast obstructed. Missile missed.");
                        return;
                    }
                }
                else{
                    target = null;
                    missileMissed = true;

                    Debug.Log("MissileBehaviour: Spherecast missed the target. Missile missed.");
                    return;
                }
            }
            
            var toTarget = target.transform.position - rb.position;
            var targetRotation = Vector3.RotateTowards(rb.transform.forward, toTarget, turningLimit, 1080);
            //var step = turningLimit * Time.fixedDeltaTime;

            //_rb.rotation = targetRotation;
            rb.transform.rotation = Quaternion.LookRotation(targetRotation);
        }
        else{
            rb.rotation = rb.rotation;
        }
        
    }

    private void FixedUpdate() {
        rb.AddRelativeForce(0,0,currentSpeed + missileSpeed, ForceMode.Acceleration);
    }


    private void OnCollisionEnter(Collision obj)
    {

        Instantiate(explosion, transform.position, Quaternion.identity);
        if(obj.gameObject.GetComponent<SpacecraftController>()){
            obj.gameObject.GetComponent<SpacecraftController>().currentHealth -= damageOutput;
        }
        trail.transform.parent = null;
        Destroy(gameObject);
    }


    private IEnumerator WakeCollider(){
        yield return new WaitForSeconds(.75f);
        _col.enabled = true;
        startCasting = true;
        
    }

    private IEnumerator Missile()
    {
        if (target != null)
        {
            rb.transform.LookAt(target.transform);
        }
        yield return new WaitForSecondsRealtime(destroyTime); trail.transform.parent = null; Destroy(gameObject);

    }
}
}
