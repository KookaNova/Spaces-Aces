using System;
using UnityEngine.UIElements;

public class MenuManager : VisualElement
{
    ProfileHandler profileHandler;
    SceneController sceneController;
    MultiplayerLauncher multiplayerLauncher;
    MenuHelper menuHelper;


    #region Screens
    VisualElement m_Title;
    VisualElement m_TopBar;
    VisualElement m_MenuSelector;
    VisualElement m_Home;
    VisualElement m_Multiplayer;
    VisualElement m_Connecting;
    VisualElement m_MatchSearch;
    VisualElement m_ReturnMain;
    VisualElement m_Friends;
    VisualElement m_Profile;
    VisualElement m_Credits;
    VisualElement m_Nameplate;
    VisualElement m_PostGame;
    VisualElement m_Settings;

    Label l_MenuName;
         
    #endregion

    public new class UxmlFactory : UxmlFactory<MenuManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    public MenuManager(){
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        sceneController = SceneController.FindObjectOfType<SceneController>();
        multiplayerLauncher = MultiplayerLauncher.FindObjectOfType<MultiplayerLauncher>();
        menuHelper = MenuHelper.FindObjectOfType<MenuHelper>();
        
        
        
    }

    private void OnGeometryChange(GeometryChangedEvent evt)
    {
        //assign screens
        m_Title = this.Q("Title");
        m_TopBar = this.Q("TopBar");
        m_MenuSelector = this.Q("MenuSelector");
        m_Nameplate = this.Q("Nameplate");
        m_Home = this.Q("HomeButtons");
        m_Multiplayer = this.Q("MultiplayerScreen");
        m_Connecting = this.Q("ConnectingScreen");
        m_MatchSearch = this.Q("MatchSearch");
        m_ReturnMain = this.Q("Return");
        m_Friends = this.Q("Friends");
        m_Profile = this.Q("Profile");
        m_Credits = this.Q("Credits");
        m_PostGame = this.Q("PostGame");
        m_Settings = this.Q("Settings");
        

        l_MenuName = this.Q<Label>("MenuName");
       

        //Click Events
        m_Title?.RegisterCallback<ClickEvent>(ev => TitleClicked()); //animation class
        m_Title?.RegisterCallback<ClickEvent>(ev => m_Title.Q("Start").style.visibility = Visibility.Hidden); //animation class

        //Home Clicks
        m_ReturnMain?.RegisterCallback<ClickEvent>(ev => EnableHome());
        m_Home?.Q("Multiplayer")?.RegisterCallback<ClickEvent>(ev => multiplayerLauncher.ConnectToServer());
        m_Home?.Q("Credits")?.RegisterCallback<ClickEvent>(ev => EnableCredits());

        m_MenuSelector?.Q("b_Home")?.RegisterCallback<ClickEvent>(ev => EnableHome());
        m_MenuSelector?.Q("b_Friends")?.RegisterCallback<ClickEvent>(ev => EnableFriends());
        m_MenuSelector?.Q("b_Settings")?.RegisterCallback<ClickEvent>(ev => EnableSettings());
        m_Nameplate.RegisterCallback<ClickEvent>(ev => EnableProfile());
        //m_Exit?.RegisterCallback<ClickEvent>(ev => sceneController.ExitGame());

        //Multiplayer Clicks
        m_Multiplayer?.Q("Quickplay")?.RegisterCallback<ClickEvent>(ev => multiplayerLauncher.FindMatchFromPlaylist(multiplayerLauncher.quickplay));
        m_MatchSearch?.Q("CancelSearch")?.RegisterCallback<ClickEvent>(ev => multiplayerLauncher.LeaveRoom());


        //Transition End Events
        m_Title?.RegisterCallback<TransitionEndEvent>(ev => EnableHome());

        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }


    private void TitleClicked(){
        m_Title.Q("Art").AddToClassList("opacityOut");
        m_Home.AddToClassList("offsetLeft");
        //this.Q("GameInfo").AddToClassList("offsetRight");
        m_MenuSelector.AddToClassList("offsetLeft");
        m_Nameplate.AddToClassList("offsetRight");

        //code to check profile
    }
    private void DisableAllScreens(){
        //Add or remove necessary animation classes
        m_Home.AddToClassList("offsetLeft");
        m_MenuSelector.AddToClassList("offsetLeft");
        m_Nameplate.AddToClassList("offsetRight");

        //turn off all screens
        m_Title.style.display = DisplayStyle.None;
        m_Home.style.display = DisplayStyle.None;
        m_MenuSelector.style.display = DisplayStyle.None;
        m_Nameplate.style.display = DisplayStyle.None;
        m_ReturnMain.style.display = DisplayStyle.None;
        
        m_Connecting.style.display = DisplayStyle.None;
        m_Multiplayer.style.display = DisplayStyle.None;
        m_Friends.style.display = DisplayStyle.None;
        m_Credits.style.display = DisplayStyle.None;
        m_Profile.style.display = DisplayStyle.None;
        m_PostGame.style.display = DisplayStyle.None;
        m_Settings.style.display = DisplayStyle.None;

        l_MenuName.style.display = DisplayStyle.None;
    }

    private void EnableProfileCreate(){

    }

    public void EnableHome(){
        DisableAllScreens();

        m_Home.RemoveFromClassList("offsetLeft");
        m_MenuSelector.RemoveFromClassList("offsetLeft");
        m_Nameplate.RemoveFromClassList("offsetRight");

        m_ReturnMain.style.display = DisplayStyle.None;
        
        m_Home.style.display = DisplayStyle.Flex;
        m_MenuSelector.style.display = DisplayStyle.Flex;
        m_Nameplate.style.display = DisplayStyle.Flex;
        
    }

    void EnableFriends(){
        DisableAllScreens();
        m_MenuSelector.RemoveFromClassList("offsetLeft");
        m_MenuSelector.style.display = DisplayStyle.Flex;
        m_Nameplate.style.display = DisplayStyle.Flex;

        m_Friends.style.display = DisplayStyle.Flex;
    }

    void EnableProfile(){
        DisableAllScreens();
        m_MenuSelector.RemoveFromClassList("offsetLeft");
        m_MenuSelector.style.display = DisplayStyle.Flex;
        m_Nameplate.style.display = DisplayStyle.Flex;

        m_Profile.style.display = DisplayStyle.Flex;
    }

    void EnableCredits(){
        DisableAllScreens();

        m_Credits.style.display = DisplayStyle.Flex;
        m_ReturnMain.style.display = DisplayStyle.Flex;

    }

    private void EnableSettings(){
        DisableAllScreens();
        m_MenuSelector.RemoveFromClassList("offsetLeft");
        m_MenuSelector.style.display = DisplayStyle.Flex;
        m_Nameplate.style.display = DisplayStyle.Flex;

        m_Settings.style.display = DisplayStyle.Flex;
    }
    

    public void EnableConnectingScreen(){
        DisableAllScreens();
        m_Connecting.style.display = DisplayStyle.Flex;
        m_ReturnMain.style.display = DisplayStyle.Flex;

    }

    public void EnableMultiplayerScreen(){
        DisableAllScreens();
        l_MenuName.style.display = DisplayStyle.Flex;
        l_MenuName.text = "Multiplayer";
        m_Multiplayer.style.display = DisplayStyle.Flex;
        m_ReturnMain.style.display = DisplayStyle.Flex;
    }

    public void EnableMatchSearch(){
        m_MatchSearch.style.display = DisplayStyle.Flex;
        
    }
    public void DisableMatchSearch(){
        m_MatchSearch.style.display = DisplayStyle.None;
        
    }

    public void EnablePostGame(){
        DisableAllScreens();
        m_PostGame.style.display = DisplayStyle.Flex;

    }
}
