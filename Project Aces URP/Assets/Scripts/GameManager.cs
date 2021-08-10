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

    private void Start() {
        Instance = this; 
        if(playerPrefab == null){
            Debug.LogError("GameManager: playerPrefab is null",this);
        }
        else{
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
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
