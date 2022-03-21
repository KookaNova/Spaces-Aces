using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[System.Serializable]
public class simple_vfx_data 
{
    //
    // events
    public UnityEvent onDestroy;

    public Action<bool> onValueChanged;
    
    
    
    public VisualEffect effect;

    public DestroyObject destroy;
    
    public GameObject originalObj;

    public GameObject shatteredObj;

    private bool _destroyed;

    public bool destroyed
    {
        get
        {
            return _destroyed;
        }
        set
        {
            if(destroyed == value) return;
            
            _destroyed = value;
            
            //onValueChanged?.Invoke(_destroyed);
        }
    }
    
    // methods
    public void DestroyAction()
    {
        
        destroyed = true;


        onDestroy?.Invoke();
        destroy.ManualBreak();
        effect.SendEvent("start");
        //onValueChanged -= setActiveObjects;
    }

}
