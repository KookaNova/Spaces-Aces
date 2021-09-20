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
    [Tooltip("Abilities are ordered 0 = Passive, 1 = Primary, 2 = Secondary, and 3 = Ace")]
    public List<AbilityHandler> abilities;
}
