using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Controller/Character")]
public class CharacterHandler : ScriptableObject
{
    public string characterName = "Character Name";
    [TextArea( 2, 50 )]
    public string bio = "Character bio...";
    public Texture2D portrait, bodyArt, nameArt;
    [Tooltip("Abilities are ordered:\n0 = Primary\n1 = Secondary\n2 = Ace")]
    public List<AbilityHandler> abilities;
    [Tooltip("0 = Start Match\n1 = Primary\n2 = Secondary\n3 = Ace\n4 = EnemyBeingTargeted\n5 = MissileTypeFired\n6 = ThisBeingTargeted\n7 = MissileIncoming\n8 = EnemyEliminated\n9 = ShieldsDown\n10 = LowHealth\n11 = SelfEliminated\n12 = Respawn")]
    public List<DialogueObject> voiceLines;

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
