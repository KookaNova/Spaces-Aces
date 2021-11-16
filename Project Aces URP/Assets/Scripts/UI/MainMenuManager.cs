using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using UnityEngine.UIElements.Experimental;

public class MainMenuManager : VisualElement
{
    ProfileHandler profileHandler;
    SceneController sceneController;
    MultiplayerLauncher multiplayerLauncher;

    #region Menu Screens
    VisualElement m_Title;
    VisualElement m_ProfileCreate;
    VisualElement m_Multiplayer;
    VisualElement m_ConnectingScreen;
    VisualElement m_Friends;
    VisualElement m_Hangar;
    VisualElement m_Challenge;
    VisualElement m_Events;
    VisualElement m_Settings;
    VisualElement m_Profile;
    #endregion

    #region OverlayElements
    VisualElement o_TopBar;
    VisualElement o_NavigationButtons;
    VisualElement o_ChallengeList;
    VisualElement o_LauncherButtons;
    VisualElement o_ReturnButton;
    VisualElement o_SearchStatus;

    public bool isRunning => throw new System.NotImplementedException();

    public int durationMs { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }




    #endregion

    public new class UxmlFactory : UxmlFactory<MainMenuManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }
    
    public MainMenuManager(){
        sceneController = SceneController.FindObjectOfType<SceneController>();
        multiplayerLauncher = MultiplayerLauncher.FindObjectOfType<MultiplayerLauncher>();
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt){
        
        Debug.Log(sceneController);
        Debug.Log("Geometry Changed.");
        #region Assign Screens
        m_Title = this.Q("TitleScreen");
        m_ProfileCreate = this.Q("ProfileCreateScreen");
        m_Multiplayer = this.Q("MultiplayerScreen");
        m_ConnectingScreen = this.Q("ConnectingScreen");
        m_Friends = this.Q("FriendsScreen");
        m_Hangar = this.Q("HangarScreen");
        m_Challenge = this.Q("ChallengesScreen");
        m_Events = this.Q("EventsScreen");
        m_Settings = this.Q("SettingsScreen");
        m_Profile = this.Q("ProfileScreen");

        o_TopBar = this.Q("TopBar");
        o_NavigationButtons = o_TopBar.Q("NavigationButtons");
        o_ChallengeList = o_TopBar.Q("ChallengeList");
        o_LauncherButtons = this.Q("LauncherButtons");
        o_ReturnButton = this.Q("ReturnButton");
        o_SearchStatus = o_TopBar.Q("GameStatusContainer");
        #endregion
        
        //Start buttons
        m_Title?.Q("StartScreenButton")?.RegisterCallback<ClickEvent>(ev => CheckProfile());
        m_ProfileCreate?.Q("Confirm")?.RegisterCallback<ClickEvent>(ev => EnableHomeMenu());

        //Home Buttons
        o_ReturnButton?.Q("Return")?.RegisterCallback<ClickEvent>(ev => EnableHomeMenu());
        o_TopBar?.Q("DropDownButton")?.RegisterCallback<ClickEvent>(ev => EnableDropdown());
        o_LauncherButtons?.Q("ConnectMultiplayer")?.RegisterCallback<ClickEvent>(ev => multiplayerLauncher.ConnectToServer());
        o_NavigationButtons?.Q("Friends")?.RegisterCallback<ClickEvent>(ev => EnableFriendsScreen());
        o_NavigationButtons?.Q("Hangar")?.RegisterCallback<ClickEvent>(ev => EnableHangarScreen());
        o_NavigationButtons?.Q("Challenges")?.RegisterCallback<ClickEvent>(ev => EnableChallengesScreen());
        o_NavigationButtons?.Q("Settings")?.RegisterCallback<ClickEvent>(ev => EnableSettingsScreen());
        o_NavigationButtons?.Q("Exit")?.RegisterCallback<ClickEvent>(ev => sceneController.ExitGame());

        //Multiplayer Buttons
        m_Multiplayer?.Q("QuickplayButton")?.RegisterCallback<ClickEvent>(ev => multiplayerLauncher.FindMatchFromPlaylist(multiplayerLauncher.quickplay));
        o_SearchStatus?.Q("CancelButton")?.RegisterCallback<ClickEvent>(ev => multiplayerLauncher.LeaveRoom());


       
        

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void CheckProfile(){
        profileHandler = GameObject.FindObjectOfType<ProfileHandler>();
        PlayerProfileData data = SaveData.LoadProfile();
        if(data == null || data.currentLevel <= 0){
            profileHandler.InitializeData();
            EnableProfileCreate();
            
            Debug.Log("MenuController: User must create a profile; activating create screen");
        }
        else{
            EnableHomeMenu();
        }

    }

    #region Enable Menus
    
    void EnableDropdown(){

        o_NavigationButtons.SetEnabled(!o_NavigationButtons.enabledSelf);
    }

    public void EnableSearchOverlay(bool isFlex){
       if(isFlex){
           o_SearchStatus.style.display = DisplayStyle.Flex;
       }
       else{
           o_SearchStatus.style.display = DisplayStyle.None;
       }
    }


    void EnableProfileCreate(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.Flex;//------------------------------------------------------
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.None; 
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None; 
        o_LauncherButtons.style.display = DisplayStyle.None; 
        o_ReturnButton.style.display = DisplayStyle.None; 

        Debug.Log("Menu: ProfileCreate");
    }
    public void EnableHomeMenu(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.Flex; //------
        o_NavigationButtons.style.display = DisplayStyle.Flex;
        o_ChallengeList.style.display = DisplayStyle.Flex; //--------
        o_LauncherButtons.style.display = DisplayStyle.Flex; //--------
        o_ReturnButton.style.display = DisplayStyle.None;     

        o_NavigationButtons.SetEnabled(false);

        Debug.Log("Menu: Home");
    }

    public void EnableConnectingScreen(){
        //multiplayerLauncher.ConnectToServer();

        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.Flex;//------
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.None;
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None;
        o_LauncherButtons.style.display = DisplayStyle.None;
        o_ReturnButton.style.display = DisplayStyle.Flex;//----
        

    }
    public void EnableMultiplayerScreen(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.Flex;//--------
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.None;
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None;
        o_LauncherButtons.style.display = DisplayStyle.None;
        o_ReturnButton.style.display = DisplayStyle.Flex;//----

    }

    void EnableFriendsScreen(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.Flex;//---
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.Flex;//---
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None;
        o_LauncherButtons.style.display = DisplayStyle.None;
        o_ReturnButton.style.display = DisplayStyle.Flex;//----

    }

    void EnableHangarScreen(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.Flex;//---
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.Flex;//---
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None;
        o_LauncherButtons.style.display = DisplayStyle.None;
        o_ReturnButton.style.display = DisplayStyle.Flex;//----

    }

    void EnableChallengesScreen(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.Flex;//---
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.Flex;//---
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None;
        o_LauncherButtons.style.display = DisplayStyle.None;
        o_ReturnButton.style.display = DisplayStyle.Flex;//----

    }

    void EnableEventsScreen(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.Flex;//---
        m_Settings.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.Flex;
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None;
        o_LauncherButtons.style.display = DisplayStyle.None;
        o_ReturnButton.style.display = DisplayStyle.Flex;//----

    }

    void EnableSettingsScreen(){
        m_Title.style.display = DisplayStyle.None;
        m_ProfileCreate.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_ConnectingScreen.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Hangar.style.display = DisplayStyle.None;
        m_Challenge.style.display = DisplayStyle.None;
        m_Events.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.Flex;//---
        m_Profile.style.display = DisplayStyle.None;

        o_TopBar.style.display = DisplayStyle.Flex;//----
        o_NavigationButtons.style.display = DisplayStyle.None;
        o_ChallengeList.style.display = DisplayStyle.None;
        o_LauncherButtons.style.display = DisplayStyle.None;
        o_ReturnButton.style.display = DisplayStyle.Flex;//----

    }
    #endregion
}




