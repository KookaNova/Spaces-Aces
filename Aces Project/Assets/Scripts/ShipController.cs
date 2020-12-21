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
        if (_g.rightTrigger.ReadValue() > .2f && thrust < maxSpeed && !_g.leftTrigger.isPressed || _k.spaceKey.isPressed)
        {
            thrust = thrust + (_g.rightTrigger.ReadValue() * accelerationRate) * Time.deltaTime;
            _controllerBody.AddRelativeForce(0, 0, thrust);

            if (thrust > maxSpeed)
            {
                thrust = maxSpeed;
            }

            _g.SetMotorSpeeds(_g.rightTrigger.ReadValue() - .65f, _g.rightTrigger.ReadValue() - .35f);
            foreach (var t in ThrustTrail)
            {
                t.gameObject.SetActive(true);
            }
        }
        else if (thrust > maxSpeed - 5)
        {
            thrust = Mathf.Lerp(thrust, maxSpeed - 5, (Time.deltaTime / 15));
            _g.SetMotorSpeeds(0, 0);
            foreach (var t in ThrustTrail)
            {
                t.gameObject.SetActive(false);
            }
        }

        //break
        if (_g.leftTrigger.isPressed && thrust > minSpeed || _k.leftCtrlKey.isPressed)
        {
            thrust = Mathf.Lerp(thrust, minSpeed, Time.deltaTime);
            _controllerBody.velocity = Vector3.MoveTowards(_controllerBody.velocity, new Vector3(0, 0, thrust),
                Time.deltaTime * breakForce);
            _g.SetMotorSpeeds(thrust / maxSpeed * .95f, thrust / maxSpeed * .26f);
        }
        else
        {
            _g.SetMotorSpeeds(0, 0);
        }
        
        //twist
        if (_g.leftStick.left.ReadValue() > .1f || _k.aKey.isPressed)
        {
            _controllerBody.AddRelativeTorque(0, 0, (_g.leftStick.left.ReadValue() * 3) + 1 * twistSpeed);
            _controllerBody.AddRelativeTorque(0, 0, (_k.aKey.ReadValue() * 3) + 1 * twistSpeed);
        }

        if (_g.leftStick.right.ReadValue() > .1f || _k.dKey.isPressed)
        {
            _controllerBody.AddRelativeTorque(0, 0, -((_g.leftStick.right.ReadValue() * 3) + 1 * twistSpeed));
            _controllerBody.AddRelativeTorque(0, 0, -((_k.dKey.ReadValue() * 3) + 1 * twistSpeed));
        }

        //pitch
        if (_g.leftStick.up.ReadValue() > 0 || _k.wKey.isPressed)
        {
            _controllerBody.AddRelativeTorque(
                _g.leftStick.up.ReadValue() * downSpeed / (thrust / (highSpeedTurning * 15)), 0, 0);
            _controllerBody.AddRelativeTorque(
                _k.wKey.ReadValue() * downSpeed / (thrust / (highSpeedTurning * 10)), 0, 0);
        }

        if (_g.leftStick.down.ReadValue() > 0 || _k.sKey.isPressed)
        {
            _controllerBody.AddRelativeTorque(
                -(_g.leftStick.down.ReadValue() * upSpeed / (thrust / (highSpeedTurning * 15))), 0, 0);
            _controllerBody.AddRelativeTorque(
                -(_k.sKey.ReadValue() * upSpeed / (thrust / (highSpeedTurning * 10))), 0, 0);
            foreach (var t in Contrails)
            {
                t.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var t in Contrails)
            {
                t.gameObject.SetActive(false);
            }
        }

        //yaw
        if (_g.leftShoulder.isPressed || _k.qKey.isPressed)
        {
            _controllerBody.AddRelativeTorque(0, -yawSpeed, 0);
        }

        if (_g.rightShoulder.isPressed || _k.eKey.isPressed)
        {
            _controllerBody.AddRelativeTorque(0, yawSpeed, 0);
        }

    }
}