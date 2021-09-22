using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

namespace Com.Con.SpacesAcesGame{

public class PrivateGamesLauncher : MonoBehaviourPunCallbacks
{   
    public GamesHandler quickPlayGamesHandler;
    [SerializeField]
    private GameObject matchmakingSearchUI, playerListUI, profilePrefab;
    [SerializeField]
    private Transform[] nameSlots;
    [SerializeField]
    private List<GameObject> nameplates;
    [SerializeField]
    private List<Player> connectedPlayers;
    bool isConnectingRandom;
    [SerializeField]
    private GamemodeData chosenGamemode;
    
    //This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    string gameVersion = "1";

    private void Awake() {
        //this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region Public
    public void Matchmaking(){
        // we check if we are connected, else we connect.
        if(PhotonNetwork.IsConnected){
            // We attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else{
            // #Critical, we must first and foremost connect to Photon Online Server.
            isConnectingRandom = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        matchmakingSearchUI.SetActive(true);
    }

    public void CreatePrivateRoom(GamemodeData chosenGamemode){
         // we check if we are connected, else we connect.
        if(PhotonNetwork.IsConnected){
            // We attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.CreateRoom(chosenGamemode.levelName, new RoomOptions{MaxPlayers = chosenGamemode.maxPlayers} );
        }
        else{
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            CreatePrivateRoom(chosenGamemode);
        }
    }

    public void OnLeaveRoom(){
        if(PhotonNetwork.IsConnected){
            PhotonNetwork.LeaveRoom(true);
            playerListUI.SetActive(false);
            Debug.Log("Launcher: OnLeaveRoom() called by PUN. Client exits the room.");
        }
    }
    #endregion

    #region PUN

    public override void OnConnectedToMaster(){
        // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
        if(isConnectingRandom){
            //will callback OnJoinedRoom() or OnJoinRoomFailed().
            PhotonNetwork.JoinRandomRoom();
            isConnectingRandom = false;
        }
    }
    public override void OnDisconnected(DisconnectCause cause){
        Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        quickPlayGamesHandler.SelectRandomLevel();
        PhotonNetwork.CreateRoom(quickPlayGamesHandler.gamemodeSettings.levelName, new RoomOptions{MaxPlayers = quickPlayGamesHandler.gamemodeSettings.maxPlayers});

    }
    public override void OnJoinedRoom(){
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        playerListUI.SetActive(true);
        nameplates.Clear();

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
            var nameplate = PhotonNetwork.Instantiate(profilePrefab.name, nameSlots[i].position, Quaternion.identity);
            nameplates.Add(nameplate);
            nameplate.transform.SetParent(playerListUI.transform);

            nameplate.GetComponent<FillPlayerData>().FillOnlineData(
            (string)PhotonNetwork.PlayerList[i].CustomProperties["Name"], 
            (int)PhotonNetwork.PlayerList[i].CustomProperties["Level"], 
            (float)PhotonNetwork.PlayerList[i].CustomProperties["Name Hue"],
            (int)PhotonNetwork.PlayerList[i].CustomProperties["Nameplate Art"],
            (int)PhotonNetwork.PlayerList[i].CustomProperties["Emblem Primary"],
            (int)PhotonNetwork.PlayerList[i].CustomProperties["Emblem Background"],
            (float)PhotonNetwork.PlayerList[i].CustomProperties["Emblem Primary Hue"],
            (float)PhotonNetwork.PlayerList[i].CustomProperties["Emblem Background Hue"]);


        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        var newNameplate = PhotonNetwork.Instantiate(profilePrefab.name, nameSlots[PhotonNetwork.PlayerList.Length - 1].position, Quaternion.identity);
        nameplates.Add(newNameplate);

        newNameplate.transform.SetParent(playerListUI.transform);
        
        newNameplate.GetComponent<FillPlayerData>().FillOnlineData(
            (string)newPlayer.CustomProperties["Name"], 
            (int)newPlayer.CustomProperties["Level"], 
            (float)newPlayer.CustomProperties["Name Hue"],
            (int)newPlayer.CustomProperties["Nameplate Art"],
            (int)newPlayer.CustomProperties["Emblem Primary"],
            (int)newPlayer.CustomProperties["Emblem Background"],
            (float)newPlayer.CustomProperties["Emblem Primary Hue"],
            (float)newPlayer.CustomProperties["Emblem Background Hue"]
        );

        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            StartCoroutine(AllPlayersJoined());
        }

    }

    public override void OnPlayerLeftRoom(Player leftPlayer){
        for(int i = 0; i < nameplates.Count; i++){
            Destroy(nameplates[i]);
        }
        nameplates.Clear();
        StopCoroutine(AllPlayersJoined());

        for(int i = 0; i < PhotonNetwork.PlayerList.Length-1; i++){
            var nameplate = PhotonNetwork.Instantiate(profilePrefab.name, nameSlots[i].position, Quaternion.identity);
            nameplates.Add(nameplate);
            nameplate.transform.SetParent(playerListUI.transform);
        
            nameplates[i].GetComponent<FillPlayerData>().FillOnlineData(
            (string)connectedPlayers[i].CustomProperties["Name"], 
            (int)connectedPlayers[i].CustomProperties["Level"], 
            (float)connectedPlayers[i].CustomProperties["Name Hue"],
            (int)connectedPlayers[i].CustomProperties["Nameplate Art"],
            (int)connectedPlayers[i].CustomProperties["Emblem Primary"],
            (int)connectedPlayers[i].CustomProperties["Emblem Background"],
            (float)connectedPlayers[i].CustomProperties["Emblem Primary Hue"],
            (float)connectedPlayers[i].CustomProperties["Emblem Background Hue"]
        );
        }
    }

    public override void OnCreatedRoom(){

        Player masterPlayer = PhotonNetwork.LocalPlayer;
        Debug.Log("Launcher: OnCreatedRoom() loading master nameplate into slot 0");
        var nameplate = PhotonNetwork.InstantiateRoomObject(profilePrefab.name, nameSlots[0].position, Quaternion.identity);
        nameplate.transform.SetParent(playerListUI.transform);
        var newProfileData = nameplate.GetComponent<FillPlayerData>();
        newProfileData.DisplayData();
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            Debug.Log("Loading...");
            // #Critical
            // Load the Room Level.
            if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
        }

    }


    public IEnumerator AllPlayersJoined(){
        Debug.Log("Starting Countdown...");


        yield return new WaitForSecondsRealtime(6);
        Debug.Log("Loading...");
            // #Critical
            // Load the Room Level.
            if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
    }
    #endregion
}}
