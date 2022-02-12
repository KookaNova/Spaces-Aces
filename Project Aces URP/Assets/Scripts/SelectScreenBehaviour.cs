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
    
    VisualElement root;
    VisualElement nameArt, bodyArt;
    Label Bio;

    [SerializeField] List<CharacterHandler> allCharacters;
    [SerializeField] List<ShipHandler> allShips;

    [SerializeField] Transform shipLocation;
    [SerializeField] GameObject shipBG;
    
    
    private GameObject shipModel;
    private GameObject instancedShip = null;

    private void Awake(){
        root = GetComponent<UIDocument>().rootVisualElement;
        nameArt = root.Q("Name");
        bodyArt = root.Q("BodyArt");
        Bio = root.Q<Label>("Bio");

        EnableCharacterSelect();

        for(int i = 0; i < allCharacters.Count; i++){
            var button = root?.Q(i.ToString());
            button.style.backgroundImage = allCharacters[i].portrait;
        }

        root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        //Characters in order
        root?.Q("0")?.RegisterCallback<ClickEvent>(ev => DisplayCharacterData(allCharacters[0]));
        root?.Q("1")?.RegisterCallback<ClickEvent>(ev => DisplayCharacterData(allCharacters[1]));
        root?.Q("2")?.RegisterCallback<ClickEvent>(ev => DisplayCharacterData(allCharacters[2]));
        root?.Q("3")?.RegisterCallback<ClickEvent>(ev => DisplayCharacterData(allCharacters[3]));
        root?.Q("4")?.RegisterCallback<ClickEvent>(ev => DisplayCharacterData(allCharacters[4]));

        //Ships in order
        root?.Q("Tri-Speeder")?.RegisterCallback<ClickEvent>(ev => DisplayShipData(allShips[0]));
        root?.Q("Angelfish")?.RegisterCallback<ClickEvent>(ev => DisplayShipData(allShips[1]));
        root?.Q("Furnace")?.RegisterCallback<ClickEvent>(ev => DisplayShipData(allShips[2]));
        root?.Q("Falcon")?.RegisterCallback<ClickEvent>(ev => DisplayShipData(allShips[3]));

        root?.Q("CharConfirm")?.RegisterCallback<ClickEvent>(ev => EnableShipSelect());
        root?.Q("ShipConfirm")?.RegisterCallback<ClickEvent>(ev => EnableOverview());
        root?.Q("Confirm")?.RegisterCallback<ClickEvent>(ev => UnloadSelectScreen());
    }

    private void EnableCharacterSelect(){
        shipBG.SetActive(false);
        bodyArt.style.visibility = Visibility.Visible;

        root.Q("Characters").style.display = DisplayStyle.Flex;
        root.Q("Ships").style.display = DisplayStyle.None;

        root.Q("CharConfirm").style.display = DisplayStyle.Flex;
        root.Q("ShipConfirm").style.display = DisplayStyle.None;

    }
    private void EnableShipSelect(){
        shipBG.SetActive(true);
        bodyArt.style.visibility = Visibility.Hidden;

        root.Q("Characters").style.display = DisplayStyle.None;
        root.Q("Ships").style.display = DisplayStyle.Flex;

        root.Q("CharConfirm").style.display = DisplayStyle.None;
        root.Q("ShipConfirm").style.display = DisplayStyle.Flex;

        DisplayShipData(allShips[0]);
    }
    private void EnableOverview(){
        UnloadSelectScreen();

    }

    private void DisplayCharacterData(CharacterHandler newCharacter){
        playerObject.ChangeCharacter(newCharacter);
        character = playerObject.chosenCharacter;

        nameArt.style.backgroundImage = character.nameArt;
        bodyArt.style.backgroundImage = character.bodyArt;
        Bio.text = character.bio;
    }

    private void DisplayShipData(ShipHandler newShip){
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
        FindObjectOfType<GameManager>().CloseSelectMenu();
        SceneManager.UnloadSceneAsync("Select Scene");
    }
}
