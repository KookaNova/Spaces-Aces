using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

static class EditPreferences
{
    public static int
        displayMode = 0,   //0=Exclusive Fullscreen, 1=FullScreen Window, 2=Maximized Window, 3=Windowed
        resolution = 4,    //0=2560x1440, 1=1920x1080, 2=1600x900, 3=1366x768, 4=1280x720, 5=864x486
        quality = 2,       //0=Low, 1=Medium, 2=High
        textureRes = 0,    //0=Full, 1= Half, 2=Quarter
        aa = 2,            //None, 2x, 4x
        shadowQuality = 0; //0=low, medium, high, veryhigh
    public static bool 
        usingRealtimeReflections = true,
        usingBetterParticles = true,
        usingVSync = false;
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
        //Resolution
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
        //Quality Mode
        QualitySettings.SetQualityLevel(quality);
        QualitySettings.masterTextureLimit = textureRes;
        //Anti-Aliasing None, 2x, 4x
        switch (aa){
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
        }
        
        switch (shadowQuality){
            case 0:
                QualitySettings.shadowResolution = ShadowResolution.Low;
                break;
            case 1:
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                break;
            case 2:
                QualitySettings.shadowResolution = ShadowResolution.High;
                break;
            case 3:
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                break;
        }
        QualitySettings.realtimeReflectionProbes = usingRealtimeReflections;
        QualitySettings.softParticles = usingBetterParticles;
        if(usingVSync){
            QualitySettings.vSyncCount = 1;
        }
        else{
            QualitySettings.vSyncCount = 0;
        }
        
    }

    public static void LoadSettings(){
        displayMode = PlayerPrefs.GetInt("DisplayMode");
        resolution = PlayerPrefs.GetInt("Resolution");
        quality = PlayerPrefs.GetInt("Quality");
        textureRes = PlayerPrefs.GetInt("TextureRes");
        aa = PlayerPrefs.GetInt("Anti-Aliasing");
        shadowQuality = PlayerPrefs.GetInt("ShadowQuality");
        usingRealtimeReflections = PlayerPrefs.GetInt("RealtimeReflections") == 1;
        usingBetterParticles = PlayerPrefs.GetInt("BetterParticles") == 1;
        usingVSync = PlayerPrefs.GetInt("VSync") == 1;
        
        master = PlayerPrefs.GetFloat("v_Master");
        music = PlayerPrefs.GetFloat("v_Music");
        sound = PlayerPrefs.GetFloat("v_Sound");
        environment = PlayerPrefs.GetFloat("v_Environment");
        dialogue = PlayerPrefs.GetFloat("v_Dialogue");

        UpdateAudioSettings();
        UpdateGraphicsSettings();
    }

    public static void SaveSettings(){
        PlayerPrefs.SetInt("DisplayMode", displayMode);
        PlayerPrefs.SetInt("Resolution", resolution);
        PlayerPrefs.SetInt("Quality", quality);
        PlayerPrefs.SetInt("TextureRes", textureRes);
        PlayerPrefs.SetInt("Anti-Aliasing", aa);
        PlayerPrefs.SetInt("ShadowQuality", shadowQuality);
        PlayerPrefs.SetInt("RealtimeReflections", usingRealtimeReflections? 1:0);
        PlayerPrefs.SetInt("BetterParticles", usingBetterParticles? 1:0);
        PlayerPrefs.SetInt("VSync", usingVSync? 1:0);

        PlayerPrefs.SetFloat("v_Master", master);
        PlayerPrefs.SetFloat("v_Music", music);
        PlayerPrefs.SetFloat("v_Sound", sound);
        PlayerPrefs.SetFloat("v_Environment", environment);
        PlayerPrefs.SetFloat("v_Dialogue", dialogue);
        PlayerPrefs.Save();
    }
}


