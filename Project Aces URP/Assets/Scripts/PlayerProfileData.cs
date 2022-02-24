using UnityEngine;

[System.Serializable]
public class PlayerProfileData
{
    public string profileName;
    public int totalGames, totalWins, totalScore, totalKills, totalDeaths, totalSeconds;
    public float  nameHue, emblemPrimaryHue, emblemBackHue;
    public int nameArt, frameArt, emblemPrimary, emblemBackground;

    public int currentXp, currentLevel, levelUpPoint;

    public PlayerProfileData(ProfileHandler profile){
        profileName = profile.profileName;
        //nameHue = profile.nameHue;
        totalGames = profile.totalGames;
        totalWins = profile.totalWins;
        totalScore = profile.totalScore;
        totalKills = profile.totalKills;
        totalDeaths = profile.totalDeaths;
        totalSeconds = profile.totalSeconds;

        currentXp = profile.currentXp;
        currentLevel = profile.currentLevel;
        levelUpPoint = profile.levelUpPoint;
        /*nameArt = profile.nameArt;
        frameArt = profile.frameArt;
        emblemPrimary = profile.emblemPrimary;
        emblemBackground = profile.emblemBackground;
        emblemPrimaryHue = profile.emblemPrimaryHue;
        emblemBackHue = profile.emblemBackHue;*/
    }
    
}
