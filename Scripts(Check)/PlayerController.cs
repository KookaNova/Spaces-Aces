using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected Rigidbody _controllerBody;
    protected Gamepad _g;
    protected Keyboard _k;
    protected Mouse _m;
    public List<TrailRenderer> ThrustTrail, Contrails;

    protected void Start()
    {
        _k = Keyboard.current;
        _m = Mouse.current;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        _controllerBody = GetComponent<Rigidbody>();
        StartCoroutine(CheckGamepad());
    }
    
    protected IEnumerator CheckGamepad()
    {
        if (Gamepad.current != null)
        {
            _g = Gamepad.current;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(CheckGamepad());
        }
    }
}
