using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : PlayerController
{
    public CinemachineVirtualCamera playerCamera;

    private void LateUpdate()
    {
        if (_g.rightStick.y.ReadValue() > 0.1f || _g.rightStick.y.ReadValue() < -0.1f || _g.rightStick.x.ReadValue() > 0.1f || _g.rightStick.x.ReadValue() < -0.1f)
                 {
            playerCamera.Follow.localRotation =
                Quaternion.Euler(80 * _g.rightStick.y.ReadValue(), 80 * _g.rightStick.x.ReadValue(), 0);
        }
        else
        {
            playerCamera.Follow.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
