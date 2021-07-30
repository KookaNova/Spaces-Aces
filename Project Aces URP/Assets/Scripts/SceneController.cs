using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string defaultScene = "Main Menu";

    private void Start() {
        LoadDefaultScene();
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
        Application.Quit();
    }
}