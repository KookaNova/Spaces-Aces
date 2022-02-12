using UnityEngine;

[CreateAssetMenu(menuName = "Controller/Dialogue Object")]
public class DialogueObject : ScriptableObject
{
    public AudioClip audio;
    [TextArea]
    public string subtitle;

    //NOTE: We could add a list and randomize audio as well. This would prevent lines from becoming too annoying.
}
