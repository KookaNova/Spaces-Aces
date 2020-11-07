using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackControls : PlayerController
{
    public float missileReload = 3, missileSpeed = 10, lockOnEfficiency = 1, fireRate;
   
    public GameObject gunAmmo, missile, lockOnSprite;
    public Transform gunLocation, launcherR, launcherL;
    
    private GameObject _queueTarget = null, _missileTarget = null;
    private bool _canFire = true, _canLaunchR = true, _canLaunchL = true, _isLocked = false;

    public List<GameObject> _targetOptions;

    private void Update()
    {
        if (Gamepad.current == null)
        {
            CheckGamepad();
        }
        //gun and missiles
        if (_g.buttonSouth.isPressed && _canFire)
        {
            StartCoroutine(FireGun());
        }
        if (_g.buttonEast.wasPressedThisFrame)
        {
            if (_canLaunchR)
            {
                StartCoroutine(FireMissileR());
            }
            else if (_canLaunchL)
            {
                StartCoroutine(FireMissileL());
            }
        }

        if (_g.buttonNorth.wasPressedThisFrame && _targetOptions.Count > 1)
        {
            var i = _targetOptions[0].gameObject;
            _targetOptions.Remove(_targetOptions[0]);
            _missileTarget = _targetOptions[0];
            _targetOptions.Add(i.gameObject);
        }

        if (_isLocked)
        {
            if (_missileTarget == null)
            {
                _isLocked = true;
            }
            RaycastHit hit;

            Vector3 p1 = gameObject.transform.position;
            Vector3 dir = _missileTarget.transform.position;
            
            lockOnSprite.gameObject.SetActive(true);
            lockOnSprite.transform.position = dir;
            
            Debug.DrawLine(p1 ,dir, Color.red);
            if (Physics.SphereCast(p1, 50, dir , out hit, 20))
            {

            }
            else
            {
                _isLocked = false;
                _missileTarget = null;
                lockOnSprite.gameObject.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter(Collider obj)
    {
        print("target Enter");
        _targetOptions.Add(obj.gameObject);
        StartCoroutine(LockOn());
    }
    private void OnTriggerExit(Collider obj)
    {
        print("target lost");
        _targetOptions.Remove(obj.gameObject);
        StartCoroutine(LockOn());
    }


    private IEnumerator LockOn()
    {
        
        yield return new WaitForSeconds(lockOnEfficiency);
        if (_targetOptions.Count <= 0) yield break;
        _isLocked = true;
        _missileTarget = _targetOptions[0];
        _queueTarget = null;
        print("target ID");

    }
    private IEnumerator FireGun()
    {
        _canFire = false;
        var g = Instantiate(gunAmmo, gunLocation.position, launcherL.localRotation);
        g.GetComponent<Rigidbody>().velocity = gunLocation.forward * (80 + 500);
        yield return new WaitForSeconds(fireRate);
        _canFire = true;
    }

    private IEnumerator FireMissileR()
    {
        _canLaunchR = false;
        var r = Instantiate(missile, launcherR.position, launcherL.localRotation);
        if (_isLocked)
        {
            r.transform.LookAt(_missileTarget.transform);
            r.GetComponent<Rigidbody>().AddRelativeForce(0,0,80 + missileSpeed);
        }
        else
        {
            r.GetComponent<Rigidbody>().velocity = launcherR.forward * (80 + missileSpeed);
        }
        yield return new WaitForSeconds(missileReload);
        _canLaunchR = true;
    }
    private IEnumerator FireMissileL()
    {
        _canLaunchL = false;
        var l = Instantiate(missile, launcherL.position, launcherL.localRotation);
        if (_isLocked)
        {
            l.transform.LookAt(_missileTarget.transform);
            l.GetComponent<Rigidbody>().AddRelativeForce(0,0, 80 + missileSpeed);
        }
        else
        {
            l.GetComponent<Rigidbody>().velocity = launcherR.forward * (80 + missileSpeed);
        }
        yield return new WaitForSeconds(missileReload);
        _canLaunchL = true;
    }
    
}
