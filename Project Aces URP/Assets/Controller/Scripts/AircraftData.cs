using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aircraft Data", menuName = "Aircraft Controls")]
public class AircraftData : ScriptableObject
{
    [Header("Flight Inputs")]
    public Vector2 torqueInput;
    public float thrustInput, yawInput, brakeInput;
    
    [Header("Camera")] 
    public Vector2 cameraInput;
    
    
    public void UpdateInputData
        (Vector2 _cameraInput, Vector2 _torqueInput, float _thrustInput, float _yawInput, float _brakeInput)
    {
        cameraInput = _cameraInput;
        torqueInput = _torqueInput;

        thrustInput = _thrustInput;
        yawInput = _yawInput;
        brakeInput = _brakeInput;
    }
}
