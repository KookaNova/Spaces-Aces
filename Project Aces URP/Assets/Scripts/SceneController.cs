using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    string defaultScene = "Main Menu";
    
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