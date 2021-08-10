using UnityEngine;

[System.Serializable]
public class PlayerProfileData
{
    public string profileName;
    public float  nameHue, emblemPrimaryHue, emblemBackHue;
    public int nameArt, frameArt, emblemPrimary, emblemBackground;

    public int currentXp, currentLevel, levelUpPoint;

    public PlayerProfileData(ProfileHandler profile){
        profileName = profile.profileName;
        nameHue = profile.nameHue;

        currentXp = profile.currentXp;
        currentLevel = profile.currentLevel;
        levelUpPoint = profile.levelUpPoint;
        nameArt = profile.nameArt;
        frameArt = profile.frameArt;
        emblemPrimary = profile.emblemPrimary;
        emblemBackground = profile.emblemBackground;
        emblemPrimaryHue = profile.emblemPrimaryHue;
        emblemBackHue = profile.emblemBackHue;
    }
}
