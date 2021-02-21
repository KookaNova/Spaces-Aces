using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public float x, y, z;

    private Rigidbody _r;

    private void Start()
    {
        _r = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _r.AddRelativeForce(x,y,z);
    }
}
