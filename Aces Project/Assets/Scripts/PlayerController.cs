using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected Rigidbody _controllerBody;
    protected Gamepad _g;

    protected void Start()
    {
        _controllerBody = GetComponent<Rigidbody>();
        StartCoroutine(CheckGamepad());
    }
    
    protected IEnumerator CheckGamepad()
    {
        if(Gamepad.current!=null) _g = Gamepad.current;
        if (Gamepad.current == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(CheckGamepad());
        }
    }
}
