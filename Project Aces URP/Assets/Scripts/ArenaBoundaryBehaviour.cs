using UnityEngine;

public class ArenaBoundaryBehaviour : MonoBehaviour
{
    private void OnTriggerExit(Collider obj) {
        if(obj.GetComponentInParent<Cox.PlayerControls.SpacecraftController>()){
            obj.GetComponentInParent<Cox.PlayerControls.SpacecraftController>().Eliminate();
            return;
        }
        if(obj.GetComponentInParent<Cox.PlayerControls.SpacecraftController>()){
            obj.GetComponentInParent<AIBehaviour>().Eliminate();
            return;
        }
    }
}
