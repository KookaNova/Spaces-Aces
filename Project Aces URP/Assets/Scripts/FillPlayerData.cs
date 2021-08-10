using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FillPlayerData : MonoBehaviour
{
    [Header("Player Profile")]
    [SerializeField]
    private TMPro.TextMeshProUGUI nameText, levelText;
    [SerializeField]
    private Image namePlate, emblemPrimary, emblemBackground;
    [SerializeField]
    private List<Sprite> namePlates, emblems;
    [Header("Optional")]
    [SerializeField]
    private Image xpBar;
    [SerializeField]
    private TMPro.TextMeshProUGUI coinText, premiumText;

    [SerializeField]
    private Dropdown selectPrimary, selectBack, selectNameplate;
    
    int currentXp, levelUpPoint;

    public void EditingProfile(){
        selectNameplate.ClearOptions();
        selectPrimary.ClearOptions();
        selectBack.ClearOptions();

        for(int i = 0; i < namePlates.Count; i++){
            selectNameplate.options.Add(new Dropdown.OptionData());
            selectNameplate.options[i].image = namePlates[i];
        }
        for(int i = 0; i < emblems.Count; i++){
            selectPrimary.options.Add(new Dropdown.OptionData());
            selectBack.options.Add(new Dropdown.OptionData());
            selectPrimary.options[i].image = emblems[i];
            selectBack.options[i].image = emblems[i];
        }
    }

    public void DisplayData(){

        PlayerProfileData data = SaveData.LoadProfile();
        SetCustomData(data.profileName, data.currentLevel, data.nameHue, data.nameArt, data.emblemPrimary, data.emblemBackground, data.emblemPrimaryHue, data.emblemBackHue);
        if(data == null) return;

        nameText.text = data.profileName;
        levelText.text = data.currentLevel.ToString();
        namePlate.sprite = namePlates[data.nameArt];
        namePlate.color = Color.HSVToRGB(data.nameHue, .75f, .65f);

        emblemPrimary.sprite = emblems[data.emblemPrimary];
        emblemPrimary.color = Color.HSVToRGB(data.emblemPrimaryHue, .80f, .85f);
        emblemBackground.sprite = emblems[data.emblemBackground];
        emblemBackground.color = Color.HSVToRGB(data.emblemBackHue, .80f, .85f);

        if(xpBar == null)return;
        UpdateBar();

    }

    public void UpdateBar(){
        float difference = 15000 - currentXp;
        float barfill = 1 - (difference/15000);
        xpBar.fillAmount = barfill;
    }

    public void SetCustomData(string name, int level, float nameHue, int nameArt, int emblemPrim, int emblemBack, float emblemPrimHue, float emblemBackHue){
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable(){
            {"Name", name},
            {"Level", level},
            {"Name Hue", nameHue},
            {"Nameplate Art", nameArt},
            {"Emblem Primary", emblemPrim},
            {"Emblem Background", emblemBack},
            {"Emblem Primary Hue", emblemPrimHue},
            {"Emblem Background Hue", emblemBackHue}
        };

        PhotonNetwork.SetPlayerCustomProperties(properties);
    }

    public void FillOnlineData(string name, int level, float nameHue, int nameArt, int emblemPrim, int emblemBack, float emblemPrimHue, float emblemBackHue){
        Debug.Log("FillPlayerData: SetCustomData(), Player " + name + " custom data set to " + PhotonNetwork.LocalPlayer.CustomProperties);
        nameText.text = name;
        levelText.text = level.ToString();
        namePlate.color = Color.HSVToRGB(nameHue, .75f, .75f);
        namePlate.sprite = namePlates[nameArt];
        emblemPrimary.sprite = emblems[emblemPrim];
        emblemBackground.sprite = emblems[emblemBack];
        emblemPrimary.color = Color.HSVToRGB(emblemPrimHue, .80f, .85f);
        emblemBackground.color = Color.HSVToRGB(emblemBackHue, .80f, .85f);

    }
}
