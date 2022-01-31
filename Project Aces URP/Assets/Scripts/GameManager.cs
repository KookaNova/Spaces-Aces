using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Cox.PlayerControls;
using UnityEngine.UIElements;
using System.Collections;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject debugPlayer;
    public static GameManager Instance;
    public GamemodeData currentGamemode;
    public Transform[] teamASpawnpoints, teamBSpawnpoints;
    public bool isSelectLoaded = false;

    [Tooltip("The amount of time in seconds that the match will last.")]
    [SerializeField] private int gameTimer = 600;
    [SerializeField] bool debugMode = false;

    int teamAScore, teamBScore;
    int timeOut = 45, startCount = 10;
    bool gameReady = false, gameStarted = false, gameOver = false;
    SceneController sceneController;


    //UI
    VisualElement root, feed, subtitle, tabScreen;
    [SerializeField] UIDocument uIDocument;



    private Scene mainScene;

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
        
        

        Instance = this; 
        mainScene = SceneManager.GetActiveScene();
        if(!PhotonNetwork.IsConnected){
            //allows us to play in a level even when we're offline. We switch to offline mode and create an offline room.
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom(null, null);
        }

        if((string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == null){
            PhotonNetwork.SetPlayerCustomProperties(new Hashtable(){{"Team", "A"}});
        }

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

        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        if((string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == "A"){
            int spawnPoint = Random.Range(0, teamASpawnpoints.Length - 1);
            var p = PhotonNetwork.Instantiate(this.playerPrefab.name, teamASpawnpoints[spawnPoint].position, Quaternion.identity, 0);
            Debug.LogFormat("GameManager: SpawnPlayer(), Spawned player {0}", p);
        }
        else{
            int spawnPoint = Random.Range(0, teamBSpawnpoints.Length - 1);
            var p = PhotonNetwork.Instantiate(this.playerPrefab.name, teamBSpawnpoints[spawnPoint].position, Quaternion.identity, 0);
            Debug.LogFormat("GameManager: SpawnPlayer(), Spawned player {0}", p);
        }
        
        
    }

    private IEnumerator StartCheck(){
        yield return new WaitForSeconds(1);
        timeOut--;
        root.Q<Label>("GameTimer").text = "Waiting for players.";
        if(timeOut <= 0){
            CancelGame();
        }
        if(PhotonNetwork.PlayerList.Length == currentGamemode.playerCount){
            StartCoroutine(StartCountDown());
            root.Q<Label>("GameTimer").text = startCount.ToString();
        }
        else{
            StartCoroutine(StartCheck());
            
        }

    }

    private IEnumerator StartCountDown(){
        yield return new WaitForSecondsRealtime(1);
        startCount--;
        root.Q<Label>("GameTimer").text = "Game starts in " + startCount.ToString();
        if(startCount <= 0){
            StartGame();
        }
        else{
            StartCoroutine(StartCountDown());
        }
       

    }

    private void CancelGame(){
        Debug.Log("CancelGame() Called! Not enough players!");
    }


    private void StartGame(){
        gameStarted = true;
        StartCoroutine(GameTimer());
        SpawnPlayer();
        

        Debug.Log("Current Players: " + PhotonNetwork.PlayerList.Length);

    }

    private void GameOver(){
        gameOver = true;
        StartCoroutine(GoToPostGame());
        Debug.Log("GameOver() Called! Game has ended!");

        string winningTeam = null;
        if(teamAScore > teamBScore){
            winningTeam = "A";
        }
        else if(teamAScore < teamBScore){
            winningTeam = "B";
        }
        else{
            winningTeam = "Draw";
            Debug.Log("Draw.");
            return;
        }
        if((string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == winningTeam){
            Debug.Log("Victory!");
        }
        else{
            Debug.Log("Defeat.");
        }

    }

    #endregion

    #region Leaving & Joining
    public override void OnLeftRoom(){
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }    

    public override void OnPlayerEnteredRoom(Player newPlayer){
        if((string)newPlayer.CustomProperties["Team"] == null){
            int teamA = 0;
            int teamB = 0;
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
                if((string)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == "A"){
                    teamA++;
                }
                else if((string)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == "B"){
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
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        UpdateScoreBoard();
    }
    #endregion

    #region UI
    #region Feed Events
    public void FeedEvent(SpacecraftController dealer, SpacecraftController receiver, string cause, bool isKill){
        
        var item = new FeedItem();
        feed.Add(item);
        if(receiver == null){
            item.SetData(dealer.playerName, null, cause);
        }
        else if(dealer == receiver){
            item.SetData(null, receiver.playerName, cause);
        }
        else{
            item.SetData(dealer.playerName, receiver.playerName, cause);
        }
        StartCoroutine(item.feedTimer());

        //Does event get score?
        if(dealer == receiver){
            return;
        }
        if(isKill){
            if(currentGamemode.EliminationPoints){
                Score(dealer);
            }
        }
        else{
            Score(dealer);
        }



    }
    public void FeedEvent(SpacecraftController dealer, string receiver, string cause, bool isKill){
        
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



    }
    #endregion

    public void UpdateScoreBoard(){
        if(gameOver)return;
        var list = PhotonNetwork.PlayerList;

        tabScreen.Q("Friendly").Clear();

        for(int i = 0; i < list.Length; i++){
            
            string _player = (string)list[i].CustomProperties["Name"];
            string _char = "?";
            string _ship = "?";
            
            int _kills = 0;
            int _score = 0;
            int _deaths = 0;

            if(gameStarted){
                _char = (string)list[i].CustomProperties["Character"];
                _ship = (string)list[i].CustomProperties["Ship"];
                _kills = (int)list[i].CustomProperties["Kills"];
                _score = (int)list[i].CustomProperties["Score"];
                _deaths = (int)list[i].CustomProperties["Deaths"];
            }

            var card = new ScoreBoardCard();
            if((string)list[i].CustomProperties["Team"] == (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"]){
                tabScreen.Q("Friendly").Add(card);
            }
            else{
                tabScreen.Q("Enemy").Add(card);
            }
            
            card.SetData(true, _player, _char, _ship, _kills, _score, _deaths);

            if((string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == "A"){
                root.Q<Label>("FriendScore").text = teamAScore.ToString("0#");
                root.Q<Label>("EnemyScore").text = teamBScore.ToString("0#");
            }
            else{
                root.Q<Label>("FriendScore").text = teamBScore.ToString("0#");
                root.Q<Label>("EnemyScore").text = teamAScore.ToString("0#");
            }
            
        }

    }

    public void Subtitle(DialogueObject dialogueObject){
        var sub = new Subtitle();
        subtitle.Add(sub);
        sub.SetData(dialogueObject.subtitle);
        StartCoroutine(sub.Timer());

    }
    #endregion

    #region Scoring
    public void Score(SpacecraftController dealer){
        if(gameOver)return;
        if((string)dealer.photonView.Owner.CustomProperties["Team"] == "A"){
            teamAScore += currentGamemode.scoreValue;
        }
        if((string)dealer.photonView.Owner.CustomProperties["Team"] == "B"){
            teamBScore += currentGamemode.scoreValue;
        }
        
        dealer.AddScore(currentGamemode.scoreValue);

        if(teamAScore >= currentGamemode.maxScore || teamBScore >= currentGamemode.maxScore){
            GameOver();
        }

    }

    #endregion



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

    




}
