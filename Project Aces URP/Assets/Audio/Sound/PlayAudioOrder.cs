using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOrder : MonoBehaviour
{
    public List<AudioClip> soundOrder;
    public AudioSource player;

    private void Awake() {
        player = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        player.clip = soundOrder[0];
        player.loop = false;
        player.Play();

    }

    private void Update(){
        if(player.isPlaying == false){
            player.clip = soundOrder[1];
            player.loop = true;

            player.Play();
        }

    }
}
