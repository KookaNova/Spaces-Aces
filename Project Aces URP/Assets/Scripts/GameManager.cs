using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Cox.PlayerControls;
using UnityEngine.UIElements;
using System.Collections;

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
    private VisualElement root, feed;


    private Scene mainScene;

    #region GameStart

    private void Start() {
        //currentGamemode = FindObjectOfType<SceneController>().chosenGamemode;

        root = FindObjectOfType<UIDocument>().rootVisualElement;
        feed = root.Q("EventFeed");
        

        Instance = this; 
        mainScene = SceneManager.GetActiveScene();
        if(!PhotonNetwork.IsConnected){
            //allows us to play in a level even when we're offline. We switch to offline mode and create an offline room.
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom(null, null);
        }
        SpawnPlayer();
        StartCoroutine(GameTimer());
        //OpenSelectMenu();

    }

    public void OpenSelectMenu(){
        
        //SceneManager.LoadSceneAsync("Select Scene", LoadSceneMode.Additive);
        //isSelectLoaded = true;
    }
     public void CloseSelectMenu(){
        //isSelectLoaded = false;
        //SceneManager.UnloadSceneAsync("Select Scene");
        //SpawnPlayer();
        
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

    }

    private void GameOver(){
        Debug.Log("GameOver() Called! Game has ended!");

    }

    #endregion

    #region Leaving
    public override void OnLeftRoom(){
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
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
