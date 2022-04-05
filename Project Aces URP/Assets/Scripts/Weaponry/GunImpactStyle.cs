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
            if(photonView.IsMine){
                if(obj.gameObject.GetComponentInParent<SpacecraftController>()){
                    var hitObj = obj.gameObject.GetComponentInParent<SpacecraftController>();
                    if(hitObj == owner){
                        return;
                    }
                    else{
                        if(photonView.IsMine){
                            hitObj.photonView.RPC("TakeDamage", RpcTarget.All, damageOutput, owner.photonView.ViewID, weaponName);
                            owner.photonView.RPC("TargetHit", RpcTarget.All);
                        }
                    
                    }
                }
                EndUse();
            }
           
        }

        private IEnumerator StartUp(){
            yield return new WaitForSeconds(colliderDelay);
            thisCollider.enabled = true;
            yield return new WaitForSeconds(destroyTime);
            if(photonView.IsMine)PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
