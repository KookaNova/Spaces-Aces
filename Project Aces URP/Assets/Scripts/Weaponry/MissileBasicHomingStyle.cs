using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{

    public class MissileBasicHomingStyle : MissileBehaviour
    {

        private void Update(){
            int layer = 1 << 14;

            Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (gameObject.transform.forward * 2500), Color.red);
            RaycastHit hit;
        
            //Target Check
            if(target != null){
                if(target.GetComponentInParent<SpacecraftController>()){
                    sc = target.GetComponentInParent<SpacecraftController>();
                    sc.missileChasing = true;
                }
                if(startCasting == true){
                    //Target Check
                    if(Physics.SphereCast(gameObject.transform.position, missProbability, gameObject.transform.forward, out hit, 5000, ~layer)){
                        //miss
                        if(hit.rigidbody == null || hit.rigidbody.gameObject != target){
                            target = null;
                            missileMissed = true;
                            sc.missileChasing = false;
                            sc.missileClose = false;

                        
                            Debug.Log("MissileBehaviour: Spherecast obstructed. Missile missed.");
                            if(hit.rigidbody == null){
                                Debug.Log("Missile: Raycast did not hit a RigidBody");
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


        
    
    }
}
