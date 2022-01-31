using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsManager : VisualElement
{
    public new class UxmlFactory : UxmlFactory<SettingsManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }
    

    //Graphics

    public DropdownField
        displayMode,
        resolution,
        quality,
        textureRes,
        antiAliasing,
        shadowQuality;
    public Toggle 
        realtimeReflections,
        betterParticles,
        vSync;


    public Button apply;
    //Audio
    public Slider
        master,
        music,
        sound,
        environment,
        dialogue;

    public SettingsManager(){
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt)
    {
        //graphics controls
        displayMode = this.Q<DropdownField>("DisplayMode");
        resolution = this.Q<DropdownField>("Resolution");
        quality = this.Q<DropdownField>("Quality");
        textureRes = this.Q<DropdownField>("Texture");
        antiAliasing = this.Q<DropdownField>("AA");
        realtimeReflections = this.Q<Toggle>("RealtimeReflections");
        betterParticles = this.Q<Toggle>("BetterParticles");
        vSync = this.Q<Toggle>("VSync");

        apply = this.Q<Button>("Apply");

        //audio controls
        master = this.Q<Slider>("Master");
        music = this.Q<Slider>("Music");
        sound = this.Q<Slider>("Sound");
        environment = this.Q<Slider>("Environment");
        dialogue = this.Q<Slider>("Dialogue");

        //inputs
        master.RegisterCallback<ChangeEvent<float>>(ev => UpdateAudio());
        music.RegisterCallback<ClickEvent>(ev => UpdateAudio());
        sound.RegisterCallback<ClickEvent>(ev => UpdateAudio());
        environment.RegisterCallback<ClickEvent>(ev => UpdateAudio());
        dialogue.RegisterCallback<ClickEvent>(ev => UpdateAudio());

        apply.RegisterCallback<ClickEvent>(ev => ApplyChanges());

        
    }

    private void UpdateAudio(){
        EditPreferences.master = master.value;
        EditPreferences.music = music.value;
        EditPreferences.sound = sound.value;
        EditPreferences.environment = environment.value;
        EditPreferences.dialogue = dialogue.value;

        EditPreferences.UpdateAudioSettings();
        EditPreferences.SaveSettings();
        
    }

    private void UpdateGraphics(){

        EditPreferences.UpdateGraphicsSettings();
    }

    private void ApplyChanges(){
        EditPreferences.displayMode = displayMode.index;
        EditPreferences.resolution = resolution.index;
        EditPreferences.quality = quality.index;
        EditPreferences.textureRes = textureRes.index;
        EditPreferences.aa = antiAliasing.index;
        EditPreferences.usingRealtimeReflections = realtimeReflections.value;

        EditPreferences.UpdateGraphicsSettings();
        EditPreferences.SaveSettings();

    }
    public void LoadDefaults(){

        Debug.Log("LOADING HOMIE");

    }
}
