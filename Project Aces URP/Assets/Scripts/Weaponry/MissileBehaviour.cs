using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class MissileBehaviour : MonoBehaviourPun
{
    public string weaponName = "Missile";
    public int destroyTime = 10;
    public float missileSpeed = 800, turningLimit = 20, missProbability = 2, currentSpeed, damageOutput = 1000;

    public GameObject explosion;
    [HideInInspector]public SpacecraftController owner = null;
    [HideInInspector]public GameObject target = null;
    [HideInInspector]public Rigidbody rb;
    private Collider _col;
    protected bool startCasting = false;
    public TrailRenderer trail;
    public bool isEmitting = true;

    protected SpacecraftController sc;

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

    protected virtual void OnCollisionEnter(Collision obj){
        if(obj.gameObject.GetComponentInParent<SpacecraftController>()){
            obj.gameObject.GetComponentInParent<SpacecraftController>().TakeDamage(damageOutput, owner, weaponName);
            if(owner.isActiveAndEnabled){
                 owner.TargetHit();
            }
            
            if(sc != null){
                sc.missileChasing = false;
            }
        }
        EndUse();
    }

    public virtual void EndUse(){
        Instantiate(explosion, transform.position, Quaternion.identity);
        trail.transform.parent = null;
        if(photonView.IsMine) PhotonNetwork.Destroy(gameObject);
    }


    protected virtual IEnumerator WakeCollider(){
        yield return new WaitForSeconds(.1f);
        _col.enabled = true;
        startCasting = true;
        
    }

    protected virtual IEnumerator Missile()
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
