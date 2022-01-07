using UnityEngine.UIElements;


public class GameUIManager : VisualElement
{
    VisualElement selectScreen;
    VisualElement mainScreen;
    VisualElement tabMenu;
    VisualElement startMenu;

    public new class UxmlFactory : UxmlFactory<GameUIManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    public GameUIManager(){
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt){
        mainScreen = this.Q("MainScreen");
        tabMenu = this.Q("TabScreen");
        startMenu = this.Q("StartScreen");

        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void DisableScreens(){
        mainScreen.style.display = DisplayStyle.None;
        tabMenu.style.display = DisplayStyle.None;
        startMenu.style.display = DisplayStyle.None;

    }

    public void EnableMainScreen(){
        DisableScreens();
        mainScreen.style.display = DisplayStyle.Flex;
    }

    public void ToggleTab(){
        if(tabMenu.style.display == DisplayStyle.Flex){
            EnableMainScreen();
        }
        else{
            DisableScreens();
            tabMenu.style.display = DisplayStyle.Flex;
        }

    }
    public void ToggleMenu(){
        if(startMenu.style.display == DisplayStyle.Flex){
            EnableMainScreen();
        }
        else{
            DisableScreens();
            startMenu.style.display = DisplayStyle.Flex;
        }

    }
}
