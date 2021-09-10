using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLock : MonoBehaviour
{
    [SerializeField]
    bool lockPositionInWorld = true, lockRotationInWorld = true;
    [SerializeField]
    Transform lockPosition;

    Vector3 startingPosition;
    Quaternion startingRotation;

    private void OnEnable() {
        startingPosition = gameObject.transform.position;
        startingRotation = gameObject.transform.rotation;
    }

    void Update()
    {
        if(lockPositionInWorld){
            gameObject.transform.position = startingPosition;
        }
        else{
            gameObject.transform.position = lockPosition.position;
        }

        if(lockRotationInWorld){
            gameObject.transform.rotation = startingRotation;
        }
        else{
            gameObject.transform.rotation = lockPosition.rotation;
        }
    }
}
