using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Cox.PlayerControls;
using UnityEngine.UIElements;
using System.Collections;
using Photon.Realtime;

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


    //UI
    VisualElement root, feed, tabScreen;
    [SerializeField] UIDocument uIDocument;



    private Scene mainScene;

    #region GameStart

    private void Start() {
        //currentGamemode = FindObjectOfType<SceneController>().chosenGamemode;
        root = uIDocument.rootVisualElement;
        feed = root.Q("EventFeed");
        tabScreen = root.Q("TabScreen");
        
        

        Instance = this; 
        mainScene = SceneManager.GetActiveScene();
        if(!PhotonNetwork.IsConnected){
            //allows us to play in a level even when we're offline. We switch to offline mode and create an offline room.
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom(null, null);
        }
        
        OpenSelectMenu();

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
        SpawnPlayer();
        StartGame();
        
    }

    public void SpawnPlayer(){
        if(playerPrefab == null){
            Debug.LogError("GameManager: SpawnPlayer(), playerPrefab is null. Place a prefab in the inspector.", this);
            return;
        }


        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        var p = PhotonNetwork.Instantiate(this.playerPrefab.name, teamASpawnpoints[0].position, Quaternion.identity, 0);
        Debug.LogFormat("GameManager: SpawnPlayer(), Spawned player {0}", p);
    }

    private IEnumerator StartCheck(){
        yield return new WaitForSeconds(1);

    }

    private void StartGame(){
        StartCoroutine(GameTimer());

        Debug.Log("Current Players: " + PhotonNetwork.PlayerList.Length);
        UpdateScoreBoard();

    }

    private void GameOver(){
        Debug.Log("GameOver() Called! Game has ended!");

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
        UpdateScoreBoard();

    }
    #endregion

    #region UI

    public void FeedEvent(SpacecraftController dealer, SpacecraftController receiver, string cause){
        Debug.LogFormat("GameManager: FeedEvent(), {0} eliminated {1} with {2}", dealer.playerName, receiver.playerName, cause);

        var item = new FeedItem();
        feed.Add(item);
        item.SetData(dealer.playerName, receiver.playerName, cause);

        StartCoroutine(item.feedTimer());

    }

    public void UpdateScoreBoard(){
        var list = PhotonNetwork.PlayerList;

        tabScreen.Q("Friendly").Clear();

        for(int i = 0; i < list.Length; i++){
            
            string _player = (string)list[i].CustomProperties["Name"];
            string _char = (string)list[i].CustomProperties["Character"];
            string _ship = (string)list[i].CustomProperties["Ship"];
            int _kills = (int)list[i].CustomProperties["Kills"];
            int _score = (int)list[i].CustomProperties["Score"];
            int _deaths = (int)list[i].CustomProperties["Deaths"];

            var card = new ScoreBoardCard();
            tabScreen.Q("Friendly").Add(card);
            card.SetData(true, _player, _char, _ship, _kills, _score, _deaths);
            Debug.Log(card.name + " added.");
        }

    }

    #endregion



    private IEnumerator GameTimer(){
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

    




}
