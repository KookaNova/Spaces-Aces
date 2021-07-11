using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterHandler : ScriptableObject
{
    public string characterName = "Character Name";
    public Sprite avatar;
    public List<AbilityHandler> abilities;
}
