using UnityEngine;

[CreateAssetMenu(menuName = "Games/Gamemode")]
public class GamemodeData : ScriptableObject
{
    public string levelName = "Name of Scene";
    public string gamemodeName = "Name of Gamemode";
    public bool EliminationPoints = false;
    public bool isTeams = true;
    public int playerCount;
    public int AIPlayers;
    public int scoreValue;
    public int maxScore;
    public int timeLimit;
    
}
