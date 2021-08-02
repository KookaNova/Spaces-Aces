using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Con.SpacesAcesGame{

public class Launcher : MonoBehaviourPunCallbacks
{   
    public GamesHandler gamesHandler;
    private GameObject matchmakingSearchUI;
    //This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    string gameVersion = "1";
    bool isConnecting;

    private void Awake() {
        //#Critical
        //this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region Public
    public void Matchmaking(){
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if(PhotonNetwork.IsConnected){
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else{
            // #Critical, we must first and foremost connect to Photon Online Server.
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        matchmakingSearchUI.SetActive(true);
    }

    public void LeaveRoom(){
        if(PhotonNetwork.IsConnected){
            //Allows us to disconnect from a room or search.
            PhotonNetwork.LeaveRoom();
        }
    }
    #endregion

    #region PUN

    public override void OnConnectedToMaster(){
        // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
        if(isConnecting){
        PhotonNetwork.JoinRandomRoom();
        isConnecting = false;
        }
    }
    public override void OnDisconnected(DisconnectCause cause){
        Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        gamesHandler.SelectRandomLevel();
        PhotonNetwork.CreateRoom(gamesHandler.gamemodeSettings.levelName, new RoomOptions{MaxPlayers = gamesHandler.gamemodeSettings.maxPlayers});
    }
    public override void OnJoinedRoom(){
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    
        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            Debug.Log("Loading ");


            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
}
    }


    #endregion
}
}
