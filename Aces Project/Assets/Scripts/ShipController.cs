using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : PlayerController
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

    private void Awake()
    {
        thrust = minSpeed;
    }

    private void FixedUpdate()
    {
        if (Gamepad.current == null)
        {
            CheckGamepad();
        }

        _controllerBody.AddRelativeForce(0, 0, thrust);

        //accelerate
        if (_g.rightTrigger.ReadValue() > .2f && thrust < maxSpeed && !_g.leftTrigger.isPressed)
        {
            thrust = thrust + (_g.rightTrigger.ReadValue() * accelerationRate) * Time.deltaTime;
            _controllerBody.AddRelativeForce(0, 0, thrust);

            if (thrust > maxSpeed)
            {
                thrust = maxSpeed;
            }

            _g.SetMotorSpeeds(_g.rightTrigger.ReadValue() - .65f, _g.rightTrigger.ReadValue() - .35f);
        }
        else if (thrust > maxSpeed - 5)
        {
            thrust = Mathf.Lerp(thrust, maxSpeed - 5, (Time.deltaTime / 15));
            _g.SetMotorSpeeds(0, 0);
        }

        //break
        if (_g.leftTrigger.isPressed && thrust > minSpeed)
        {
            thrust = Mathf.Lerp(thrust, minSpeed, (Time.deltaTime));
            _controllerBody.velocity = Vector3.MoveTowards(_controllerBody.velocity, new Vector3(0, 0, thrust),
                Time.deltaTime * breakForce);
            _g.SetMotorSpeeds(thrust / maxSpeed * .95f, thrust / maxSpeed * .26f);
        }
        else
        {
            _g.SetMotorSpeeds(0, 0);
        }

        //twist
        if (_g.leftStick.left.ReadValue() > .1f)
        {
            _controllerBody.AddRelativeTorque(0, 0, (_g.leftStick.left.ReadValue() * 3) * twistSpeed);
        }

        if (_g.leftStick.right.ReadValue() > .1f)
        {
            _controllerBody.AddRelativeTorque(0, 0, -((_g.leftStick.right.ReadValue() * 3) * twistSpeed));
        }

        //pitch
        if (_g.leftStick.up.ReadValue() > 0)
        {
            _controllerBody.AddRelativeTorque(
                _g.leftStick.up.ReadValue() * downSpeed / (thrust / (highSpeedTurning * 15)), 0, 0);
        }

        if (_g.leftStick.down.isPressed)
        {
            _controllerBody.AddRelativeTorque(
                -(_g.leftStick.down.ReadValue() * upSpeed / (thrust / (highSpeedTurning * 15))), 0, 0);
        }

        //yaw
        if (_g.leftShoulder.isPressed)
        {
            _controllerBody.AddRelativeTorque(0, -yawSpeed, 0);
        }

        if (_g.rightShoulder.isPressed)
        {
            _controllerBody.AddRelativeTorque(0, yawSpeed, 0);
        }

    }
}