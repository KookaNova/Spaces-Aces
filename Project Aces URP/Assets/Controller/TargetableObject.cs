using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    public int targetTeam; // 0 = A, 1 = B, 2= global, 3 = objective
    public string nameOfTarget = "Targetable Object";
    [HideInInspector] public MeshRenderer mesh;

    private void Awake(){
        mesh = GetComponentInChildren<MeshRenderer>();
    }


}
