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

    bool isAdjusting = false;

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

        //master.RegisterCallback<NavigationSubmitEvent>(ev => master.);
        master.RegisterCallback<NavigationSubmitEvent>(ev => master.value += 2);
        master.RegisterCallback<NavigationCancelEvent>(ev => master.value -= 2);
        music.RegisterCallback<NavigationSubmitEvent>(ev => music.value += 2);
        music.RegisterCallback<NavigationCancelEvent>(ev => music.value -= 2);
        sound.RegisterCallback<NavigationSubmitEvent>(ev => sound.value += 2);
        sound.RegisterCallback<NavigationCancelEvent>(ev => sound.value -= 2);
        environment.RegisterCallback<NavigationSubmitEvent>(ev => environment.value += 2);
        environment.RegisterCallback<NavigationCancelEvent>(ev => environment.value -= 2);
        dialogue.RegisterCallback<NavigationSubmitEvent>(ev => dialogue.value += 2);
        dialogue.RegisterCallback<NavigationCancelEvent>(ev => dialogue.value -= 2);
        

        //inputs
        master.RegisterCallback<ChangeEvent<float>>(ev => UpdateAudio());
        music.RegisterCallback<ChangeEvent<float>>(ev => UpdateAudio());
        sound.RegisterCallback<ChangeEvent<float>>(ev => UpdateAudio());
        environment.RegisterCallback<ChangeEvent<float>>(ev => UpdateAudio());
        dialogue.RegisterCallback<ChangeEvent<float>>(ev => UpdateAudio());
        apply.RegisterCallback<ClickEvent>(ev => ApplyChanges());
        apply.RegisterCallback<NavigationSubmitEvent>(ev => ApplyChanges());

        
    }

    private void UpdateAudio(){
        EditPreferences.master = master.value;
        EditPreferences.music = music.value;
        EditPreferences.sound = sound.value;
        EditPreferences.environment = environment.value;
        EditPreferences.dialogue = dialogue.value;

        EditPreferences.UpdateAudioSettings();
        
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
