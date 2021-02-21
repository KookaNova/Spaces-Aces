using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AircraftController : MonoBehaviour
{
    [Header("Aircraft Input Data")]
    public AircraftData aircraftInputData;

    [Header("Aircraft Stats")] 
    public FloatData currentSpeed;

    public float acceleration = 10,
        minSpeed = 10,
        cruiseSpeed = 100,
        maxSpeed = 200,
        roll = 5, 
        pitch = 7, 
        yaw = 1;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ThrustControl();
        TorqueControl();

        _rb.AddRelativeForce(0,0,currentSpeed.value);

        if (gameObject.transform.localRotation.x < -60 && gameObject.transform.localRotation.x > -135)
        {
            print("losing speed");
            currentSpeed.value = Mathf.Lerp(currentSpeed.value, minSpeed, .1f);
        }
        
        if (currentSpeed.value > cruiseSpeed)
        {
            currentSpeed.value = Mathf.Lerp(currentSpeed.value, cruiseSpeed, .001f);
        }
        
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, minSpeed, aircraftInputData.brakeInput * .001f);
    }

    private void OnCollisionEnter(Collision other)
    {
        currentSpeed.value = currentSpeed.value / 4;
    }

    private void ThrustControl()
    {
        var speed = currentSpeed.value + (aircraftInputData.thrustInput * acceleration);
        currentSpeed.value = Mathf.Lerp(currentSpeed.value, speed, .005f);
        currentSpeed.value = Mathf.Clamp(currentSpeed.value, minSpeed, maxSpeed);

    }
    
    private void TorqueControl()
    {
        Vector3 torque  = new Vector3(aircraftInputData.torqueInput.y * pitch, aircraftInputData.yawInput * yaw, aircraftInputData.torqueInput.x * roll);
        _rb.AddRelativeTorque(torque);
    }
}
