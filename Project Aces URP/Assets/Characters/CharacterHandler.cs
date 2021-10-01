using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Controller/Character")]
public class CharacterHandler : ScriptableObject
{
    public string characterName = "Character Name";
    [TextArea( 2, 50 )]
    public string bio = "Character bio...";
    public Sprite portrait, bodyArt, nameArt;
    [Tooltip("Abilities are ordered 0 = Primary, 1 = Secondary, and 2 = Ace")]
    public List<AbilityHandler> abilities;

    //Base passive modifiers. Additional modifiers are added through the passive ability
    [Header("Passive Modifiers")]
    public float health = 0;
    public float shield = 0,
        shieldRechargeRate = 0,
        acceleration = 0,
        minSpeed = 0,
        maxSpeed = 0,
        roll = 0, 
        pitch = 0, 
        yaw = 0,
        gunDamage = 0,
        missileDamage = 0,
        missileReload = 0,
        lockOnEfficiency = 0;
        
}
