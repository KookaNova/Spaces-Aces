using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetableObject),typeof(Rigidbody))]
public class AIBehaviour : MonoBehaviour
{
    public GameObject explosionObject;  
    public bool isAwaitingRespawn, isRotating = false;
    public float speed = 100;
    private Vector3 randRot = Vector3.zero;
    private Rigidbody _rb;

    public enum DecisionStates{
        RandomFlying,
    }

    private void Awake() {
        StartCoroutine(DecisionTime());
        _rb = GetComponent<Rigidbody>();
    }

    private IEnumerator DecisionTime(){
        float randTime = Random.Range(1f,3);
        yield return new WaitForSeconds(randTime);
        randRot = new Vector3(Random.Range(-30f,30), Random.Range(-30f,30), Random.Range(-30f,30));
        isRotating = true;
        float actionTime = Random.Range(1f,6f);
        yield return new WaitForSeconds(actionTime);
        isRotating = false;
        StartCoroutine(DecisionTime());
        
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Crash Hazard") || collision.gameObject.layer == LayerMask.NameToLayer("Player")){

            Eliminate();
            return;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("GunAmmo") || collision.gameObject.layer == LayerMask.NameToLayer("Missile")){
            Eliminate();
            return;
          
        }
    }

    public void Eliminate(){
        var deathLocation = transform.position;
        Object.Instantiate(explosionObject, deathLocation, transform.rotation);

        isAwaitingRespawn = true;

        gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + (transform.forward * 25), transform.forward);
        //Debug.DrawRay(transform.position + (transform.forward * 25), transform.forward * 700, Color.green, 0);
        if(Physics.Raycast(ray, 700, ~100, queryTriggerInteraction: QueryTriggerInteraction.Collide)){
            //print("hit");
            var rot = Quaternion.identity;
            _rb.AddRelativeTorque(-120,0,0, ForceMode.Force);
        }
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, randRot, 100 * Time.deltaTime);
        _rb.AddRelativeForce(0,0,speed, ForceMode.Acceleration);
        if(isRotating){
            _rb.AddRelativeTorque(randRot, ForceMode.Force);
        }
        
        
    }
}
