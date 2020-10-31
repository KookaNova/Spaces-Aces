using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{

    public float thrust = 1,
        maxSpeed = 10,
        yawSpeed = 0.1f,
        upSpeed = 5,
        downSpeed = 3,
        breakForce = 1.1f,
        twistSpeed = 1,
        highSpeedTurning = 4;

    public Vector3 _bodyVelocity;

    private Rigidbody _controllerBody;
    private Gamepad _gamepad;
    
    
    // Start is called before the first frame update
    void Start()
    {
       _controllerBody =  GetComponent<Rigidbody>();
       CheckGamepad();
       _gamepad = Gamepad.current;
    }

    private void CheckGamepad()
    {
        if (Gamepad.current == null)
        {
            Debug.Log("no gamepad found");
            return;
        }
        else
        {
            _gamepad = Gamepad.current;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _controllerBody.AddRelativeForce(0,0, thrust);

        //accelerate
        if (_gamepad.rightTrigger.isPressed && thrust < maxSpeed)
        {
            thrust = (_gamepad.rightTrigger.ReadValue() * maxSpeed);
            _controllerBody.AddRelativeForce(0,0, thrust);
            
            _bodyVelocity = _controllerBody.velocity;
            
            _gamepad.SetMotorSpeeds(_gamepad.rightTrigger.ReadValue()-.65f, _gamepad.rightTrigger.ReadValue()-.35f);
        }
        else if(thrust > 10)
        {
            thrust = Mathf.Lerp(thrust, 10, (Time.deltaTime/4));
            _gamepad.SetMotorSpeeds(0, 0);
        }

        //decelerate
        if (_gamepad.leftTrigger.isPressed && thrust > 5)
        {
            thrust = Mathf.Lerp(thrust, 5, (Time.deltaTime));
            _controllerBody.velocity = Vector3.MoveTowards(_controllerBody.velocity, Vector3.zero, breakForce/5);
        }

        //twist
        if (_gamepad.leftStick.left.isPressed)
        {
            _controllerBody.AddRelativeTorque(0,0,_gamepad.leftStick.left.ReadValue() + twistSpeed);
        }
        if (_gamepad.leftStick.right.isPressed)
        {
            _controllerBody.AddRelativeTorque(0,0,-(_gamepad.leftStick.right.ReadValue() + twistSpeed));
        }
        //pitch
        if (_gamepad.leftStick.up.isPressed)
        {
            _controllerBody.AddRelativeTorque(_gamepad.leftStick.up.ReadValue() + (downSpeed/(thrust/highSpeedTurning)),0, 0);
        }
        if (_gamepad.leftStick.down.isPressed)
        {
            _controllerBody.AddRelativeTorque(-(_gamepad.leftStick.down.ReadValue() + (upSpeed/(thrust/highSpeedTurning))),0, 0);
        }
        //yaw
        if (_gamepad.leftShoulder.isPressed)
        {
            _controllerBody.AddRelativeTorque(0,-yawSpeed,0);
        }
        if (_gamepad.rightShoulder.isPressed)
        {
            _controllerBody.AddRelativeTorque(0,yawSpeed,0);
        }


    }
}
