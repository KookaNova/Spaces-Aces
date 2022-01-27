using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

static class EditPreferences
{
    //graphics
    [Range(0,100)]
    public static float brightness = 50; //0-100 or 0-1
    public static int
        displayMode = 0,   //0=fullscreen, 1=full (windowed), 2=windowed
        resolution = 0,    //0=1440, 1=1080, 2=900, 3=768, 4=720, 5=480
        quality = 0,       //0=High, 1=Medium, 2=Low
        textureRes = 0,    //0=Full, 1= Half, 2=Quarter
        aa = 0,            //anti-aliasing: 0=Fast (FXAA), 1=Fancy (SMAA)
        shadowQuality = 0; //0=High,

    //audio
    public static AudioMixer finalMix;
    [Range(-80, 20)]
    public static float 
        master = 0, 
        music = 0, 
        sound = 0, 
        environment = 0, 
        dialogue = 0; 
        //-80db to 20db

    public static void UpdateAudioSettings(){
        finalMix.SetFloat("v_Master", master);
        finalMix.SetFloat("v_Music", music);
        finalMix.SetFloat("v_Sound", sound);
        finalMix.SetFloat("v_Env", environment);
        finalMix.SetFloat("v_Dialogue", dialogue);
    }

    public static void UpdateGraphicsSettings(){
        Screen.brightness = brightness;
    }
}
