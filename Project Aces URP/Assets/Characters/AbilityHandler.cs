using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public float cooldownTime = 3f, startUpTime = .2f;

    public ParticleSystem startUpParticle, endParticle;

    public GameObject player;

}
