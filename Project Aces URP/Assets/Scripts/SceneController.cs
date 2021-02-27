using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Tooltip("This array should be filled with any scene that can be loaded from this scene. The integer put into elements should correspond with the scene's number in Build Settings.")]
    [Header("Scene Manager")]
    public int[] scenes;

    [Header("Load Scene Events")]
    public UnityEvent levelLoadingEvent;
    public UnityEvent levelLoadingWhileEvent, levelLoadingWhile90PercentEvent;

    public AsyncOperation sceneLoad;

    public void LoadLevel(int SceneNumber)
    {
        StartCoroutine(asyncLoad(scenes[SceneNumber]));
    }

    private IEnumerator asyncLoad(int loadScene)
    {
        sceneLoad = SceneManager.LoadSceneAsync(loadScene);
        sceneLoad.allowSceneActivation = false;
        levelLoadingEvent.Invoke();
        
        //things to do while the level is loading
        while (!sceneLoad.isDone)
        {
            levelLoadingWhileEvent.Invoke();
            
            //things to do while the level is >90% loaded
            if (sceneLoad.progress >= 0.9f)
            {
                levelLoadingWhile90PercentEvent.Invoke();
                yield return new WaitForSeconds(.5f);
                sceneLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}