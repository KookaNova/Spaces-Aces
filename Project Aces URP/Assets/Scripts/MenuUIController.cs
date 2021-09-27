using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MenuUIController : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu, 
        profileBar, 
        startScreen, 
        createProfileScreen, 
        personalProfileMenu,
        multiplayerMenu,
        trainingMenu,
        optionsMenu;
    bool isTransitioning = false;
    [SerializeField]
    private ProfileHandler profileHandler;

    private void Awake() {
        mainMenu.SetActive(false);
        profileBar.SetActive(false);
        createProfileScreen.SetActive(false);
        personalProfileMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
        trainingMenu.SetActive(false);
        optionsMenu.SetActive(false);
        startScreen.SetActive(true);
    }

    public void StartScreenClicked() {
        PlayerProfileData data = SaveData.LoadProfile();
        if(data == null || data.currentLevel <= 0){
            createProfileScreen.SetActive(true);
            profileHandler.InitializeData();
            mainMenu.SetActive(false);
            
            Debug.Log("MenuController: User must create a profile; activating create screen");
        }
        else{
            ReturnToMainMenu();
        }
    }

    public void OpenProfileMenu(){
        personalProfileMenu.SetActive(true);
    }
    public void OpenMultiplayerMenu(){
        mainMenu.SetActive(false);
        multiplayerMenu.SetActive(true);
    }
    public void OpenTrainingMenu(){
        mainMenu.SetActive(false);
        trainingMenu.SetActive(true);
    }

    public void OpenOptionsMenu(){
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ReturnToMainMenu(){
        mainMenu.SetActive(true);
        profileBar.SetActive(true);
        startScreen.SetActive(false);
        createProfileScreen.SetActive(false);
        personalProfileMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
        trainingMenu.SetActive(false);
        optionsMenu.SetActive(false);
        
    }
}
