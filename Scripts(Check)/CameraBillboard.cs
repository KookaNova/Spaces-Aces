using Cinemachine;
using UnityEngine;
 
public class CameraBillboard : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;

    void LateUpdate()
    {
        transform.LookAt(transform.position + currentCamera.transform.rotation * Vector3.forward,
            currentCamera.transform.rotation * Vector3.up);
    }
    
    
}