using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetableObject),typeof(Rigidbody))]
public class AIBehaviour : MonoBehaviour
{
    public GameObject explosionObject;  
    private Quaternion randRot = new Quaternion(0,0,0,0);
    private Rigidbody _rb;

    public enum DecisionStates{
        RandomFlying,
    }

    private void Awake() {
        StartCoroutine(DecisionTime());
        _rb = GetComponent<Rigidbody>();
    }

    private IEnumerator DecisionTime(){
        float randTime = Random.Range(5,15);
        yield return new WaitForSeconds(randTime);
        randRot = Random.rotation;
        StartCoroutine(DecisionTime());
        
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){

           var deathLocation = transform.position;
           Object.Instantiate(explosionObject, deathLocation, transform.rotation);

           Destroy(gameObject);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("GunAmmo") || collision.gameObject.layer == LayerMask.NameToLayer("Missile")){

           var deathLocation = transform.position;
           Object.Instantiate(explosionObject, deathLocation, transform.rotation);

           Destroy(gameObject);
        }
    }

    private void FixedUpdate() {

        transform.rotation = randRot;
        _rb.AddRelativeForce(0,0,170);
        
    }
}
