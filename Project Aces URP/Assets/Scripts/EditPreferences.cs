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
        displayMode = 0,   //0=Exclusive Fullscreen, 1=FullScreen Window, 2=Maximized Window, 3=Windowed
        resolution = 4,    //0=2560x1440, 1=1920x1080, 2=1600x900, 3=1366x768, 4=1280x720, 5=864x486
        quality = 0,       //0=High, 1=Medium, 2=Low
        textureRes = 0,    //0=Full, 1= Half, 2=Quarter
        aa = 0,            //anti-aliasing: 0=Fast (FXAA), 1=Fancy (SMAA)
        shadowQuality = 0; //0=High,

    //audio
    public static AudioMixer finalMix = Resources.Load<AudioMixer>("Audio/Mixers/FinalMix");
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
        FullScreenMode fsm;
        //display mode
        switch(displayMode){
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                fsm = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                fsm = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                fsm = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                fsm = FullScreenMode.Windowed;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                fsm = FullScreenMode.ExclusiveFullScreen;
                break;
        }
        switch (resolution){
            case 0:
                Screen.SetResolution(2560, 1440, fsm);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, fsm);
                break;
            case 2:
                Screen.SetResolution(1600, 900, fsm);
                break;
            case 3:
                Screen.SetResolution(1366, 768, fsm);
                break;
            case 4:
                Screen.SetResolution(1280, 720, fsm);
                break;
            case 5:
                Screen.SetResolution(864, 486, fsm);
                break;
            default:
                Screen.SetResolution(1280, 720, fsm);
                break;

        }


        
        
    }
}
