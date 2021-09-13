using UnityEngine;

public class CameraController : MonoBehaviour
{
    public WeaponsController weaponsController;
    private Vector2 rotationInput;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera[] cameras;
    [HideInInspector]
    public int currentCamera = 0;
    private bool isCameraTargetLocked = false;
    private Vector3 target;

    public void Activate(){
        for(int i = 0; i < cameras.Length; i++){
            cameras[i].gameObject.SetActive(false);
        }
        cameras[0].gameObject.SetActive(true);
    }

    public void RotateCamera(Vector2 input){

        rotationInput.y = input.y * 70;
        rotationInput.x = input.x * 120;
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
        if(!isCameraTargetLocked){
            isCameraTargetLocked = true;
        }
        else{
            isCameraTargetLocked = false;
        }
        if(weaponsController.currentTargetSelection.Count <= 0){
                isCameraTargetLocked = false;
                return;
            }
            else{
                if(weaponsController.currentTargetSelection[0] == null){
                    isCameraTargetLocked = false;
                    return;
                }
            }
    }

    private void LateUpdate(){
        if(rotationInput == Vector2.zero){
            gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, Quaternion.identity, .05f);
        }
        if(weaponsController.currentTargetSelection.Count > 0){
            target = weaponsController.currentTargetSelection[0].transform.position;
        }
        if(gameObject.transform.localRotation.x >= 20 || gameObject.transform.localRotation.x <= -90 || gameObject.transform.localRotation.y >= 120 || gameObject.transform.localRotation.y <= -120){
                CameraLockTarget();
            }

        //if camera isn't locked, use controller input
        if(!isCameraTargetLocked){
            var targetRotation = Quaternion.Euler(-rotationInput.y, rotationInput.x, 0);
            gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, targetRotation, .05f);

            gameObject.transform.localRotation = new Quaternion(
                Mathf.Clamp(gameObject.transform.localRotation.x, -1f, .3f), 
                Mathf.Clamp(gameObject.transform.localRotation.y, -.9f, .9f), 
                Mathf.Clamp(gameObject.transform.localRotation.z, -.1f, .1f), 
                Mathf.Clamp(gameObject.transform.localRotation.w, -1, 1));
        }
        else{
            if(weaponsController.currentTargetSelection.Count <= 0){
                isCameraTargetLocked = false;
                return;
            }
            else{
                if(!weaponsController.currentTargetSelection[0].gameObject.activeSelf){
                    isCameraTargetLocked = false;
                    return;
                }
            }
            var toTarget = target - gameObject.transform.position;
            var targetRotation = Quaternion.LookRotation(toTarget, weaponsController.gameObject.transform.up);

            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, .05f);
            //RotateTowards(gameObject.transform.rotation, targetRotation, 50 * Time.deltaTime);

            gameObject.transform.localRotation = new Quaternion(
                Mathf.Clamp(gameObject.transform.localRotation.x, -1f, .3f), 
                Mathf.Clamp(gameObject.transform.localRotation.y, -.9f, .9f), 
                Mathf.Clamp(gameObject.transform.localRotation.z, -.1f, .1f), 
                Mathf.Clamp(gameObject.transform.localRotation.w, -1, 1));

        }
    }
}
