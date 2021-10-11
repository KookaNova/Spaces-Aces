using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cox.PlayerControls;

public class SelectScreenBehaviour : MonoBehaviour
{
    public PlayerObject playerObject;
    
    [SerializeField] private Image characterNameArt, bodyArt, shipNameArt;
    [SerializeField] private Transform shipLocation;
    [SerializeField] private Text charBio, shipBio;

    private CharacterHandler character;
    private ShipHandler ship;

    private GameObject shipModel;
    private GameObject instancedShip = null;

    public void DisplayCharacterData(){
        character = playerObject.chosenCharacter;
        
        characterNameArt.sprite = character.nameArt;
        bodyArt.sprite = character.bodyArt;
        charBio.text = character.bio;
    }

    public void DisplayShipData(){
        if(instancedShip != null){
            Destroy(instancedShip);
        }

        ship = playerObject.chosenShip;

        shipNameArt.sprite = ship.nameArt;
        shipBio.text = ship.bio;
        shipModel = ship.displayShip;

        instancedShip = Instantiate(shipModel, shipLocation.position, shipLocation.rotation, shipLocation);

    }

    public void UnloadSelectScreen(){
        SceneManager.UnloadSceneAsync("Select Scene");
    }
}
