using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    public enum TargetType{
        TeamA,
        TeamB,
        Global,
        Objective
    }
    public TargetType targetTeam;
    public string nameOfTarget = "Targetable Object";


}
