using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Games/GamesHandler")]
public class GamesHandler : ScriptableObject
{

    public string matchmakingType = "Quickplay or name a mode";
    public byte expectedMaxPlayers = 6;
    public GamemodeData gamemodeSettings; //current level selection from the playlist
    [SerializeField] List<GamemodeData> levelOptions; //playlist of possible levels


    private int selectionIndex; //current level selection in the code

    public void SelectRandomLevel(){
        selectionIndex = Random.Range(0, levelOptions.Count);

        gamemodeSettings = levelOptions[selectionIndex];
    }
    public void SelectNextLevel(){
        selectionIndex ++;
        if(selectionIndex > levelOptions.Count - 1)
        selectionIndex = 0;

        gamemodeSettings = levelOptions[selectionIndex];
    }
}
