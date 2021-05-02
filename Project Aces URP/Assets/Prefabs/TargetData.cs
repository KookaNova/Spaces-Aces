using System.Collections.Generic;
using UnityEngine;

public class TargetData : MonoBehaviour
{
    public List<GameObject> global;

    public void AddGlobalTargets(GameObject obj){
        global.Add(obj);
    }
}
