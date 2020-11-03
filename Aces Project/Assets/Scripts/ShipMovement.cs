using System.Security.AccessControl;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{

    public float thrust = 1,
        minSpeed = 5,
        maxSpeed = 50,
        yawSpeed = 0.1f,
        upSpeed = 5,
        downSpeed = 3,
        breakForce = 1.1f,
        twistSpeed = 1,
        highSpeedTurning = 6,
        accelerationRate = 5;

    public CinemachineVirtualCamera camera;

    private Vector3 _bodyVelocity;

    private Rigidbody _controllerBody;
    private Gamepad _g;
    
    void Start()
    {
       _controllerBody =  GetComponent<Rigidbody>();
       CheckGamepad();
       _g = Gamepad.current;
    }

    private void CheckGamepad()
    {
        
        if (Gamepad.current == null)
        {
            Debug.Log("no gamepad found");
            return;
        }
        
        _g = Gamepad.current;
        
    }

    private void FixedUpdate()
    {
        if (Gamepad.current == null)
        {
            Debug.Log("no gamepad found");
            CheckGamepad();
        }
        _controllerBody.AddRelativeForce(0,0, thrust);

        //accelerate
        if (_g.rightTrigger.ReadValue() > .2f && thrust < maxSpeed && !_g.leftTrigger.isPressed)
        {
            thrust = thrust + (_g.rightTrigger.ReadValue() * accelerationRate) * Time.deltaTime;
            _controllerBody.AddRelativeForce(0,0, thrust);
            
            _bodyVelocity = _controllerBody.velocity;
            if (thrust > maxSpeed)
            {
                thrust = maxSpeed;
            }
            
            _g.SetMotorSpeeds(_g.rightTrigger.ReadValue()-.65f, _g.rightTrigger.ReadValue()-.35f);
        }
        else if (thrust > maxSpeed - 5)
        {
            thrust = Mathf.Lerp(thrust, maxSpeed - 5, (Time.deltaTime/15));
            _g.SetMotorSpeeds(0, 0);
        }

        //decelerate
        if (_g.leftTrigger.isPressed && thrust > minSpeed)
        {
            thrust = Mathf.Lerp(thrust, minSpeed, (Time.deltaTime/3));
            _controllerBody.velocity = Vector3.MoveTowards(_controllerBody.velocity, Vector3.zero, breakForce/3);
            _g.SetMotorSpeeds( thrust/maxSpeed * .95f, thrust/maxSpeed *  .26f);
        }
        else
        {
            _g.SetMotorSpeeds(0, 0);
        }

        //twist
        if (_g.leftStick.left.ReadValue() > .1f)
        {
            _controllerBody.AddRelativeTorque(0,0,(_g.leftStick.left.ReadValue()*3) * (twistSpeed/2));
        }
        if (_g.leftStick.right.ReadValue() > .1f)
        {
            _controllerBody.AddRelativeTorque(0,0,-((_g.leftStick.right.ReadValue()*3) * (twistSpeed/2)));
        }
        //pitch
        if (_g.leftStick.up.isPressed)
        {
            _controllerBody.AddRelativeTorque(_g.leftStick.up.ReadValue() + (downSpeed/(thrust/highSpeedTurning)),0, 0);
        }
        if (_g.leftStick.down.isPressed)
        {
            _controllerBody.AddRelativeTorque(-(_g.leftStick.down.ReadValue() + (upSpeed/(thrust/highSpeedTurning))),0, 0);
        }
        //yaw
        if (_g.leftShoulder.isPressed)
        {
            _controllerBody.AddRelativeTorque(0,-yawSpeed,0);
        }
        if (_g.rightShoulder.isPressed)
        {
            _controllerBody.AddRelativeTorque(0,yawSpeed,0);
        }
        //camera control
        if (_g.rightStick.y.IsPressed() || _g.rightStick.x.IsPressed())
        {
            camera.Follow.localRotation = Quaternion.Euler(80 * _g.rightStick.y.ReadValue(), 80 * _g.rightStick.x.ReadValue(), 0);
        }
        else
        {
            camera.Follow.localRotation = Quaternion.Euler(Vector3.zero);
        }




    }
}
