using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public GameObject playerPrefab;
    public GamemodeData currentGamemode;
    public Transform[] teamASpawnpoints, teamBSpawnpoints;
    public bool isSelectLoaded = false;

    private Scene mainScene;

    private void Start() {
        //currentGamemode = FindObjectOfType<SceneController>().chosenGamemode;
        Instance = this; 
        mainScene = SceneManager.GetActiveScene();
        OpenSelectMenu();

    }

    public void OpenSelectMenu(){
        SceneManager.LoadSceneAsync("Select Scene", LoadSceneMode.Additive);
        isSelectLoaded = true;
    }
     public void CloseSelectMenu(){
        isSelectLoaded = false;
        SceneManager.UnloadSceneAsync("Select Scene");
        SpawnPlayer();
        
    }

    public void SpawnPlayer(){
        if(playerPrefab == null){
            Debug.LogError("GameManager: playerPrefab is null", this);
            return;
        }
        if(!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom){
            Instantiate(this.playerPrefab, teamASpawnpoints[0].position, Quaternion.identity);
        }
        else{
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, teamASpawnpoints[0].position, Quaternion.identity, 0);
        }
    }

    public void CreateRoom(){
        PhotonNetwork.CreateRoom("tester");
        Instance = this;
    }

    public override void OnLeftRoom(){
        SceneManager.LoadScene("Main Menu");
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }
}
