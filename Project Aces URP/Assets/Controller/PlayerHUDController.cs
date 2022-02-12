using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Cox.PlayerControls{
public class PlayerHUDController : MonoBehaviour
{
    //Canvases
    [SerializeField] private GameObject firstPersonHUD, OverlayHUD;
    VisualElement root;

    //UI displayed for character items like abilities and portraits.
    [Header("Character UI")]
    private CharacterHandler chosenCharacter;

    //UI displayed for ship UI.
    [Header("Ship UI")]
    [SerializeField] private GameObject lowHealthIndicator;
    [SerializeField] private List<UnityEngine.UI.Image> speedBar, thrustBar, healthBar, shieldBar;
    [SerializeField] private List<Text> speedText, healthText, shieldText;
    [HideInInspector] public SpacecraftController currentCraft;

    //values to display in UI.
    private float currentSpeed, maxSpeed, thrustInput, currentHealth, maxHealth, currentShields, maxShields;

    public void Activate(){

        root = FindObjectOfType<UIDocument>().rootVisualElement;

        if(currentCraft == null){
            Debug.LogError("PlayerHUDController: Activate(), currentCraft is null, script can't function");
            return;
        }
        firstPersonHUD.SetActive(true); OverlayHUD.SetActive(true);
        OverlayHUD.GetComponent<Canvas>().worldCamera = Camera.main;
        chosenCharacter = currentCraft.chosenCharacter;

        if(chosenCharacter == null){
            Debug.LogWarning("PlayerHUDController: OnEnable(), currentCraft is null, can't fill Spacecraft info");
            return;
        }
        
        maxSpeed = currentCraft.maxSpeed;
        maxHealth = currentCraft.maxHealth;
        maxShields = currentCraft.maxShield;

        SetPlayerIcons();
    }
    public void FirstPersonHudSetInactive(){
        firstPersonHUD.SetActive(false);
        OverlayHUD.SetActive(true);
        root.Q("Radar").style.display.Equals(DisplayStyle.None);
    }
    public void ThirdPersonHudSetInactive(){
        firstPersonHUD.SetActive(true);
        OverlayHUD.SetActive(true);
        root.Q("Radar").style.display.Equals(DisplayStyle.Flex);
    }
    public void HudSetInactive(){
        firstPersonHUD.SetActive(false);
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

        root.Q<Label>("TargetingMode").text = "Targeting:" + currentCraft.weaponSystem.targMode.ToString();

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
            if(currentHealth > 0){
                healthText[i].text = currentHealth.ToString("#####");
            }
            else{
                healthText[i].text = "0";
            }
            
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
            if(currentShields > 0){
                shieldText[i].text = currentShields.ToString("#####");
            }
            else{
                shieldText[i].text = "NO SHIELDS!";
            }
            
        }
        for(int i = 0; i < healthBar.Count; i++){
            if(shieldBar[i] == null){
                Debug.LogError("HudController: Shield Bar list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            shieldBar[i].fillAmount = barfill;
        }
        
    }

    public void IsLowHealth(bool Active){
        lowHealthIndicator.SetActive(Active);
    }

    #region UITK

    public void SetPlayerIcons(){
        root.Q("Portrait").style.backgroundImage = chosenCharacter.portrait;
        root.Q("PrimAbility").style.backgroundImage = chosenCharacter.abilities[0].icon;
        root.Q("SecAbility").style.backgroundImage = chosenCharacter.abilities[1].icon;
        root.Q("AceAbility").style.backgroundImage = chosenCharacter.abilities[2].icon;

    }

    #endregion
}
}
