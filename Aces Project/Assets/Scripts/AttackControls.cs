using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackControls : PlayerController
{
    public float missileReload = 3, missileSpeed = 10, lockOnEfficiency = 1, fireRate;
   
    public GameObject gunAmmo, missile, lockOnSprite;
    public Transform gunLocation, launcherR, launcherL;
    
    private GameObject _missileTarget = null;
    private bool _canFire = true, _canLaunchR = true, _canLaunchL = true, _isLocked = false;

    public List<GameObject> _targetOptions;

    private void Update()
    {
        if (Gamepad.current == null){StartCoroutine(CheckGamepad());}
        //gun and missiles
        if (_g.buttonSouth.isPressed && _canFire || _m.leftButton.isPressed && _canFire )
        {
            StartCoroutine(FireGun());
        }
        if (_g.buttonEast.wasPressedThisFrame || _m.rightButton.wasPressedThisFrame)
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

        if (_g.buttonNorth.wasPressedThisFrame && _targetOptions.Count > 1 || _m.scroll.IsPressed() && _targetOptions.Count > 1)
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
                
                lockOnSprite.gameObject.SetActive(false);
                _isLocked = false;
                return;
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

            if (_targetOptions.Count == 0)
            {
                _missileTarget = null;
            }
        }
        
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (_targetOptions.Contains(obj.gameObject)) return;
        _targetOptions.Add(obj.gameObject);
        StartCoroutine(LockOn());
    }
    private void OnTriggerExit(Collider obj)
    {
        _targetOptions.Remove(obj.gameObject);
        StartCoroutine(LockOn());
    }


    private IEnumerator LockOn()
    {
        if (_targetOptions.Count <= 0) yield break;
        yield return new WaitForSeconds(lockOnEfficiency);
        if (_targetOptions.Count <= 0) yield break;
        _isLocked = true;
        _missileTarget = _targetOptions[0];

    }
    private IEnumerator FireGun()
    {
        _canFire = false;
        var g = Instantiate(gunAmmo, gunLocation.position, gunLocation.rotation);
        g.GetComponent<Rigidbody>().velocity = gunLocation.forward * 700;
        yield return new WaitForSeconds(fireRate);
        _canFire = true;
    }

    private IEnumerator FireMissileR()
    {
        _canLaunchR = false;
        var r = Instantiate(missile, launcherR.position, launcherR.rotation);
        if (_missileTarget != null)
        {
            r.gameObject.GetComponent<MissileBehaviour>().target = _missileTarget.transform;
        }

        yield return new WaitForSeconds(missileReload);
        _canLaunchR = true;
    }
    private IEnumerator FireMissileL()
    {
        _canLaunchL = false;
        var l = Instantiate(missile, launcherL.position, launcherL.rotation);
        if (_missileTarget != null)
        {
            l.gameObject.GetComponent<MissileBehaviour>().target = _missileTarget.transform;
        }

        yield return new WaitForSeconds(missileReload);
        _canLaunchL = true;
    }
    
}
