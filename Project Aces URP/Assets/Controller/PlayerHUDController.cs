using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cox.PlayerControls{
public class PlayerHUDController : MonoBehaviour
{
    public Image avatarImage;
    public List<Image> speedBar, thrustBar, healthBar, shieldBar;
    public List<Text> speedText, healthText, shieldText;

    [HideInInspector]
    public SpacecraftController currentCraft;
    [SerializeField]
    private GameObject firstPersonHUD, thirdPersonHUD, OverlayHUD;
    private CharacterHandler chosenCharacter;
    private float currentSpeed, maxSpeed, thrustInput, currentHealth, maxHealth, currentShields, maxShields;

    public void Activate(){
        if(currentCraft == null){
            Debug.LogError("PlayerHUDController: Activate(), currentCraft is null, script can't function");
            return;
        }
        firstPersonHUD.SetActive(true); OverlayHUD.SetActive(true);
        chosenCharacter = currentCraft.chosenCharacter;

        if(chosenCharacter == null){
            Debug.LogWarning("PlayerHUDController: OnEnable(), currentCraft is null, can't fill Spacecraft info");
            return;
        }
        
        maxSpeed = currentCraft.maxSpeed;
        maxHealth = currentCraft.maxHealth;
        maxShields = currentCraft.maxShield;

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
        currentShields = currentCraft.currentShields;
        thrustInput = currentCraft.thrust;

        FillSpeedData();
        FillThrustData();
        FillHealthData();
        FillShieldData();
    }

    private void FillSpeedData(){
        float difference = maxSpeed - currentSpeed;
        float barfill = 1 - (difference/maxSpeed);
        for(int i = 0; i < speedText.Count; i++){
            if(speedText[i] == null){
                Debug.LogError("HudController: Speed Text list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            speedText[i].text = currentSpeed.ToString("#####");
        }
        for(int i = 0; i < speedBar.Count; i++){
            if(speedBar[i] == null){
                Debug.LogError("HudController: Speed Bar list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
            }
            speedBar[i].fillAmount = barfill;
        }
    }
    private void FillThrustData(){
        for(int i = 0; i < thrustBar.Count; i++){
            if(thrustBar[i] == null){
                Debug.LogError("HudController: Thrust Bar list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            thrustBar[i].fillAmount = thrustInput;
        }
    }
    private void FillHealthData(){
        float difference = maxHealth - currentHealth;
        float barfill = 1 - (difference/maxHealth);
        for(int i = 0; i < healthText.Count; i++){
            if(healthText[i] == null){
                Debug.LogError("HudController: Health Text list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            healthText[i].text = currentHealth.ToString("#####");
        }
        for(int i = 0; i < healthBar.Count; i++){
            if(healthBar[i] == null){
                Debug.LogError("HudController: Health Bar list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            healthBar[i].fillAmount = barfill;
        }
        
    }
     private void FillShieldData(){
        float difference = maxShields - currentShields;
        float barfill = 1 - (difference/maxShields);
        for(int i = 0; i < shieldText.Count; i++){
            if(shieldText[i] == null){
                Debug.LogError("HudController: Shield Text list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            shieldText[i].text = currentShields.ToString("#####");
        }
        for(int i = 0; i < healthBar.Count; i++){
            if(shieldBar[i] == null){
                Debug.LogError("HudController: Shield Bar list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            shieldBar[i].fillAmount = barfill;
        }
        
    }
}
}
