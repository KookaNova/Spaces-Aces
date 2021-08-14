using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    public Image avatarImage, speedBar, healthBar;
    public Text speedText, healthText;

    [HideInInspector]
    public SpacecraftController currentCraft;
    
    private CharacterHandler chosenCharacter;
    private ShipHandler chosenShip;
    private float currentSpeed, maxSpeed, currentHealth, maxHealth;

    public void Activate(){
        if(currentCraft == null){
            Debug.LogError("PlayerHUDController: Activate(), currentCraft is null, script can't function");
            return;
        }
        PlayerProfileData data = SaveData.LoadProfile();
        chosenCharacter = currentCraft.chosenCharacter;
        chosenShip = currentCraft.chosenShip;
        if(chosenCharacter == null){
            Debug.LogWarning("PlayerHUDController: OnEnable(), currentCraft is null, can't fill Spacecraft info");
            return;
        }
        maxSpeed = chosenShip.maxSpeed;
        maxHealth = chosenShip.maxHealth;

        if(chosenCharacter == null){
            Debug.LogWarning("PlayerHUDController: OnEnable(), chosenCharacter is null, can't fill character info");
            return;
        }
        avatarImage.sprite = chosenCharacter.avatar;
    }

    private void LateUpdate() {
        currentSpeed = currentCraft.currentSpeed;
        currentHealth = currentCraft.currentHealth;

        FillSpeedData();
        FillHealthData();
    }

    private void FillSpeedData(){
        
        speedText.text = currentSpeed.ToString("#####");
        float difference = maxSpeed - currentSpeed;
        float barfill = 1 - (difference/maxSpeed);
        speedBar.fillAmount = barfill;
    }

    private void FillHealthData(){
        healthText.text = currentHealth.ToString("#####");
        float difference = maxHealth - currentHealth;
        float barfill = 1 - (difference/maxHealth);
        healthBar.fillAmount = barfill;
    }
}
