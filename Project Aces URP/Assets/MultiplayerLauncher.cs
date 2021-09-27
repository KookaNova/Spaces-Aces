using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerLauncher : MonoBehaviourPunCallbacks
{
    //This client's version number. Users are separated from each other by gameVersion (which allows breaking changes).
    string gameVersion = "0.1";

    //The multiplayer launcher should be in the main menu scene, and most methods accessible by pressing buttons.
    #region Serialized Fields
    [Header("UI Menus")]
    [SerializeField] GameObject multiplayerMenu;
    [SerializeField] GameObject connectingUI, disconnectMessage, matchmakingMenu, matchmakingSearchUI, mainMenu, playerListUI;

    [Header("Text")]
    [SerializeField] Text versionText;
    [SerializeField] Text serverText, disconnectText, gameStartText;
    [SerializeField] int countDownTime = 4;

    #endregion

    #region Nameplates
    //Important for generating nameplates and placing them
    [Header("Nameplates")]
    [SerializeField] GameObject profilePrefab;
    [SerializeField] Transform nameplateStartingPosition;
    private List<GameObject> nameplates = new List<GameObject>();
    private List<Player> connectedPlayers;

    #endregion

    private GamesHandler gamesHandler;
    private RoomOptions roomOptions = new RoomOptions();


    private void Awake() {
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically.
        PhotonNetwork.AutomaticallySyncScene = true;
        versionText.text = gameVersion;
        serverText.text = "Offline";

        //Makes sure UI is off until it's used.
        connectingUI.SetActive(false); disconnectMessage.SetActive(false); matchmakingMenu.SetActive(false); 
        matchmakingSearchUI.SetActive(false); multiplayerMenu.SetActive(false); playerListUI.SetActive(false);
    }

    public void ReturnToMainMenu(){
        mainMenu.SetActive(true);
        connectingUI.SetActive(false); disconnectMessage.SetActive(false); matchmakingMenu.SetActive(false); 
        multiplayerMenu.SetActive(false);
    }

    #region Connecting to Server

    public void ConnectToServer(){
        //#Critical: we must first and foremost connect to Photon Online Server. We check if we are connected, else we connect.
        if(PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode){
            multiplayerMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else{
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.OfflineMode = false;
            connectingUI.SetActive(true);
            mainMenu.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster(){
        multiplayerMenu.SetActive(true);
        connectingUI.SetActive(false);
        serverText.text = PhotonNetwork.CloudRegion.ToString();
    }

    public override void OnDisconnected(DisconnectCause cause){
        Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        disconnectMessage.SetActive(true);
        disconnectText.text = cause.ToString();
        serverText.text = "Offline";

        if(connectingUI == null)return;
        connectingUI.SetActive(false); matchmakingSearchUI.SetActive(false); multiplayerMenu.SetActive(false); mainMenu.SetActive(false);

    }

    #endregion

    #region Finding or Creating Rooms
    public void FindMatchFromPlaylist(GamesHandler selectedGamesHandler){
        gamesHandler = selectedGamesHandler;
        //Finds a random room using properties set by gamesHandler (essentially playlist). This allows different random search modes.
        //If no rooms are found, a room is created using the same properties.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { {"MMT", gamesHandler.matchmakingType}};
        roomOptions.MaxPlayers = gamesHandler.expectedMaxPlayers;
        Debug.LogFormat("Launcher: FindMatchFromPlaylist(), searching for a random room with properties {0} and max players {1}", roomOptions.CustomRoomProperties, gamesHandler.expectedMaxPlayers);
        PhotonNetwork.JoinRandomRoom(roomOptions.CustomRoomProperties, roomOptions.MaxPlayers);
        matchmakingSearchUI.SetActive(true);
        gameStartText.text = "Searching for a match...";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat("Launcher: OnJoinRandomFailed(), No random room available with properties {0} and max players {1}, creating new room", 
        roomOptions.CustomRoomProperties, roomOptions.MaxPlayers);
        //We make a room using the same roomOptions from the random search.
        PhotonNetwork.CreateRoom(null, roomOptions, null, null);


        //RoomOptions contain CustomRoomProperties that we make, properties of it's own like MaxPlayers that we can change, and properties that display in lobby.
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        matchmakingSearchUI.SetActive(false);
        if(matchmakingMenu.activeSelf == true){
            matchmakingMenu.SetActive(false);
            multiplayerMenu.SetActive(true);
        }

        Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
        
    }

    public override void OnCreatedRoom(){
        //The local player is labeled the master of the room.
        Player masterPlayer = PhotonNetwork.LocalPlayer;
        gameStartText.text = "Waiting for players...";
        Debug.Log("Launcher: OnCreatedRoom(), visibility of current room is " + PhotonNetwork.CurrentRoom.IsVisible);

        //When player count reaches max, start the game.
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            StartCoroutine(StartingCountdown());
        }
        
    }

    #endregion

    #region Entering Rooms
    public override void OnJoinedRoom(){
        //creates player list when local player joins a room.
        CreatePlayerList();
    }

    public void LeaveRoom(){
        StopAllCoroutines();
        if(PhotonNetwork.InRoom){
            PhotonNetwork.LeaveRoom(true);
        }
        
        Debug.Log("Launcher: OnLeaveRoom() called by PUN. Client exits the room.");
        
        nameplates.Clear();
        gameStartText.text = "Waiting for players...";
        
        playerListUI.SetActive(false);
        matchmakingSearchUI.SetActive(false);
        if(matchmakingMenu.activeInHierarchy){
            matchmakingMenu.SetActive(false);
            multiplayerMenu.SetActive(true);
        }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        //When player count reaches max, start the game.
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            StartCoroutine(StartingCountdown());
        }
        Debug.LogFormat("Launcher: OnPlayerEnteredRoom(), Player {0} entered room.", newPlayer);
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player leftPlayer){
        //When player count reaches max, start the game.
        if(PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers){
            StopAllCoroutines();
            gameStartText.text = "Waiting for players...";
        }
        Debug.LogFormat("Launcher: OnPlayerEnteredRoom(), Player {0} left room.", leftPlayer);
        UpdatePlayerList();
    }

    #endregion

    #region PlayerList

    private void CreatePlayerList(){
        playerListUI.SetActive(true);
        var nameplate = Instantiate(profilePrefab, nameplateStartingPosition.position, Quaternion.identity, playerListUI.transform);
        nameplates.Add(nameplate);
        var newProfileData = nameplate.GetComponent<FillPlayerData>();
        newProfileData.DisplayData();

    }

    private void UpdatePlayerList(){
        playerListUI.SetActive(true);
        nameplates.Clear();

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
            var nameplate = Instantiate(profilePrefab, nameplateStartingPosition.position, Quaternion.identity);
            nameplates.Add(nameplate);
            nameplate.transform.SetParent(playerListUI.transform);
            nameplate.transform.Translate(0, -i *20, 0);

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

    #endregion

    public IEnumerator StartingCountdown(){
        Debug.Log("Starting Countdown...");
        gameStartText.text = "Players found.";
        

        int timer = countDownTime;

        while (timer > 0){
            yield return new WaitForSecondsRealtime(1);
            timer--;
            gameStartText.text = "Game starting in " + timer.ToString();
        }
        if(timer <= 0){
            gameStartText.text = "Prepare for takeoff.";
        }
        //Selects the level from the playlist
        gamesHandler.SelectRandomLevel();

        Debug.LogFormat("Loading {0} players into level: {1}...", PhotonNetwork.CurrentRoom.PlayerCount, gamesHandler.gamemodeSettings.levelName);
        // #Critical: Load the Room Level.
        if(PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(gamesHandler.gamemodeSettings.levelName);
    }

    public void StartOfflineGame(GamemodeData gamemodeData){
        StartCoroutine(StartingCountdownOffline(gamemodeData));
    }

    private IEnumerator StartingCountdownOffline(GamemodeData gamemodeData){
        Debug.Log("Starting Offline Countdown...");
        matchmakingSearchUI.SetActive(true);
        gameStartText.text = "Offline Mode";
        

        int timer = countDownTime;

        while (timer > 0){
            yield return new WaitForSecondsRealtime(1);
            timer--;
            gameStartText.text = "Game starting in " + timer.ToString();
        }
        if(timer <= 0){
            gameStartText.text = "Prepare for takeoff.";
        }

        // #Critical: Load the Room Level.
        PhotonNetwork.LoadLevel(gamemodeData.levelName);
    }

}
