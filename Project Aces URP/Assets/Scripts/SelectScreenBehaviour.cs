using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectScreenBehaviour : MonoBehaviour
{
    public PlayerObject playerObject;
    
    [SerializeField]
    private Image nameArt, bodyArt;
    [SerializeField]
    private Text bio;

    private CharacterHandler character;

    public void UpdateDisplay(){
        character = playerObject.chosenCharacter;
        
        nameArt.sprite = character.nameArt;
        bodyArt.sprite = character.bodyArt;
        bio.text = character.bio;
    }

    public void UnloadSelectScreen(){
        SceneManager.UnloadSceneAsync("Select Scene");
    }
}
