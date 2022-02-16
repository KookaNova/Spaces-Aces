using System.Collections;
using UnityEngine;
namespace Cox.PlayerControls{
public class CameraController : MonoBehaviour
{
    public WeaponsController weaponsController;
    public Cinemachine.CinemachineVirtualCamera[] cameras;
    [HideInInspector] public int currentCamera = 0;
    bool isCameraTargetLocked = false;
    bool mouseActive = false;
    bool waitingRespawn = false;
    GameObject followTarget;
    Vector2 rotationInput;
    Vector3 target;

    public void Activate(){
        for(int i = 0; i < cameras.Length; i++){
            cameras[i].gameObject.SetActive(false);
        }
        cameras[0].gameObject.SetActive(true);
    }

    public void RotateCamera(Vector2 input, bool isMouse){
        mouseActive = isMouse;
        if(mouseActive){
            if(input == Vector2.zero){
                rotationInput = Vector2.zero;
            }
            rotationInput.y = input.y;
            rotationInput.x = input.x;
        }
        else{
            rotationInput.y = input.y * 70;
            rotationInput.x = input.x * 120;
        }
    }

    public void ChangeCamera(){
        currentCamera ++;
        if(currentCamera >= cameras.Length)currentCamera = 0;
        for(int i = 0; i < cameras.Length; i++){
            cameras[i].gameObject.SetActive(false);
        }
        cameras[currentCamera].gameObject.SetActive(true);
    }

    public void CameraLockTarget(){
        isCameraTargetLocked = !isCameraTargetLocked;
        Debug.Log(weaponsController.currentTarget);
        if(weaponsController.currentTarget == -1 || weaponsController.currentTargetSelection.Count <= 0){
            isCameraTargetLocked = false;
            return;
        }
        else if(weaponsController.currentTargetSelection[weaponsController.currentTarget] == null){
            isCameraTargetLocked = false;
            return;
        }
    }

    public void FollowTarget(bool isCamFollowTarget, GameObject target){
        waitingRespawn = followTarget;
        followTarget = target;
        if(!waitingRespawn){
            followTarget = null;
        }
    }

    private void LateUpdate(){
        if(rotationInput == Vector2.zero){
            gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, Quaternion.identity, .05f);
        }
        if(gameObject.transform.localRotation.x >= 20 || gameObject.transform.localRotation.x <= -90 || gameObject.transform.localRotation.y >= 120 || gameObject.transform.localRotation.y <= -120){
            CameraLockTarget();
        }
        //if camera isn't locked, use controller input
        if(!isCameraTargetLocked){
            var targetRotation = Quaternion.Euler(-rotationInput.y, rotationInput.x, 0);
            
            if(mouseActive){
                gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, targetRotation, .05f);
            }
            else{
                gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, targetRotation, .05f);
            }
            gameObject.transform.localRotation = new Quaternion(
                Mathf.Clamp(gameObject.transform.localRotation.x, -1f, .3f), 
                Mathf.Clamp(gameObject.transform.localRotation.y, -.85f, .85f), 
                Mathf.Clamp(gameObject.transform.localRotation.z, -.1f, .1f), 
                Mathf.Clamp(gameObject.transform.localRotation.w, -1, 1));
        } // else if Camera is Target Locked
        else{
            Debug.Log(weaponsController.currentTarget);
            if(weaponsController.currentTargetSelection.Count <= 0 || weaponsController.currentTarget == -1){
                isCameraTargetLocked = false;
                return;
            }
            else if(!weaponsController.currentTargetSelection[weaponsController.currentTarget].gameObject.activeInHierarchy){
                    isCameraTargetLocked = false;
                    return;
            }
            else if(weaponsController.currentTargetSelection.Count > 0){
                target = weaponsController.currentTargetSelection[weaponsController.currentTarget].transform.position;
            }
            var toTarget = target - gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(toTarget, weaponsController.gameObject.transform.up);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, .05f);
            //RotateTowards(gameObject.transform.rotation, targetRotation, 50 * Time.deltaTime);
            gameObject.transform.localRotation = new Quaternion(
                Mathf.Clamp(gameObject.transform.localRotation.x, -1f, .3f), 
                Mathf.Clamp(gameObject.transform.localRotation.y, -.85f, .85f), 
                Mathf.Clamp(gameObject.transform.localRotation.z, -.1f, .1f), 
                Mathf.Clamp(gameObject.transform.localRotation.w, -1, 1));
        }
        if(waitingRespawn && followTarget != null){
            var toTarget = followTarget.transform.position - gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(toTarget, weaponsController.gameObject.transform.up);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, .05f);
        }
        
    }
}
}
