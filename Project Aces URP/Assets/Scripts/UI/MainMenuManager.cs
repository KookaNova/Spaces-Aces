using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : VisualElement
{
    [SerializeField] ProfileHandler profileHandler;

    VisualElement m_TitleScreen;
    VisualElement m_MainMenu;

    public new class UxmlFactory : UxmlFactory<MainMenuManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }
    
    public MainMenuManager(){
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt){
        m_TitleScreen = this.Q("TitleScreen");
        m_MainMenu = this.Q("HomeMenu");

        m_TitleScreen?.Q("StartScreenButton")?.RegisterCallback<ClickEvent>(ev => CheckProfile());


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void CheckProfile(){
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

    void EnableProfileCreate(){
        
    }

    void EnableHomeMenu(){
        m_TitleScreen.style.display = DisplayStyle.None;
        m_MainMenu.style.display = DisplayStyle.Flex;
    }

}
