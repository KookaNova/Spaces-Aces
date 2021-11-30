using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance = null;

    public GamemodeData chosenGamemode;
    
    string defaultScene = "Home";


    void Awake() {

        if(instance == null){
            instance = this;
            Debug.Log("SceneController: Awake(), instance set to " + this);
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this){
            Debug.Log("SceneController: Awake(), instance already set. Destroying " + this);
            Destroy(gameObject);
        }
    }
    
    public void LoadGamemode(GamemodeData newGamemode){
        chosenGamemode = newGamemode;
        LoadSpecificScene(chosenGamemode.levelName);
    }
    public void LoadSpecificScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadDefaultScene(){
        LoadSpecificScene(defaultScene);
    }
    
    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }
}