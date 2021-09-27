using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System;

public class MultiplayerLauncher : MonoBehaviourPunCallbacks
{
    //This client's version number. Users are separated from each other by gameVersion (which allows breaking changes).
    string gameVersion = "0.1";

    //The multiplayer launcher should be in the main menu scene, and most methods accessible by pressing buttons.
    #region Serialized Fields
    [Header("UI Menus")]
    [SerializeField] GameObject multiplayerMenu;
    [SerializeField] GameObject playChoiceButtons, connectingUI, disconnectMessage, matchmakingMenu, matchmakingSearchUI, mainMenu, playerListUI;

    [Header("Text")]
    [SerializeField] Text versionText;
    [SerializeField] Text serverText, disconnectText, gameStartText;
    
    [Header("Private Games")]
    [SerializeField] GameObject privateRoom;
    [SerializeField] Button editRoom, startPrivateGame;
    [SerializeField] Text hostText, levelText, gamemodeText, maxPlayerText, isPublicText, privateStatusText;

    //The time it takes to start a game after StartGame is initiated.
    int countDownTime = 4;

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

    #region Private Games
    private PrivateGameSettings privateGameSettings = new PrivateGameSettings();


    
    #endregion

    private void Awake() {
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically.
        PhotonNetwork.AutomaticallySyncScene = true;
        versionText.text = gameVersion;
        serverText.text = "Offline";

        //Makes sure UI is off until it's used.
        connectingUI.SetActive(false); disconnectMessage.SetActive(false); matchmakingMenu.SetActive(false); playChoiceButtons.SetActive(true);
        matchmakingSearchUI.SetActive(false); multiplayerMenu.SetActive(false); playerListUI.SetActive(false); privateRoom.SetActive(false);

        editRoom.interactable = false; startPrivateGame.interactable = false;
    }

    public void ReturnToMainMenu(){
        mainMenu.SetActive(true);
        connectingUI.SetActive(false); disconnectMessage.SetActive(false); matchmakingMenu.SetActive(false); 
        multiplayerMenu.SetActive(false); privateRoom.SetActive(false);
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
        playChoiceButtons.SetActive(true);
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
        editRoom.interactable = false; startPrivateGame.interactable = false; privateRoom.SetActive(false); playChoiceButtons.SetActive(true);

    }

    #endregion

    #region Finding or Creating Rooms
    public void FindMatchFromPlaylist(GamesHandler selectedGamesHandler){
        gamesHandler = selectedGamesHandler;
        //Finds a random room using properties set by gamesHandler (essentially playlist). This allows different random search modes.
        //If no rooms are found, a room is created using the same properties.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {{"MMT", gamesHandler.matchmakingType}};
        roomOptions.MaxPlayers = gamesHandler.expectedMaxPlayers;
        Debug.LogFormat("Launcher: FindMatchFromPlaylist(), searching for a random room with properties {0} and max players {1}", 
            roomOptions.CustomRoomProperties, gamesHandler.expectedMaxPlayers);
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
            privateRoom.SetActive(false);
            multiplayerMenu.SetActive(true);
        }

        Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
        
    }

    public override void OnCreatedRoom(){
        //The local player is labeled the master of the room.
        Player masterPlayer = PhotonNetwork.LocalPlayer;
        gameStartText.text = "Waiting for players...";

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
        if(PhotonNetwork.IsMasterClient){
            editRoom.interactable = true;
            startPrivateGame.interactable = true;
        }

        hostText.text = PhotonNetwork.MasterClient.ToString();
        maxPlayerText.text = roomOptions.MaxPlayers.ToString();
        levelText.text = (string)roomOptions.CustomRoomProperties["Level"];
        gamemodeText.text = (string)roomOptions.CustomRoomProperties["Gamemode"];
        isPublicText.text = roomOptions.IsVisible.ToString();
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
        privateRoom.SetActive(false);
        playChoiceButtons.SetActive(true);
        matchmakingSearchUI.SetActive(false);
        if(matchmakingMenu.activeInHierarchy){
            matchmakingMenu.SetActive(false);
            multiplayerMenu.SetActive(true);
        }
        if(PhotonNetwork.IsMasterClient){
            editRoom.interactable = false;
            startPrivateGame.interactable = false;
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

    #region Private Games

    public void ChangePrivateSettings(string levelName){
        privateGameSettings.nameOfLevel = levelName;

    }

    public void ChangePrivateSettings(GamemodeData gamemode){
        privateGameSettings.gamemodeData = gamemode;
    }

    public void ChangePrivateSettings(Single setMaxPlayers){
        privateGameSettings.maxPlayers = ((byte)setMaxPlayers);
    }

    public void ChangePrivateSettings(bool setVisible){
        privateGameSettings.isVisible = setVisible;
    }

    public void SetRoomName(string newName){
        privateGameSettings.roomName = newName;
    }

    public void CreateRoomWithPrivateSettings(){
        //We take our private games settings and fill them into a custom room
        roomOptions.MaxPlayers = privateGameSettings.maxPlayers;
        roomOptions.IsVisible = privateGameSettings.isVisible;

        

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable{{"MMT", "Custom Game"}, {"Level", privateGameSettings.nameOfLevel}, {"Gamemode", "Attack" /*privateGameSettings.gamemodeData*/}};
        PhotonNetwork.CreateRoom(privateGameSettings.roomName, roomOptions);

        privateRoom.SetActive(true);
        privateStatusText.text = "Waiting for host to start.";

        Debug.LogFormat("Launcher: CreateRoomWithPrivateSettings(), created room with name {0}, maxPlayers {1}, visibility {2}, on level {3}.", 
            privateGameSettings.roomName, roomOptions.MaxPlayers, roomOptions.IsVisible, privateGameSettings.nameOfLevel);

    }

    public void StartPrivateGame(){
        StartCoroutine(StartingCountdownPrivate());
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

    #region Starting Games
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

    public IEnumerator StartingCountdownPrivate(){
        Debug.Log("Starting Countdown...");
        privateStatusText.text = "Start game initiated.";
        

        int timer = countDownTime;

        while (timer > 0){
            yield return new WaitForSecondsRealtime(1);
            timer--;
            privateStatusText.text = "Game starting in " + timer.ToString();
        }
        if(timer <= 0){
            privateStatusText.text = "Prepare for takeoff.";
        }

        Debug.LogFormat("Loading {0} players into level: {1}...", PhotonNetwork.CurrentRoom.PlayerCount, privateGameSettings.nameOfLevel);
        // #Critical: Load the Room Level.
        if(PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(privateGameSettings.nameOfLevel);
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



    #endregion



}
/// <summary> Settings used to create private games. </summary>
public class PrivateGameSettings{
    public string roomName;

    public string nameOfLevel;
    public GamemodeData gamemodeData;

    public byte maxPlayers = 6;
    public bool isVisible = true;

}
