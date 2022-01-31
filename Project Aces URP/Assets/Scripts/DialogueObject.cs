using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Controller/Dialogue Object")]
public class DialogueObject : ScriptableObject
{
    public AudioClip voiceLine;
    [TextArea]
    public string subtitle;
}
