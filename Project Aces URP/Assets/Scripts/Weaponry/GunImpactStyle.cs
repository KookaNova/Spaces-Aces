using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
    public class GunImpactStyle : GunAmmoBehaviour
    {
        private void Awake() {
            this.GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
            thisCollider = GetComponent<Collider>();
            thisCollider.enabled = false;
            StartCoroutine(StartUp());
        }

        private void OnCollisionEnter(Collision obj) {
           if(obj.gameObject.GetComponentInParent<SpacecraftController>()){
                var controller = obj.gameObject.GetComponentInParent<SpacecraftController>();
                if(controller == owner){
                    return;
                }
                else{
                    if(photonView.IsMine){
                        controller.photonView.RPC("TakeDamage", RpcTarget.All, damageOutput, owner, weaponName);
                        //controller.TakeDamage(damageOutput, owner, "gun");
                        owner.TargetHit();
                    }
                    
                }
            }
            EndUse();
        }

        private IEnumerator StartUp(){
            yield return new WaitForSeconds(colliderDelay);
            thisCollider.enabled = true;

            yield return new WaitForSeconds(destroyTime);

            if(photonView.IsMine)PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
