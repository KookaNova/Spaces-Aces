using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Cox.PlayerControls;
using UnityEngine.UIElements;
using System.Collections;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Fields
    public static GameManager Instance; //used to keep data persistant if necessary, may not become used

    [SerializeField] GameObject playerPrefab; //#CRITICAL: this is the player
    [SerializeField] GameObject aiPrefab; //required if bots are used in place of players
    public GamemodeData currentGamemode; //#CRITICAL: the rules for the game. Set before the game starts by the scene manager
    public Transform[] teamASpawnpoints, teamBSpawnpoints; //#CRITICAL: required for spawning players.
    [SerializeField] int aiPlayerCount = 0;

    [HideInInspector] public bool isSelectLoaded = false; //prevents player from spawning before making a character selection
    SceneController sceneController; //used for loading the current gamemode, and allowing a player to return to the menu at any given time.

    public List<TargetableObject> allTargets;
    

    #region Gamemode Fields
    int gameTimer = 600;
    int teamAScore, teamBScore;
    int playersA = 0, playersB = 0;
    int timeOut = 45, startCount = 3;
    bool gameStarted = false, gameOver = false;
    #endregion

    //UI
    VisualElement root, feed, subtitle, tabScreen;
    [SerializeField] UIDocument uIDocument;

    #endregion

    #region GameStart
    private void Start() {
        sceneController = FindObjectOfType<SceneController>();
        if(sceneController != null){
            currentGamemode = FindObjectOfType<SceneController>().chosenGamemode;
        }
        
        root = uIDocument.rootVisualElement;
        feed = root.Q("EventFeed");
        subtitle = root.Q("Subtitle");
        tabScreen = root.Q("TabScreen");
        gameTimer = currentGamemode.timeLimit;
        aiPlayerCount = currentGamemode.AIPlayers;

        Instance = this;
        var targets = FindObjectsOfType<TargetableObject>();
        for(int i = 0; i < targets.Length; i++){
            allTargets.Add(targets[i]);
        }
        
        if(!PhotonNetwork.IsConnected){
            //allows us to play in a level even when we're offline. We switch to offline mode and create an offline room.
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom(null, null);
        }

        if(PhotonNetwork.LocalPlayer.CustomProperties["Team"] == null || (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] != 0 && (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] != 1){
            PhotonNetwork.SetPlayerCustomProperties(new Hashtable(){{"Team", 0}});
            playersA++;
        }
        Debug.Log("Team is " + (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"]);

        OpenSelectMenu();
        StartCoroutine(StartCheck());
    }

    public void OpenSelectMenu(){
        SceneManager.LoadSceneAsync("Select Scene", LoadSceneMode.Additive);
        uIDocument.sortingOrder = -1;
        isSelectLoaded = true;
    }

    public void CloseSelectMenu(){
        isSelectLoaded = false;
        SceneManager.UnloadSceneAsync("Select Scene");
        uIDocument.sortingOrder = 1;
        root.Q<GameUIManager>().EnableMainScreen();
        SpawnPlayer();
    }

    public void SpawnPlayer(){
        if(playerPrefab == null){
            Debug.LogError("GameManager: SpawnPlayer(), playerPrefab is null. Place a prefab in the inspector.", this);
            return;
        }
        if(!gameStarted)return;

        if(isSelectLoaded)return;
        if(PhotonNetwork.LocalPlayer.CustomProperties["Team"] == null){
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable(){{"Team", 0}});
        }

        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        if((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 0){
            
            int spawnPoint = Random.Range(0, teamASpawnpoints.Length);
            var p = PhotonNetwork.Instantiate(this.playerPrefab.name, teamASpawnpoints[playersA].position, teamASpawnpoints[playersA].rotation, 0);
            p.GetComponentInChildren<SpacecraftController>().Activate();
            playersA++;
            Debug.LogFormat("GameManager: SpawnPlayer(), Spawned player {0} at {1}.", p, spawnPoint);
        }
        else{
            int spawnPoint = Random.Range(0, teamBSpawnpoints.Length);
            var p = PhotonNetwork.Instantiate(this.playerPrefab.name, teamBSpawnpoints[playersB].position, teamASpawnpoints[playersA].rotation, 0);
            p.GetComponentInChildren<SpacecraftController>().Activate();
            playersB++;
            Debug.LogFormat("GameManager: SpawnPlayer(), Spawned player {0} at {1}.", p, spawnPoint);
        }
    }

    [PunRPC]
    public void AddTarget(int targetID, int team){
        var obj = PhotonView.Find(targetID).gameObject;
        TargetableObject target = obj.GetComponentInChildren<TargetableObject>();
        target.targetTeam = team;
        allTargets.Add(target);
    }
    [PunRPC]
    public void RemoveTarget(int index){
        allTargets.RemoveAt(index);
        allTargets.TrimExcess();

    }

    private IEnumerator StartCheck(){
        yield return new WaitForSeconds(1);
        timeOut--;
        root.Q<Label>("GameTimer").text = "Waiting for players.";
        if(timeOut <= 0){
            CancelGame();
        }
        if(PhotonNetwork.PlayerList.Length >= currentGamemode.playerCount){
            if(PhotonNetwork.IsMasterClient){
                photonView.RPC("StartCountDown", RpcTarget.All, null);
                //StartCoroutine(StartCountDown());
                root.Q<Label>("GameTimer").text = startCount.ToString();
            }
            
        }
        else{
            StartCoroutine(StartCheck());
        }
    }

    [PunRPC]
    private IEnumerator StartCountDown(){
        yield return new WaitForSecondsRealtime(1);
        startCount--;
        root.Q<Label>("GameTimer").text = "Game starts in " + startCount.ToString();
        if(startCount <= 0){
            if(PhotonNetwork.IsMasterClient){
                photonView.RPC("StartGame", RpcTarget.All);

            }
            //StartGame();
        }
        else{
            StartCoroutine(StartCountDown());
        }
    }
    
    [PunRPC]
    private void StartGame(){
        Debug.Log(allTargets.Count);
        gameStarted = true;
        photonView.RPC("GameTimer", RpcTarget.AllBuffered);
        //StartCoroutine(GameTimer());
        SpawnPlayer();

        //Only spawn in AI if we're the master client. This prevents duplicates from other clients instantiating AI over the network.
        if(PhotonNetwork.IsMasterClient){
            if(aiPrefab != null){
                for(int i = 0; i < aiPlayerCount; i++){
                    if(playersA < playersB){
                        int spawnPoint = Random.Range(0, teamASpawnpoints.Length);
                        var p = PhotonNetwork.Instantiate(this.aiPrefab.name, teamASpawnpoints[playersA].position, Quaternion.identity, 0);
                        var controller = p.GetComponentInChildren<SpacecraftController>();
                        controller.teamInt = 0;
                        controller.name = "AI" + i.ToString();
                        p.GetComponentInChildren<SpacecraftController>().photonView.RPC("Activate", RpcTarget.All);
                        UpdateScoreBoard(controller);
                        playersA++;
                    }
                    else{
                        int spawnPoint = Random.Range(0, teamBSpawnpoints.Length);
                        var p = PhotonNetwork.Instantiate(this.aiPrefab.name, teamBSpawnpoints[playersB].position, Quaternion.identity, 0);
                        var controller = p.GetComponentInChildren<SpacecraftController>();
                        controller.teamInt = 1;
                        controller.name = "AI" + i.ToString();
                        p.GetComponentInChildren<SpacecraftController>().photonView.RPC("Activate", RpcTarget.All);
                        UpdateScoreBoard(controller);
                        playersB++;
                    }
                }
            }
        }
        Debug.Log(allTargets.Count);
        Debug.Log("Current Players: " + PhotonNetwork.PlayerList.Length);
    }
    [PunRPC]
    private void CancelGame(){
        Debug.Log("CancelGame() Called! Not enough players!");
        StartCoroutine(GoToPostGame());
    }
    [PunRPC]
    private void GameOver(){
        gameOver = true;
        root.Q("GameOver").style.display = DisplayStyle.Flex;
        StartCoroutine(GoToPostGame());
        Debug.Log("GameOver() Called! Game has ended!");

        int winningTeam = -1;
        
        if(teamAScore > teamBScore){
            winningTeam = 0;
        }
        else if(teamAScore < teamBScore){
            winningTeam = 1;
        }
        else{
            winningTeam = -1;
            Debug.Log("Draw.");
            return;
        }
        if((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == winningTeam){
            PhotonNetwork.SetPlayerCustomProperties(new Hashtable{{"isWin", true}});
            root.Q("Victory").style.display = DisplayStyle.Flex;
        }
        else{
            PhotonNetwork.SetPlayerCustomProperties(new Hashtable{{"isWin", false}});
            root.Q("Defeat").style.display = DisplayStyle.Flex;
        }
    }
    #endregion

    #region Leaving & Joining
    public override void OnLeftRoom(){
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        allTargets.TrimExcess();
    }

    public void LeaveRoom(){
        //code to remove the player from targets, weapons controller might take care of this.
        PhotonNetwork.LeaveRoom();
    }    

    public override void OnPlayerEnteredRoom(Player newPlayer){
        if(PhotonNetwork.LocalPlayer.CustomProperties["Team"] == null){
            int teamA = 0;
            int teamB = 0;
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
                if((int)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == 0){
                    teamA++;
                }
                else if((int)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == 1){
                    teamB++;
                }
            }
            if(teamA <= teamB){
                newPlayer.SetCustomProperties(new Hashtable(){{"Team", "A"}});
            }
            else{
                newPlayer.SetCustomProperties(new Hashtable(){{"Team", "B"}});
            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps){
        UpdateScoreBoard(targetPlayer);
    }
    #endregion
    #region UI
    #region Feed Events
    [PunRPC]
    public void FeedEvent(int dealerID, int receiverID, string cause, bool isKill, bool receiverIsPlayer){
        var dealer = PhotonView.Find(dealerID).gameObject.GetComponent<SpacecraftController>();
        if(receiverIsPlayer){
            var receiver = PhotonView.Find(receiverID).gameObject.GetComponent<SpacecraftController>();
            var item = new FeedItem();
            feed.Add(item);

            if(dealer == receiver){
                item.SetData(null, receiver.playerName, cause);
            }
            else{
                item.SetData(dealer.playerName, receiver.playerName, cause);
            }
            StartCoroutine(item.feedTimer());
            //Does event get score?
            if(dealer == receiver || dealerID == -1){
                return;
            }
        }
        else{
            var receiver = PhotonView.Find(receiverID).gameObject.GetComponent<TargetableObject>();
            var item = new FeedItem();
            feed.Add(item);

            item.SetData(dealer.playerName, receiver.nameOfTarget, cause);
            StartCoroutine(item.feedTimer());
        }
        //determine score
        if(PhotonNetwork.IsMasterClient){
            if(isKill){
                if(currentGamemode.EliminationPoints){
                    photonView.RPC("Score", RpcTarget.AllBuffered, dealerID);
                }
            }
            else{
                photonView.RPC("Score", RpcTarget.AllBuffered, dealerID);
            }
        }
        
        
    }
    /*public void FeedEvent(SpacecraftController dealer, string receiver, string cause, bool isKill){
        var item = new FeedItem();
        feed.Add(item);
        if(receiver == null){
            item.SetData(dealer.playerName, null, cause);
        }
        else{
            item.SetData(dealer.playerName, receiver, cause);
        }
        StartCoroutine(item.feedTimer());
        //Does event get score?
        if(isKill){
            if(currentGamemode.EliminationPoints){
                Score(dealer);
            }
        }
        else{
            Score(dealer);
        }
    }
    public void FeedEvent(SpacecraftController dealer, string cause, bool isKill){
        var item = new FeedItem();
        feed.Add(item);
        item.SetData(dealer.playerName, null, cause);
        StartCoroutine(item.feedTimer());
        
        //Does event get score?
        if(isKill){
            if(currentGamemode.EliminationPoints){
                Score(dealer);
            }
        }
        else{
            Score(dealer);
        }
    }*/
    #endregion
    public void Subtitle(DialogueObject dialogueObject){
        var sub = new Subtitle();
        subtitle.Add(sub);
        sub.SetData(dialogueObject.subtitle);
        StartCoroutine(sub.Timer());
    }
    #endregion

    #region Scoring
    [PunRPC]
    public void Score(int dealerID){
        var dealer = PhotonView.Find(dealerID).gameObject.GetComponent<SpacecraftController>();
        var team = dealer.teamInt;
        if(gameOver)return;
        if(team == 0){
            teamAScore += currentGamemode.scoreValue;
        }
        else if(team == 1){
            teamBScore += currentGamemode.scoreValue;
        }
        else{
            Debug.Log("No team detected to give score to");
        }
        dealer.AddScore(currentGamemode.scoreValue);
        if(photonView.IsMine){
            Debug.Log("Player " + PhotonNetwork.LocalPlayer.NickName + " is on team " + PhotonNetwork.LocalPlayer.CustomProperties["Team"]);
            if((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 0){
                root.Q<Label>("FriendScore").text = teamAScore.ToString("0#");
                root.Q<Label>("EnemyScore").text = teamBScore.ToString("0#");
            }
            else{
                root.Q<Label>("FriendScore").text = teamBScore.ToString("0#");
                root.Q<Label>("EnemyScore").text = teamAScore.ToString("0#");
            }
        }
        if(PhotonNetwork.IsMasterClient){
            if(teamAScore >= currentGamemode.maxScore || teamBScore >= currentGamemode.maxScore){
                photonView.RPC("GameOver", RpcTarget.AllBuffered);
                GameOver();
            }
        }
    }
     public void UpdateScoreBoard(Player targetPlayer){
        if(gameOver)return;
        if(!gameStarted)return;
        if(targetPlayer.CustomProperties["Kills"] == null)return;

        //Create Card if needed
        ScoreBoardCard card = null;
        bool isFriendly = false;
        if(tabScreen.Q<ScoreBoardCard>(targetPlayer.NickName) == null){
            card = new ScoreBoardCard();
            card.name = targetPlayer.NickName;
        
            if((int)targetPlayer.CustomProperties["Team"] == (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"]){
                tabScreen.Q("Friendly").Add(card);
                isFriendly = true;
            }
            else{
                tabScreen.Q("Enemy").Add(card);
                isFriendly = false;
            }
        }
        else{
            card = tabScreen.Q<ScoreBoardCard>(targetPlayer.NickName);
            isFriendly = card.isFriendly;
        }
        //Find data
        string _player = (string)targetPlayer.CustomProperties["Name"];
        string _char = (string)targetPlayer.CustomProperties["Character"];
        string _ship = (string)targetPlayer.CustomProperties["Ship"];
        int _kills = (int)targetPlayer.CustomProperties["Kills"];
        int _score = (int)targetPlayer.CustomProperties["Score"];
        int _deaths = (int)targetPlayer.CustomProperties["Deaths"];

        var handler = Resources.Load<CharacterHandler>("Characters/" + _char);
        card.SetData(isFriendly, _player, _char, _ship, _kills, _score, _deaths, handler);
    }
    //used for AI
    public void UpdateScoreBoard(SpacecraftController controller){
        //if(gameOver)return;
        if(!gameStarted)return;
        //var list = PhotonNetwork.PlayerList;
        string _player = (string)controller.customProperties["Name"];
        string _char = (string)controller.customProperties["Character"];
        string _ship = (string)controller.customProperties["Ship"];
            
        int _kills = (int)controller.customProperties["Kills"];
        int _score = (int)controller.customProperties["Score"];
        int _deaths = (int)controller.customProperties["Deaths"];

        ScoreBoardCard card = null;
        bool isFriendly = false;
        if(tabScreen.Q<ScoreBoardCard>(controller.name) == null){
            card = new ScoreBoardCard();
            card.name = controller.name;
        
            if((int)controller.teamInt == (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"]){
                tabScreen.Q("Friendly").Add(card);
                isFriendly = true;
            }
            else{
                tabScreen.Q("Enemy").Add(card);
                isFriendly = false;
            }
        }
        else{
            card = tabScreen.Q<ScoreBoardCard>(controller.name);
            isFriendly = card.isFriendly;
        }
        var handler = Resources.Load<CharacterHandler>("Characters/" + _char);
        card.SetData(isFriendly, _player, _char, _ship, _kills, _score, _deaths, handler);
    }
    #endregion
    
    #region IEnumerators
    [PunRPC]
    private IEnumerator GameTimer(){
        if(gameOver){
            root.Q<Label>("GameTimer").text = "Game Over.";
            yield break;
        }
        int minutes = Mathf.CeilToInt(gameTimer /60);
        int seconds = gameTimer % 60;

        string timeText = minutes.ToString() + ":" + seconds.ToString("0#");

        root.Q<Label>("GameTimer").text = timeText;
        yield return new WaitForSecondsRealtime(1);

        gameTimer--;
        if(gameTimer <= 0){
            GameOver();
        }
        StartCoroutine(GameTimer());
    }

    private IEnumerator GoToPostGame(){
        yield return new WaitForSecondsRealtime(5);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Home");
    }
    #endregion
}
