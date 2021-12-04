using UnityEngine.UIElements;


public class GameUIManager : VisualElement
{
    VisualElement selectScreen;
    VisualElement tabMenu;
    VisualElement startMenu;

    public new class UxmlFactory : UxmlFactory<GameUIManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    public GameUIManager(){
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt){
        tabMenu = this.Q("TabScreen");
        startMenu = this.Q("StartScreen");

        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ToggleTab(){
        if(tabMenu.style.display == DisplayStyle.Flex){
            tabMenu.style.display = DisplayStyle.None;
        }
        else{
            tabMenu.style.display = DisplayStyle.Flex;
        }

    }
    public void ToggleMenu(){
        if(startMenu.style.display == DisplayStyle.Flex){
            startMenu.style.display = DisplayStyle.None;
        }
        else{
            startMenu.style.display = DisplayStyle.Flex;
        }

    }
}
