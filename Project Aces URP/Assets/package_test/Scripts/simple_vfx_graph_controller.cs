using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class simple_vfx_graph_controller : MonoBehaviour
{
    /*
     Events notes
     1-define events
     2-listeners subscribe to events
     3-define how the event gets fired
     TODO: don't use player input eventually
     */

    public UnityEvent onDestroyed;
    [SerializeField]
    private simple_vfx_data _effect;

    //private PlayerInput _inputs;
    // private void OnEnable()
    // {
    //     _effect.onValueChanged += toggleActive;
    // }
    //
    // private void OnDisable()
    // {
    //     _effect.onValueChanged -= toggleActive;
    // }

    public void Explode(InputAction.CallbackContext context)
    {
        //_effect.destroyed = true;
        onDestroyed?.Invoke();
        
        Debug.Log(context);
        //_effect.effect.Play();
        Debug.Log("space pressed");
        _effect.DestroyAction();
    }

    // private void toggleActive(bool b)
    // {
    //     if (b == true)
    //     {
    //         _effect.originalObj.SetActive(false);
    //         _effect.shatteredObj.SetActive(true);
    //     }
    //     else
    //     {
    //         _effect.originalObj.SetActive(true);
    //         _effect.shatteredObj.SetActive(false);
    //     }
    // }



}
