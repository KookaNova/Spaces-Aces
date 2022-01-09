using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerLauncher : MonoBehaviourPunCallbacks
{
    //The multiplayer launcher should be in the main menu scene, and most functions accessible by pressing buttons.
    //This client's version number. Users are separated from each other by gameVersion (which allows breaking changes).
    string gameVersion = "0.23";

    
    #region UI Fields
    VisualElement root;
    MenuManager menuManager;

    Label versionLabel, serverLabel, connectingLabel, connectMessageLabel, gameStatusLabel;
    //Private Games UI
    string hostText, levelText, gamemodeText, maxPlayerText, isPublicText, privateStatusText;

    //The time it takes to start a game after StartGame is initiated.
    int countDownTime = 4;

    int teamA, teamB;

    #endregion

    #region Nameplates
    //Important for generating nameplates and placing them
    [Header("Nameplates")]
    [SerializeField] GameObject profilePrefab;
    //[SerializeField] Transform nameplateStartingPosition;
    //private List<GameObject> nameplates = new List<GameObject>();
    //private List<Player> connectedPlayers;

    #endregion

    public GamesHandler quickplay;
    private GamemodeData chosenMode;

    private GamesHandler gamesHandler;
    private RoomOptions roomOptions = new RoomOptions();

    #region Private Games
    private PrivateGameSettings privateGameSettings = new PrivateGameSettings();
    
    
    #endregion

    private void Awake() {
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically.
        PhotonNetwork.AutomaticallySyncScene = true;

        //Send info to UI easily
        root = GetComponent<UIDocument>().rootVisualElement;

        versionLabel = root.Q<Label>("VersionNumber");
        serverLabel = root.Q<Label>("ServerName");
        menuManager = root.Q<MenuManager>();

        connectingLabel = root.Q<Label>("ConnectingLabel");
        connectMessageLabel = root.Q<Label>("ConnectMessage");
        gameStatusLabel = root.Q<Label>("GameStatus");

        versionLabel.text = gameVersion;
        serverLabel.text = "Offline";

    }

    public void ReturnToMainMenu(){
    }
    #region Connecting to Server

    public void ConnectToServer(){
        //#Critical: we must first and foremost connect to Photon Online Server. We check if we are connected, else we connect.
        if(PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode){
            menuManager.EnableMultiplayerScreen();
        }
        else{
            var m_connectScreen = root.Q("ConnectingScreen");
            m_connectScreen.Q("Progress").style.width = new StyleLength(Length.Percent(0));
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.ConnectUsingSettings();
            menuManager.EnableConnectingScreen();
            connectingLabel.text = "Connecting...";
            
            /*var m = root.Q("MessageContainer");
            m.style.display = DisplayStyle.None;*/
        }
    }

    public override void OnConnectedToMaster(){
        serverLabel.text = PhotonNetwork.CloudRegion.ToString();
        var m_connectScreen = root.Q("ConnectingScreen");

        if(m_connectScreen.style.display == DisplayStyle.Flex){
            m_connectScreen.Q("Progress").style.width = new StyleLength(Length.Percent(100));
            m_connectScreen.Q("Progress").RegisterCallback<TransitionEndEvent>(ev => menuManager.EnableMultiplayerScreen());
            
        }

    }

    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        var m_connectScreen = root.Q("ConnectingScreen");
        m_connectScreen.Q("Progress").style.width = new StyleLength(Length.Percent(25));
    }

    public override void OnConnected()
    {
        var m_connectScreen = root.Q("ConnectingScreen");
        m_connectScreen.Q("Progress").style.width = new StyleLength(Length.Percent(75));
    }

    

    public override void OnDisconnected(DisconnectCause cause){
        Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);

        //UI
        var m = root.Q("MessageContainer");
        //menuManager.EnableSearchOverlay(false);
        //m.style.display = DisplayStyle.Flex;
        connectMessageLabel.text = cause.ToString();
        connectingLabel.text = ("Disconnected");
        serverLabel.text = "Offline";
    }

    #endregion

    #region Finding or Creating Rooms
    public void FindMatchFromPlaylist(GamesHandler selectedGamesHandler){
        gamesHandler = selectedGamesHandler;
        //Finds a random room using properties set by gamesHandler (essentially playlist). This allows different random search modes.
        //If no rooms are found, a room is created using the same properties.
        roomOptions.CustomRoomProperties = new Hashtable {{"MMT", gamesHandler.matchmakingType}};
        roomOptions.MaxPlayers = gamesHandler.expectedMaxPlayers;
        Debug.LogFormat("Launcher: FindMatchFromPlaylist(), searching for a random room with properties {0} and max players {1}", 
            roomOptions.CustomRoomProperties, gamesHandler.expectedMaxPlayers);
        PhotonNetwork.JoinRandomRoom(roomOptions.CustomRoomProperties, roomOptions.MaxPlayers);

        //UI
        menuManager.EnableMatchSearch();
        menuManager.EnableHome();
        gameStatusLabel.text = "Searching for a match...";
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
       
        Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
        
    }

    public override void OnCreatedRoom(){
        //The local player is labeled the master of the room.
        Player masterPlayer = PhotonNetwork.LocalPlayer;
        gameStatusLabel.text = "Waiting for players...";
        teamA = 0;
        teamB = 0;
        //When player count reaches max, start the game.
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            StartCoroutine(StartingCountdown());
        }
        PhotonNetwork.SetPlayerCustomProperties(new Hashtable() {{"Team", "A"}});
        quickplay.SelectRandomLevel();
        chosenMode = quickplay.gamemodeSettings;
    }

    #endregion

    #region Entering Rooms
    public override void OnJoinedRoom(){
        //creates player list when local player joins a room.
        CreatePlayerList();

        hostText = PhotonNetwork.MasterClient.ToString();
        maxPlayerText = roomOptions.MaxPlayers.ToString();
        levelText = (string)roomOptions.CustomRoomProperties["Level"];
        gamemodeText = (string)roomOptions.CustomRoomProperties["Gamemode"];
        isPublicText = roomOptions.IsVisible.ToString();

        
    }

    public void LeaveRoom(){
        StopAllCoroutines();
        if(PhotonNetwork.InRoom){
            PhotonNetwork.LeaveRoom(true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable(){{"Team", null}});
        }
        
        
        Debug.Log("Launcher: OnLeaveRoom() called by PUN. Client exits the room.");
        
        //nameplates.Clear();
        //UI
        menuManager.DisableMatchSearch();
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        //When player count reaches max, start the game.
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            StartCoroutine(StartingCountdown());
        }
        Debug.LogFormat("Launcher: OnPlayerEnteredRoom(), Player {0} entered room.", newPlayer);
        //Assign Team
        if(teamA >= teamB){
            newPlayer.SetCustomProperties(new Hashtable() {{"Team","B"}});
        }
        if(teamA < teamB){
            newPlayer.SetCustomProperties(new Hashtable() {{"Team","A"}});
        }
        

        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player leftPlayer){
        //When player count reaches max, start the game.
        if(PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers){
            StopAllCoroutines();
            gameStatusLabel.text = "Waiting for players...";
        }
        CountTeams();
        Debug.LogFormat("Launcher: OnPlayerEnteredRoom(), Player {0} left room.", leftPlayer);
        UpdatePlayerList();
    }

    private void CountTeams(){
        teamA = 0;
        teamB = 0;

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
            if((string)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == "A"){
                teamA++;
            }
            if((string)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == "B"){
                teamB++;
            }
        }
        Debug.Log(teamA + "/" + teamB);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        CountTeams();
        
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

        privateStatusText = "Waiting for host to start.";

        Debug.LogFormat("Launcher: CreateRoomWithPrivateSettings(), created room with name {0}, maxPlayers {1}, visibility {2}, on level {3}.", 
            privateGameSettings.roomName, roomOptions.MaxPlayers, roomOptions.IsVisible, privateGameSettings.nameOfLevel);

    }

    public void StartPrivateGame(){
        StartCoroutine(StartingCountdownPrivate());
    }

    #endregion

    #region PlayerList

    private void CreatePlayerList(){
        /*var nameplate = Instantiate(profilePrefab, nameplateStartingPosition.position, Quaternion.identity , playerListUI.transform);
        nameplates.Add(nameplate);
        var newProfileData = nameplate.GetComponent<FillPlayerData>();
        newProfileData.DisplayData();*/

    }

    private void UpdatePlayerList(){
        /*nameplates.Clear();

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
            var nameplate = Instantiate(profilePrefab, nameplateStartingPosition.position, Quaternion.identity);
            nameplates.Add(nameplate);
            //nameplate.transform.SetParent(playerListUI.transform);
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
        }*/
    }

    #endregion

    #region Starting Games
    public IEnumerator StartingCountdown(){
        Debug.Log("Starting Countdown...");
        gameStatusLabel.text = "Players found.";
        

        int timer = countDownTime;

        while (timer > 0){
            yield return new WaitForSecondsRealtime(1);
            timer--;
            gameStatusLabel.text = "Game starting in " + timer.ToString();
        }
        if(timer <= 0){
            gameStatusLabel.text = "Prepare for takeoff.";
        }

        Debug.LogFormat("Loading {0} players into level: {1}...", PhotonNetwork.CurrentRoom.PlayerCount, gamesHandler.gamemodeSettings.levelName);
        // #Critical: Load the Room Level.
        if(PhotonNetwork.IsMasterClient){
            FindObjectOfType<SceneController>().chosenGamemode = gamesHandler.gamemodeSettings;
            PhotonNetwork.LoadLevel(gamesHandler.gamemodeSettings.levelName);
        } 
    }

    public IEnumerator StartingCountdownPrivate(){
        Debug.Log("Starting Countdown...");
        privateStatusText = "Start game initiated.";
        

        int timer = countDownTime;

        while (timer > 0){
            yield return new WaitForSecondsRealtime(1);
            timer--;
            privateStatusText = "Game starting in " + timer.ToString();
        }
        if(timer <= 0){
            privateStatusText = "Prepare for takeoff.";
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
        gameStatusLabel.text = "Offline Mode";
        

        int timer = countDownTime;

        while (timer > 0){
            yield return new WaitForSecondsRealtime(1);
            timer--;
            gameStatusLabel.text = "Game starting in " + timer.ToString();
        }
        if(timer <= 0){
            gameStatusLabel.text = "Prepare for takeoff.";
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
