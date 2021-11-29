using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuHelper : MonoBehaviour
{
    public ProfileHandler profileHandler;

    #region UI Fields
    VisualElement root;
    MenuManager menuManager; 
    #endregion
    

    void Start()
    {
        root = this.GetComponent<UIDocument>().rootVisualElement;
        menuManager = root.Q<MenuManager>();

        //open title screen
    }

    public void CheckProfile(){
        profileHandler = GameObject.FindObjectOfType<ProfileHandler>();
        PlayerProfileData data = SaveData.LoadProfile();

        if(data == null || data.currentLevel <= 0){
            profileHandler.InitializeData();
            //EnableProfileCreate();
            
            Debug.Log("MenuController: User must create a profile; activating create screen");
        }
        else{
           // menuManager.EnableHome();
        }

    }

}
