using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillPlayerData : MonoBehaviour
{
    [Header("Player Profile")]
    [SerializeField]
    private PlayerProfileData data;
    [SerializeField]
    private Image namePlate, icon;
    [SerializeField]
    private TMPro.TextMeshProUGUI nameText, levelText;
    [Header("Optional")]
    [SerializeField]
    private Image xpBar;
    [SerializeField]
    private TMPro.TextMeshProUGUI coinText, premiumText;
    
    int currentXp, levelUpPoint;

    private void Awake() {
        UpdateInfo();
    }
    private void OnEnable() {
        UpdateInfo();
    }

    public void UpdateInfo(){
        nameText.text = data.accountName;
        namePlate.sprite = data.namePlate;
        namePlate.color = data.namePlateColor;
        icon.sprite = data.icon;
        levelText.text = data.currentLevel.ToString();

        if(coinText == null)return;
        coinText.text = data.coin.ToString();
        premiumText.text = data.premium.ToString();
        currentXp = data.currentXp;
        levelUpPoint = data.levelUpPoint;
        UpdateBar();
    }

    public void UpdateBar(){
        float difference = levelUpPoint - currentXp;
        float barfill = difference/levelUpPoint;
        xpBar.fillAmount = barfill;
    }
}
