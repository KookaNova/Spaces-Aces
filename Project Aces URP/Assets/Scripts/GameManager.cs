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

    private void Start() {
        currentGamemode = FindObjectOfType<SceneController>().chosenGamemode;
        Instance = this; 
        if(playerPrefab == null){
            Debug.LogError("GameManager: playerPrefab is null",this);
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
