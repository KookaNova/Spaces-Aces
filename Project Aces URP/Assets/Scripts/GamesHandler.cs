using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Games/GamesHandler")]
public class GamesHandler : ScriptableObject
{
    public GamemodeData gamemodeSettings;
    [SerializeField]
    private List<GamemodeData> levelOptions;

    public void SelectRandomLevel(){
        int select = Random.Range(0, levelOptions.Count);

        gamemodeSettings = levelOptions[select];
    }
}
