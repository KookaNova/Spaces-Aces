using UnityEngine;

[CreateAssetMenu(menuName = "Games/Gamemode")]
public class GamemodeData : ScriptableObject
{
    public string levelName = "Name of Scene";
    public string gamemodeName = "Name of Gamemode";
    public bool EliminationPoints = false;
    public bool isTeams = true;
    public byte playerCount;
    public byte AIPlayers;
    public byte scoreValue;
    public byte maxScore;
    public byte timeLimit;
    
}
