using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    public Image avatarImage;
    public List<Image> speedBar, thrustBar, healthBar;
    public List<Text> speedText, healthText;

    [HideInInspector]
    public SpacecraftController currentCraft;
    [SerializeField]
    private GameObject firstPersonHUD, thirdPersonHUD, OverlayHUD;
    private CharacterHandler chosenCharacter;
    private ShipHandler chosenShip;
    private float currentSpeed, maxSpeed, thrustInput, currentHealth, maxHealth;

    public void Activate(){
        if(currentCraft == null){
            Debug.LogError("PlayerHUDController: Activate(), currentCraft is null, script can't function");
            return;
        }
        firstPersonHUD.SetActive(true); OverlayHUD.SetActive(true);
        chosenCharacter = currentCraft.chosenCharacter;
        chosenShip = currentCraft.chosenShip;

        if(chosenCharacter == null){
            Debug.LogWarning("PlayerHUDController: OnEnable(), currentCraft is null, can't fill Spacecraft info");
            return;
        }
        if(chosenShip == null){
            Debug.LogWarning("PlayerHUDController: OnEnable(), chosenShip is null, can't fill character info");
            return;
        }
        
        maxSpeed = chosenShip.maxSpeed;
        maxHealth = chosenShip.maxHealth;

        avatarImage.sprite = chosenCharacter.portrait;
    }
    public void FirstPersonHudSetInactive(){
        firstPersonHUD.SetActive(false);
        thirdPersonHUD.SetActive(true);
    }
    public void ThirdPersonHudSetInactive(){
        firstPersonHUD.SetActive(true);
        thirdPersonHUD.SetActive(false);
    }
    public void HudSetInactive(){
        firstPersonHUD.SetActive(false);
        thirdPersonHUD.SetActive(false);
    }

    private void LateUpdate() {
        if(currentCraft == null){
            gameObject.SetActive(false);
            return;
        }
        currentSpeed = currentCraft.currentSpeed;
        currentHealth = currentCraft.currentHealth;
        thrustInput = currentCraft.thrust;

        FillSpeedData();
        FillThrustData();
        FillHealthData();
    }

    private void FillSpeedData(){
        float difference = maxSpeed - currentSpeed;
        float barfill = 1 - (difference/maxSpeed);
        for(int i = 0; i < speedText.Count; i++){
            speedText[i].text = currentSpeed.ToString("#####");
        }
        for(int i = 0; i < speedBar.Count; i++){
            speedBar[i].fillAmount = barfill;
        }
    }
    private void FillThrustData(){
        float offsetThrust = thrustInput + 1;
        float difference = 2 - offsetThrust;
        float barfill = 1 - (difference/2);
        for(int i = 0; i < thrustBar.Count; i++){
            thrustBar[i].fillAmount = barfill;
        }
    }
    private void FillHealthData(){
        float difference = maxHealth - currentHealth;
        float barfill = 1 - (difference/maxHealth);
        for(int i = 0; i < healthText.Count; i++){
            healthText[i].text = currentHealth.ToString("#####");
        }
        for(int i = 0; i < healthBar.Count; i++){
            healthBar[i].fillAmount = barfill;
        }
        
    }
}
