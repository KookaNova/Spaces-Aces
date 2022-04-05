using UnityEngine;

public class ArenaBoundaryBehaviour : MonoBehaviour
{
    private void OnTriggerExit(Collider obj) {
        if(obj.GetComponentInParent<Cox.PlayerControls.SpacecraftController>()){
            obj.GetComponentInParent<Cox.PlayerControls.SpacecraftController>().photonView.RPC("Eliminate", Photon.Pun.RpcTarget.All, null, "Unstoppable Force");
            return;
        }
    }
}
