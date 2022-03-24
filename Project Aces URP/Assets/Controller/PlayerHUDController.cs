using System.Collections;
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
    private CharacterHandler chosenCharacter;
    [HideInInspector] public SpacecraftController owner;

    //UI displayed for ship UI.
    [SerializeField] private List<GameObject> lowHealthIndicator, caution, warning, missileAlert, damageAlert, hitAlert, eliminatedAlert;
    [SerializeField] private List<UnityEngine.UI.Image> speedBar, thrustBar, healthBar, shieldBar, gunCharge;
    [SerializeField] private List<Text> speedText, healthText, shieldText;
    [SerializeField] private List<Text> textTargetMode, missileCountText;
    [SerializeField] private List<Text> textPosition, textRotation;

    private Color gunChargeColor;
    

    //values to display in UI.
    private float currentSpeed, maxSpeed, thrustInput, currentHealth, maxHealth, currentShields, maxShields;

    public void Activate(){

        root = FindObjectOfType<UIDocument>().rootVisualElement;

        if(owner == null){
            Debug.LogError("PlayerHUDController: Activate(), currentCraft is null, script can't function");
            return;
        }
        firstPersonHUD.SetActive(true); OverlayHUD.SetActive(true);
        OverlayHUD.GetComponent<Canvas>().worldCamera = Camera.main;

        for(int i = 0; i < lowHealthIndicator.Count; i++){
            lowHealthIndicator[i].SetActive(false);
        }
        for(int i = 0; i < caution.Count; i++){
            caution[i].SetActive(false);
        }
        for(int i = 0; i < warning.Count; i++){
            warning[i].SetActive(false);
        }
        for(int i = 0; i < missileAlert.Count; i++){
            missileAlert[i].SetActive(false);
        }
        for(int i = 0; i < damageAlert.Count; i++){
            damageAlert[i].SetActive(false);
        }
        for(int i = 0; i < hitAlert.Count; i++){
            hitAlert[i].SetActive(false);
        }
        for(int i = 0; i < eliminatedAlert.Count; i++){
            eliminatedAlert[i].SetActive(false);
        }

        chosenCharacter = owner.chosenCharacter;

        if(chosenCharacter == null){
            Debug.LogWarning("PlayerHUDController: OnEnable(), currentCraft is null, can't fill Spacecraft info");
            return;
        }
        
        maxSpeed = owner.maxSpeed;
        maxHealth = owner.maxHealth;
        maxShields = owner.maxShield;

        if(gunCharge.Count > 0){
            gunChargeColor = gunCharge[0].color;
        }
        

        SetPlayerIcons();
    }
    #region Camera
    public void FirstPersonHudSetInactive(){
        firstPersonHUD.SetActive(false);
        OverlayHUD.SetActive(true);
        root.Q("Radar").style.display = DisplayStyle.Flex;
    }
    public void ThirdPersonHudSetInactive(){
        firstPersonHUD.SetActive(true);
        OverlayHUD.SetActive(true);
        root.Q("Radar").style.display = DisplayStyle.None;
    }
    public void OverlaySetActive(bool state){
        OverlayHUD.SetActive(state);
    }
    #endregion

    private void LateUpdate() {
        if(owner == null){
            gameObject.SetActive(false);
            return;
        }
        currentSpeed = owner.currentSpeed;
        currentHealth = owner.currentHealth;
        currentShields = owner.currentShields;
        thrustInput = owner.thrust;

        

        FillSpeedData();
        FillThrustData();
        FillHealthData();
        FillShieldData();
        FillWeaponsData();
    }

    #region Alerts
        public void HealthAlertActive(bool state){
            for(int i = 0; i < lowHealthIndicator.Count; i++){
                lowHealthIndicator[i].SetActive(state);
            }
        }
        public void CautionAlertActive(bool state){
            for(int i = 0; i < caution.Count; i++){
                caution[i].SetActive(state);
            }
        }
        public void WarningAlertActive(bool state){
            for(int i = 0; i < warning.Count; i++){
                warning[i].SetActive(state);
            }
        }
        public void MissileAlertActive(bool state){
            for(int i = 0; i < missileAlert.Count; i++){
                missileAlert[i].SetActive(state);
            }
        }
        public void DamageAlertActive(bool state){
            for(int i = 0; i < damageAlert.Count; i++){
                damageAlert[i].SetActive(state);
            }
            if(state && this.isActiveAndEnabled){
                StartCoroutine(DamageAlertTimer());
            }
        }
        public void HitAlertActive(bool state){
            for(int i = 0; i < hitAlert.Count; i++){
                hitAlert[i].SetActive(state);
            }
            if(state && this.isActiveAndEnabled){
                StartCoroutine(HitTimer());
            }
        }
        public void EliminatedAlertActive(bool state){
            HitAlertActive(false);
            for(int i = 0; i < eliminatedAlert.Count; i++){
                eliminatedAlert[i].SetActive(state);
            }
            if(state && this.isActiveAndEnabled){
                StartCoroutine(EliminatedTimer());
            }
        }
    #endregion

    #region Data Fill
    private void FillWeaponsData(){
        var weapons = owner.weaponSystem;

        for(int i = 0; i < textTargetMode.Count; i++){
            switch(weapons.targMode){
                case WeaponsController.TargetingMode.TeamA:
                    if(owner.teamName == "A"){
                        textTargetMode[i].text = ("Targeting Allies");
                        root.Q<Label>("TargetingMode").text = "Targeting Allies";
                    }
                    else{
                        textTargetMode[i].text = ("Targeting Enemies");
                        root.Q<Label>("TargetingMode").text = "Targeting Enemies";
                    }
                    break;
                case WeaponsController.TargetingMode.TeamB:
                    if(owner.teamName == "A"){
                        textTargetMode[i].text = ("Targeting Enemies");
                        root.Q<Label>("TargetingMode").text = "Targeting Enemies";
                    }
                    else{
                        textTargetMode[i].text = ("Targeting Allies");
                        root.Q<Label>("TargetingMode").text = "Targeting Allies";
                    }
                    break;
                case WeaponsController.TargetingMode.Global:
                        textTargetMode[i].text = ("Targeting " + weapons.targMode.ToString());
                        root.Q<Label>("TargetingMode").text = "Targeting:" + weapons.targMode.ToString();
                    break;
                case WeaponsController.TargetingMode.Objective:
                    textTargetMode[i].text = ("Targeting " + weapons.targMode.ToString());
                    root.Q<Label>("TargetingMode").text = "Targeting:" + weapons.targMode.ToString();
                    break;

            } 
        }

        for(int i = 0; i < missileCountText.Count; i++){
            missileCountText[i].text = "(" + weapons.missilesAvailable.ToString("0") + ")";
        }

        for(int i = 0; i < textPosition.Count; i++){
            textPosition[i].text = owner.shipPosition.ToString("0000");
        }

        for(int i = 0; i < textRotation.Count; i++){
            textRotation[i].text = owner.shipRotation.ToString("000");
        }
        for(int i = 0; i < gunCharge.Count; i++){
            gunCharge[i].fillAmount = weapons.gunCharge;
            gunCharge[i].color = new Color(gunChargeColor.r + weapons.gunCharge, gunChargeColor.g, gunChargeColor.b, gunChargeColor.a);
        }
    }

    private void FillSpeedData(){
        float difference = maxSpeed - currentSpeed;
        float barfill = 1 - (difference/maxSpeed);
        for(int i = 0; i < speedText.Count; i++){
            if(speedText[i] == null){
                Debug.LogError("HudController: Speed Text list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            speedText[i].text = "(" + currentSpeed.ToString("#000") + ")";
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
                healthText[i].text = "(" + currentHealth.ToString("####") + ")";
            }
            else{
                healthText[i].text = "(0)";
            }
            
        }
        for(int i = 0; i < healthBar.Count; i++){
            if(healthBar[i] == null){
                Debug.LogError("HudController: Health Bar list is larger than the amount of objects in the list. This will cause errors with HUD. Remove unused elements.");
                return;
            }
            if(currentHealth > 0){
                healthBar[i].fillAmount = barfill;
            }
            else{
                healthBar[i].fillAmount = 0;
            }
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
                shieldText[i].text = "(" + currentShields.ToString("#####") + ")";
            }
            else{
                shieldText[i].text = "(NO SHIELDS!)";
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

    #endregion

    #region UITK

    private void SetPlayerIcons(){
        root.Q("Portrait").style.backgroundImage = chosenCharacter.portrait;
        root.Q("PrimAbility").style.backgroundImage = chosenCharacter.abilities[0].icon;
        root.Q("SecAbility").style.backgroundImage = chosenCharacter.abilities[1].icon;
        root.Q("AceAbility").style.backgroundImage = chosenCharacter.abilities[2].icon;

    }

    #endregion

    #region IEnumerators
    private IEnumerator DamageAlertTimer(){
        yield return new WaitForSecondsRealtime(2);
        DamageAlertActive(false);


    }
    private IEnumerator HitTimer(){
        yield return new WaitForSecondsRealtime(1.5f);
        HitAlertActive(false);
        

    }
    private IEnumerator EliminatedTimer(){
        yield return new WaitForSecondsRealtime(1.5f);
        EliminatedAlertActive(false);

    }
    #endregion
}
}
