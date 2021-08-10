using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ProfileHandler : MonoBehaviour
{
    public string profileName;
    public float  nameHue, emblemPrimaryHue, emblemBackHue;
    public int nameArt, frameArt, emblemPrimary, emblemBackground;

    public int currentXp, currentLevel, levelUpPoint;

    private void Start() {
        PlayerProfileData data = SaveData.LoadProfile();
        if(data == null){
            Debug.LogWarning("PlayerProfile: Start(), data is null, create save data");
            return;
        }
        levelUpPoint = 15000;

        profileName = data.profileName;
        PhotonNetwork.NickName = profileName;
        nameHue = data.nameHue;
        frameArt = data.frameArt;

        currentXp = data.currentXp;
        currentLevel = data.currentLevel;

        
    }

    #region Change Data Functions
    public void AddExp(int experience){
        currentXp += experience;
        Debug.Log("ProfileHandler: AddExp(), add " + experience + " to profile " + profileName);
        if(currentXp >= levelUpPoint){
            currentXp = currentXp - levelUpPoint;
            LevelUp();
        }
        SaveInfo();
    }

    public void ChangeName(string newName){
        profileName = newName;
        PhotonNetwork.NickName = profileName;
    }

    public void ChangeNameColor(float hue){
        nameHue = hue;
    }

    public void ChangeNameArt(int artChoice){
        nameArt = artChoice;
    }

    public void ChangeEmblemPrimary(int choice){
        emblemPrimary = choice;
    }
    public void ChangeEmblemBack(int choice){
        emblemBackground = choice;
    }

    public void ChangeEmblemPrimaryColor(float hue){
        emblemPrimaryHue = hue;
    }

    public void ChangeEmblemBackgroundColor(float hue){
        emblemBackHue = hue;
    }

    public void ChangeAvatarFrames(int choice){
        frameArt = choice;
    }

    public void LevelUp(){
        currentLevel++;
        Debug.Log("ProfileHandler: profile " + profileName + " leveled up to " + currentLevel);
        SaveInfo();
    }

    #endregion

    public void SaveInfo(){
        SaveData.SaveProfile(this);
    }

    public void LoadInfo(){
        SaveData.LoadProfile();
    }

    public void InitializeData(){
        profileName = "New Profile";
        nameHue = 0; 
        emblemPrimaryHue = 0;
        emblemBackHue = 0;
        nameArt = 0;
        emblemPrimary = 0; 
        emblemBackground = 0;
        currentXp = 0;
        currentLevel = 0;
        SaveInfo();

    }
}
