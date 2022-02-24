using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerLauncher : MonoBehaviourPunCallbacks
{
    //The multiplayer launcher should be in the main menu scene, and most functions accessible by pressing buttons.
    //This client's version number. Users are separated from each other by gameVersion (which allows breaking changes).
    string gameVersion = "0.34";

    
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
    ProfileHandler profile;
    [SerializeField] GameObject profilePrefab;
    //[SerializeField] Transform nameplateStartingPosition;
    //private List<GameObject> nameplates = new List<GameObject>();
    //private List<Player> connectedPlayers;

    #endregion

    public GamesHandler quickplay;
    private GamemodeData chosenMode;
    private GamesHandler gamesHandler;
    Hashtable customProperties;

    #region Private Games
    private PrivateGameSettings privateGameSettings = new PrivateGameSettings();
    
    
    #endregion
    private void Awake() {
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically.
        PhotonNetwork.AutomaticallySyncScene = true;

        //Send info to UI easily
        root = GetComponent<UIDocument>().rootVisualElement;
        profile = FindObjectOfType<ProfileHandler>();

        versionLabel = root.Q<Label>("VersionNumber");
        serverLabel = root.Q<Label>("ServerName");
        menuManager = root.Q<MenuManager>();

        connectingLabel = root.Q<Label>("ConnectingLabel");
        connectMessageLabel = root.Q<Label>("ConnectMessage");
        gameStatusLabel = root.Q<Label>("GameStatus");

        versionLabel.text = gameVersion;
        if(PhotonNetwork.IsConnected){
            serverLabel.text = PhotonNetwork.CloudRegion.ToString();
        }
        else{
            serverLabel.text = "Offline";
        }
        if(PhotonNetwork.InRoom){
            StartCoroutine(PostGameCheck());
        }
        

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
        
        var m_connectScreen = root.Q("ConnectingScreen");
        m_connectScreen.style.display = DisplayStyle.Flex;
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
        customProperties = new Hashtable {{"m", "a"}};
        //roomOptions.MaxPlayers = gamesHandler.expectedMaxPlayers;

        PhotonNetwork.JoinRandomRoom();
        Debug.LogFormat("Launcher: FindMatchFromPlaylist(), searching for a random room with properties {0} and max players {1}", 
            customProperties, gamesHandler.expectedMaxPlayers);
        //ui
        menuManager.EnableMatchSearch();
        menuManager.EnableHome();
        gameStatusLabel.text = "Searching for a match...";  
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat("OnJoinRandomFailed(): reason {0}, message{1}", returnCode, message);
        /*Debug.LogFormat("Launcher: OnJoinRandomFailed(), No random room available with properties {0} and max players {1}, creating new room", 
        customProperties, gamesHandler.expectedMaxPlayers);*/
        //We make a room using the same roomOptions from the random search.
        RoomOptions ro = new RoomOptions();
        //ro.CustomRoomProperties = customProperties;
        ro.MaxPlayers = gamesHandler.expectedMaxPlayers;
        PhotonNetwork.CreateRoom(null, ro, null, null);


        //RoomOptions contain CustomRoomProperties that we make, properties of it's own like MaxPlayers that we can change, and properties that display in lobby.
    }

    public override void OnCreateRoomFailed(short returnCode, string message){
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

        Debug.LogFormat("Created new room with properties {0} and max players {1}", 
        PhotonNetwork.CurrentRoom.CustomProperties, gamesHandler.expectedMaxPlayers);
    }

    #endregion

    #region Entering Rooms
    public override void OnJoinedRoom(){
        //creates player list when local player joins a room.
        CreatePlayerList();

        hostText = PhotonNetwork.MasterClient.ToString();
        maxPlayerText = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        levelText = (string)PhotonNetwork.CurrentRoom.CustomProperties["Level"];
        gamemodeText = (string)PhotonNetwork.CurrentRoom.CustomProperties["Gamemode"];
        isPublicText = PhotonNetwork.CurrentRoom.IsVisible.ToString();
        Debug.Log("Players: " + PhotonNetwork.CurrentRoom.PlayerCount);

        
    }

    public void LeaveRoom(){
        StopAllCoroutines();
        if(PhotonNetwork.InRoom){
            PhotonNetwork.LeaveRoom(true);
        }
        
        
        Debug.Log("Launcher: OnLeaveRoom() called by PUN. Client exits the room.");
        
        //nameplates.Clear();
        //UI
        menuManager.DisableMatchSearch();
        
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.SetPlayerCustomProperties(new Hashtable(){{"Team", null}});
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

        Debug.Log("Players: " + PhotonNetwork.CurrentRoom.PlayerCount);
        

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
        RoomOptions roomOptions = new RoomOptions();
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
    private IEnumerator PostGameCheck(){
        yield return new WaitForEndOfFrame();
        if(PhotonNetwork.InRoom){
            menuManager.EnablePostGame();
            StartCoroutine(PostGameDataFill());
        }
    }

    //Fill Post Game info
    private IEnumerator PostGameDataFill(){
        profile.LoadInfo();
        yield return new WaitForEndOfFrame();
        
        menuManager.m_PostGame.Q<Label>("Level").text = profile.currentLevel.ToString();
        menuManager.m_PostGame.Q<Label>("NextLevel").text = (profile.currentLevel + 1).ToString();

        //set XP bar
        Debug.Log(profile.currentXp);
        float dec1 = profile.currentXp;
        dec1 = dec1/profile.levelUpPoint;
        Debug.Log("dec1: " + dec1);
        dec1 = dec1 * 100;
        Debug.Log("dec1 * 100: " + dec1);

        menuManager.m_PostGame.Q("XPBar").style.width = new StyleLength(Length.Percent(dec1));

        //display performance data
        int score = (int)PhotonNetwork.LocalPlayer.CustomProperties["Score"];
        int kills = (int)PhotonNetwork.LocalPlayer.CustomProperties["Kills"];
        int deaths = (int)PhotonNetwork.LocalPlayer.CustomProperties["Deaths"];
        int seconds = (int)PhotonNetwork.LocalPlayer.CustomProperties["ElapsedTime"];
        bool isWin = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isWin"];
        profile.AddPerformanceData(score, kills, deaths, seconds, isWin);
        
        int h_score = 0;
        int h_kills = 0;
        int h_deaths = 0;
        int h_seconds = 0;

        while(h_score < score){
            h_score++;
            menuManager.m_PostGame.Q<Label>("Score").text = score.ToString();
            yield return new WaitForSecondsRealtime(0.05f);
        }
        while(h_kills < kills){
            h_kills++;
            menuManager.m_PostGame.Q<Label>("Kills").text = score.ToString();
            yield return new WaitForSecondsRealtime(0.05f);
        }
        while(h_deaths < deaths){
            h_deaths++;
            menuManager.m_PostGame.Q<Label>("Deaths").text = score.ToString();
            yield return new WaitForSecondsRealtime(0.05f);
        }
        while(h_seconds < seconds){
            h_seconds++;
            int min = Mathf.CeilToInt(h_seconds/60);
            int sec = h_seconds%60;
            menuManager.m_PostGame.Q<Label>("GameTime").text = min.ToString("0") + ":" + sec.ToString("0#");
            yield return new WaitForSecondsRealtime(0.005f);
        }

        //display Lifetime Data
        menuManager.m_PostGame.Q<Label>("Games").text = profile.totalGames.ToString();
        menuManager.m_PostGame.Q<Label>("Wins").text = profile.totalWins.ToString();
        menuManager.m_PostGame.Q<Label>("TotalKills").text = profile.totalKills.ToString();

        int minutes = Mathf.CeilToInt(profile.totalSeconds/60);
        int newSec = profile.totalSeconds%60;
        menuManager.m_PostGame.Q<Label>("TotalTime").text = minutes.ToString("0") + ":" + newSec.ToString("0#");

        //experience bar
        int newXP = seconds;
        if(isWin){
            newXP += 200;
        }
        for(int i = 0; i < score; i++){
            newXP += 50;
        }
        float dec2 = profile.currentXp;
        dec2 += newXP;
        dec2 = dec2/profile.levelUpPoint;
        dec2 = dec2 * 100;
        profile.AddExp(newXP);

        Debug.Log(dec1 + "/" + dec2);

        yield return new WaitForEndOfFrame();

        menuManager.m_PostGame.Q("XPBar").style.width = new StyleLength(Length.Percent(dec2));
    }
    /*private IEnumerator FillXP(){
        
    }*/



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
