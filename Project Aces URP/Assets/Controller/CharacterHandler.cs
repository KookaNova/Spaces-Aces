using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterHandler : ScriptableObject
{
    public string characterName = "Character Name";
    public Sprite avatar;
    [Tooltip("Abilities are ordered 0 = Passive, 1 = Primary, 2 = Secondary, and 3 = Ace")]
    public List<AbilityHandler> abilities;
}
