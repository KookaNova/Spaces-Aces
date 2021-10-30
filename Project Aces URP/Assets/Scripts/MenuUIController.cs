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
        friendsMenu,
        hangarMenu,
        eventsMenu,
        trainingMenu,
        settingsMenu,
        challengesMenu;
    bool isTransitioning = false;
    [SerializeField]
    private ProfileHandler profileHandler;

    //Return to start screen on Awake()
    private void Awake() {
        mainMenu.SetActive(false);
        profileBar.SetActive(false);
        createProfileScreen.SetActive(false);
        personalProfileMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
        trainingMenu.SetActive(false);
        settingsMenu.SetActive(false);
        startScreen.SetActive(true);
    }

    //Check if there's a profile, if not, create one.
    public void StartScreenClicked() {
        PlayerProfileData data = SaveData.LoadProfile();
        if(data == null || data.currentLevel <= 0){
            createProfileScreen.SetActive(true);
            profileHandler.InitializeData();
            mainMenu.SetActive(false);
            
            Debug.Log("MenuController: User must create a profile; activating create screen");
        }
        else{
            MenuReturnToMain();
        }
    }
    //Deactivates all menus, activates the main menu
    public void MenuReturnToMain(){
        mainMenu.SetActive(true);
        profileBar.SetActive(true);
        startScreen.SetActive(false);
        createProfileScreen.SetActive(false);
        personalProfileMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
        trainingMenu.SetActive(false);
        settingsMenu.SetActive(false);
        
    }

    public void MenuProfile(){
        mainMenu.SetActive(false);
        personalProfileMenu.SetActive(true);
    }
    public void MenuMultiplayer(){
        mainMenu.SetActive(false);
        multiplayerMenu.SetActive(true);
    }
    public void MenuTraining(){
        mainMenu.SetActive(false);
        trainingMenu.SetActive(true);
    }
    public void MenuFriends(){
        mainMenu.SetActive(false);
        friendsMenu.SetActive(true);
    }
    public void MenuHangar(){
        mainMenu.SetActive(false);
        hangarMenu.SetActive(true);
    }
    public void MenuEvents(){
        mainMenu.SetActive(false);
        eventsMenu.SetActive(true);
    }
    public void MenuSettings(){
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void MenuChallenges(){
        mainMenu.SetActive(false);
        challengesMenu.SetActive(true);
    }
}
