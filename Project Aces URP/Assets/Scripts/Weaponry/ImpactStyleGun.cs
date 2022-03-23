using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Cox.PlayerControls{
    public class ImpactStyleGun : GunAmmoBehaviour
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
                    controller.TakeDamage(damageOutput, owner, "gun");
                    owner.TargetHit();
                }
            }
            if(impactFX != null){
                var impact = Instantiate(impactFX);
                impact.transform.position = transform.position;
            }
            if(photonView.IsMine) PhotonNetwork.Destroy(this.gameObject);
        }

        private IEnumerator StartUp(){
            yield return new WaitForSeconds(colliderDelay);
            thisCollider.enabled = true;

            yield return new WaitForSeconds(destroyTime);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
