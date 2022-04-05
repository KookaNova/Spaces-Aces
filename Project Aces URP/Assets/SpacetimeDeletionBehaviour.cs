using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cox.PlayerControls;
using Photon.Pun;

public class SpacetimeDeletionBehaviour : MonoBehaviourPun
{
    [HideInInspector] public SpacecraftController owner;
    public float startUp;

    private bool isExpanding = true;
    private void Awake() {
        StartCoroutine(countdown());
    }

    private void Update() {
        if(isExpanding){
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, new Vector3(1000,1000,1000), 15 * Time.deltaTime);
        }
        else{
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, new Vector3(0, 0, 0), 7 * Time.deltaTime);
            if(this.transform.localScale == Vector3.zero){
                if(photonView.IsMine)PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider player){
        if(player.gameObject.GetComponent<SpacecraftController>()){
            var thisPlayer = player.gameObject.GetComponent<SpacecraftController>();
            if(thisPlayer != owner){
                Debug.Log("Delete this");
                thisPlayer.currentSpeed = 0;
            }
            
        }
    }
    private void OnTriggerExit(Collider player) {
        if(!isExpanding){
            if(player.gameObject.GetComponent<SpacecraftController>()){
               var thisPlayer = player.gameObject.GetComponent<SpacecraftController>();
               if(thisPlayer.photonView.ViewID != owner.photonView.ViewID){
                   Debug.Log("Delete this");
                   thisPlayer.TakeDamage(99999999, owner.photonView.ViewID, "Deleted");
               }
            }
        }
    }

    private IEnumerator countdown(){
        yield return new WaitForSecondsRealtime(startUp);
        isExpanding = false;
    }
}
