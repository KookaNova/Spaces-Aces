using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cox.PlayerControls;
using System;
using System.Collections.Generic;

public class SelectScreenBehaviour : MonoBehaviour
{
    public PlayerObject playerObject;
    private CharacterHandler character;
    private ShipHandler ship;
    
    VisualElement root, characters, ships;
    VisualElement nameArt, bodyArt;
    Label Bio;

    [SerializeField] List<CharacterHandler> allCharacters;
    [SerializeField] List<ShipHandler> allShips;

    [SerializeField] Transform shipLocation;
    [SerializeField] GameObject shipBG;
    
    
    private GameObject shipModel;
    private GameObject instancedShip = null;
    private UISoundManager soundManager;

    private void Awake(){
        root = GetComponent<UIDocument>().rootVisualElement;
        characters = root.Q("Characters");
        ships = root.Q("Ships");
        nameArt = root.Q("Name");
        bodyArt = root.Q("BodyArt");
        Bio = root.Q<Label>("Bio");
        soundManager = FindObjectOfType<UISoundManager>();

        EnableCharacterSelect();

        for(int i = 0; i < allCharacters.Count; i++){
            var button = characters?.Q(i.ToString());
            button.style.backgroundImage = allCharacters[i].portrait;
        }

        for(int i = 0; i < allShips.Count; i++){
            var button = ships?.Q(i.ToString());
            button.style.backgroundImage = allShips[i].shipIcon;
        }

        root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        //Characters in order
        characters?.Q("0")?.RegisterCallback<FocusInEvent>(ev => DisplayCharacterData(allCharacters[0]));
        characters?.Q("1")?.RegisterCallback<FocusInEvent>(ev => DisplayCharacterData(allCharacters[1]));
        characters?.Q("2")?.RegisterCallback<FocusInEvent>(ev => DisplayCharacterData(allCharacters[2]));
        characters?.Q("3")?.RegisterCallback<FocusInEvent>(ev => DisplayCharacterData(allCharacters[3]));
        characters?.Q("4")?.RegisterCallback<FocusInEvent>(ev => DisplayCharacterData(allCharacters[4]));

        characters?.Q("0")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("CharConfirm").Focus());
        characters?.Q("1")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("CharConfirm").Focus());
        characters?.Q("2")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("CharConfirm").Focus());
        characters?.Q("3")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("CharConfirm").Focus());
        characters?.Q("4")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("CharConfirm").Focus());

        //Ships in order
        ships?.Q("0")?.RegisterCallback<FocusInEvent>(ev => DisplayShipData(allShips[0]));
        ships?.Q("1")?.RegisterCallback<FocusInEvent>(ev => DisplayShipData(allShips[1]));
        ships?.Q("2")?.RegisterCallback<FocusInEvent>(ev => DisplayShipData(allShips[2]));
        ships?.Q("3")?.RegisterCallback<FocusInEvent>(ev => DisplayShipData(allShips[3]));
        ships?.Q("4")?.RegisterCallback<FocusInEvent>(ev => DisplayShipData(allShips[4]));
        ships?.Q("5")?.RegisterCallback<FocusInEvent>(ev => DisplayShipData(allShips[5]));

        ships?.Q("0")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("ShipConfirm").Focus());
        ships?.Q("1")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("ShipConfirm").Focus());
        ships?.Q("2")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("ShipConfirm").Focus());
        ships?.Q("3")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("ShipConfirm").Focus());
        ships?.Q("4")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("ShipConfirm").Focus());
        ships?.Q("5")?.RegisterCallback<NavigationSubmitEvent>(ev => root?.Q("ShipConfirm").Focus());

        root?.Q("CharConfirm")?.RegisterCallback<NavigationSubmitEvent>(ev => EnableShipSelect());
        root?.Q("CharConfirm")?.RegisterCallback<ClickEvent>(ev => EnableShipSelect());
        root?.Q("ShipConfirm")?.RegisterCallback<NavigationSubmitEvent>(ev => EnableOverview());
        root?.Q("ShipConfirm")?.RegisterCallback<ClickEvent>(ev => EnableOverview());
        root?.Q("Confirm")?.RegisterCallback<NavigationSubmitEvent>(ev => UnloadSelectScreen());
        root?.Q("Confirm")?.RegisterCallback<ClickEvent>(ev => UnloadSelectScreen());
        root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void EnableCharacterSelect(){
        shipBG.SetActive(false);
        bodyArt.style.visibility = Visibility.Visible;

        root.Q("Characters").style.display = DisplayStyle.Flex;
        root.Q("Ships").style.display = DisplayStyle.None;

        root.Q("CharConfirm").style.display = DisplayStyle.Flex;
        root.Q("ShipConfirm").style.display = DisplayStyle.None;
        root?.Q("0")?.Focus();

    }
    private void EnableShipSelect(){
        shipBG.SetActive(true);
        bodyArt.style.visibility = Visibility.Hidden;

        root.Q("Characters").style.display = DisplayStyle.None;
        root.Q("Ships").style.display = DisplayStyle.Flex;

        root.Q("CharConfirm").style.display = DisplayStyle.None;
        root.Q("ShipConfirm").style.display = DisplayStyle.Flex;
        root?.Q("0")?.Focus();

        DisplayShipData(allShips[0]);
    }
    private void EnableOverview(){
        UnloadSelectScreen();

    }

    private void DisplayCharacterData(CharacterHandler newCharacter){
        soundManager.focus.Play();
        playerObject.ChangeCharacter(newCharacter);
        character = playerObject.chosenCharacter;

        nameArt.style.backgroundImage = character.nameArt;
        bodyArt.style.backgroundImage = character.bodyArt;
        Bio.text = character.bio;
    }

    private void DisplayShipData(ShipHandler newShip){
        soundManager.focus.Play();
        if(instancedShip != null){
            Destroy(instancedShip);
        }
        playerObject.ChangeShip(newShip);
        ship = playerObject.chosenShip;

        nameArt.style.backgroundImage = ship.nameArt;
        Bio.text = ship.bio;
        shipModel = ship.displayShip;

        bodyArt.style.visibility = Visibility.Hidden;
        instancedShip = Instantiate(shipModel, shipLocation.position, shipLocation.rotation, shipLocation);
    }

    public void UnloadSelectScreen(){
        root.Focus();
        FindObjectOfType<GameManager>().CloseSelectMenu();
    }
}
