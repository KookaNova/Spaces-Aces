using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class MissileBehaviour : MonoBehaviourPun
{
    public int destroyTime = 10;
    public float missileSpeed = 800, turningLimit = 20, missProbability = 2, currentSpeed, damageOutput = 1000;

    public GameObject explosion;
    [HideInInspector]public SpacecraftController owner = null;
    [HideInInspector]public GameObject target = null;
    [HideInInspector]public Rigidbody rb;
    private Collider _col;
    private bool startCasting = false;
    public TrailRenderer trail;
    public bool isEmitting = true;

    private SpacecraftController sc;

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
        int layer = 1 << 14;

        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (gameObject.transform.forward * 2500), Color.red);
        RaycastHit hit;
        
        if(target != null){
            if(target.GetComponentInParent<SpacecraftController>()){
               sc = target.GetComponentInParent<SpacecraftController>();
               sc.missileChasing = true;
            }
            if(startCasting == true){
                if(Physics.SphereCast(gameObject.transform.position, missProbability, gameObject.transform.forward, out hit, 5000, ~layer)){
                    if(hit.rigidbody == null || hit.rigidbody.gameObject != target){
                        target = null;
                        missileMissed = true;
                        sc.missileChasing = false;
                        sc.missileClose = false;
                        
                        Debug.Log("MissileBehaviour: Spherecast obstructed. Missile missed.");
                        if(hit.rigidbody == null){
                            Debug.Log("Missile: Did not hit a RigidBody");
                            return;
                        }
                        
                        return;
                    }
                    if(hit.distance < 600){
                        sc.missileClose = true;
                    }
                    else{
                        sc.missileClose = false;
                    }
                }
                else{
                    target = null;
                    missileMissed = true;
                    sc.missileChasing = false;
                    sc.missileClose = false;

                    Debug.Log("MissileBehaviour: Spherecast missed the target. Missile missed. No hit.");
                    return;
                }
            }
            
            var toTarget = target.transform.position - rb.position;
            var targetRotation = Vector3.RotateTowards(rb.transform.forward, toTarget, turningLimit * Time.fixedDeltaTime, 1080);
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
        if(isEmitting){
            trail.emitting = true;
        }
    }


    private void OnCollisionEnter(Collision obj)
    {
        if(obj.gameObject.GetComponentInParent<SpacecraftController>()){
            obj.gameObject.GetComponentInParent<SpacecraftController>().TakeDamage(damageOutput, owner, "missile");
            if(owner.isActiveAndEnabled){
                owner.TargetHit();
            }
            
            if(sc != null){
                sc.missileChasing = false;
            }
        }
        EndUse();
    }

    public void EndUse(){
        Instantiate(explosion, transform.position, Quaternion.identity);
        trail.transform.parent = null;
        if(photonView.IsMine) PhotonNetwork.Destroy(gameObject);
    }


    private IEnumerator WakeCollider(){
        yield return new WaitForSeconds(.1f);
        _col.enabled = true;
        startCasting = true;
        
    }

    private IEnumerator Missile()
    {
        if (target != null)
        {
            rb.transform.LookAt(target.transform);
        }
        yield return new WaitForSecondsRealtime(destroyTime); 
        trail.transform.parent = null; PhotonNetwork.Destroy(gameObject);

    }
}
}
