using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cox.PlayerControls;

public class GameEventObject : MonoBehaviour
{
    public bool kill = false, score = false, death = false;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    /*private void OnTriggerEnter(Collider player) {
        if(player.gameObject.GetComponentInParent<SpacecraftController>()){
            var dealerID = player.gameObject.GetComponentInParent<SpacecraftController>();
            if(kill){
                dealer.TargetDestroyed(true);
                gameManager.FeedEvent(dealer, null, "ORB", true);
            }
            if(score){
                gameManager.FeedEvent(dealer, null, "ORB", false);
            }
            if(death){
                dealer.Eliminate(dealer, "Accident");
            }
        }
        
    }*/
}
