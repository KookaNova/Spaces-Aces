using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu, 
        profileBar, 
        startScreen, 
        createProfileScreen, 
        personalProfileScene,
        multiplayerMenu,
        matchmakingMenu,
        optionsMenu;
    [SerializeField]
    PlayerProfileData profileData;

    public void StartScreenClicked() {
        if(profileData.currentLevel == 0){
            createProfileScreen.SetActive(true);
            mainMenu.SetActive(false);
            
            Debug.Log("MenuController: User must create a profile; activating create screen");
        }
        else{
            ReturnToMainMenu();
        }
    }

    public void EditProfileMenu(){
        profileBar.SetActive(true);
    }
    public void OpenMultiplayerMenu(){
        mainMenu.SetActive(false);
        matchmakingMenu.SetActive(false);
        multiplayerMenu.SetActive(true);
    }
    public void OpenMatchmakingMenu(){
        multiplayerMenu.SetActive(false);
        matchmakingMenu.SetActive(true);
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
        personalProfileScene.SetActive(false);
        multiplayerMenu.SetActive(false);
        matchmakingMenu.SetActive(false);
        optionsMenu.SetActive(false);
        
    }


}
