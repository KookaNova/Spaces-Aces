using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cox.PlayerControls;

public class SelectScreenBehaviour : MonoBehaviour
{
    public PlayerObject playerObject;
    
    [SerializeField] private Texture characterNameArt, bodyArt, shipNameArt;
    [SerializeField] private Transform shipLocation;
    [SerializeField] private Label charBio, shipBio;

    private CharacterHandler character;
    private ShipHandler ship;

    private GameObject shipModel;
    private GameObject instancedShip = null;

    public void DisplayCharacterData(){
        character = playerObject.chosenCharacter;
        
        characterNameArt = character.nameArt;
        bodyArt = character.bodyArt;
        charBio.text = character.bio;
    }

    public void DisplayShipData(){
        if(instancedShip != null){
            Destroy(instancedShip);
        }

        ship = playerObject.chosenShip;

        shipNameArt = ship.nameArt;
        shipBio.text = ship.bio;
        shipModel = ship.displayShip;

        instancedShip = Instantiate(shipModel, shipLocation.position, shipLocation.rotation, shipLocation);

    }

    public void UnloadSelectScreen(){
        SceneManager.UnloadSceneAsync("Select Scene");
    }
}
