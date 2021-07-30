using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GameController : MonoBehaviour
{
    private SceneController sceneController;
    public void Awake(){
        sceneController = FindObjectOfType<SceneController>();
    }
    public void Host(string lobbyScene){
        NetworkManager.Singleton.StartHost();
        LoadGame(lobbyScene);
    }
    public void Server(){
        NetworkManager.Singleton.StartServer();
    }
    public void Client(){
        NetworkManager.Singleton.StartClient();
    }

    public void LoadGame(string sceneName){
        sceneController.LoadSpecificScene(sceneName);
    }

    public void LoadDefaultScene(){
        sceneController.LoadDefaultScene();
    }
}
