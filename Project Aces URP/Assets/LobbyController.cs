using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class LobbyController : NetworkBehaviour
{
    public int maxPlayerCount = 6;
    [HideInInspector]
    public static bool isHosting = false;
    private int currentPlayerCount;

    private void Awake() {
        if(currentPlayerCount <= 0)isHosting = true;

        if(isHosting){
            NetworkManager.Singleton.StartHost();}
        else{
            NetworkManager.Singleton.StartClient();

        }
    }
}
