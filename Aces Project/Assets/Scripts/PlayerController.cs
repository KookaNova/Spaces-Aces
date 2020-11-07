using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected Rigidbody _controllerBody;
    protected Gamepad _g;

    protected void Start()
    {
        _controllerBody = GetComponent<Rigidbody>();
        CheckGamepad();
    }
    
    protected void CheckGamepad()
    {
        _g = Gamepad.current;
    }
}
