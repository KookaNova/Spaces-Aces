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
    Slider brightness;

    DropdownField
        displayMode,
        resolution,
        quality,
        textureRes,
        antiAliasing,
        shadowQuality;

    Button apply;
    //Audio
    Slider
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
        //set
        brightness = this.Q<Slider>("Brightness");
        
        //graphics controls
        displayMode = this.Q<DropdownField>("DisplayMode");
        resolution = this.Q<DropdownField>("Resolution");
        quality = this.Q<DropdownField>("Quality");
        textureRes = this.Q<DropdownField>("Texture");
        antiAliasing = this.Q<DropdownField>("AA");

        apply = this.Q<Button>("Apply");

        //audio controls
        master = this.Q<Slider>("Master");
        music = this.Q<Slider>("Music");
        sound = this.Q<Slider>("Sound");
        environment = this.Q<Slider>("Environment");
        dialogue = this.Q<Slider>("Dialogue");

        //inputs
        brightness.RegisterCallback<ClickEvent>(ev => UpdateGraphics());


        displayMode.RegisterCallback<ClickEvent>(ev => EditPreferences.displayMode = displayMode.index);
        resolution.RegisterCallback<ClickEvent>(ev => EditPreferences.resolution = resolution.index);
        quality.RegisterCallback<ClickEvent>(ev => EditPreferences.quality = quality.index);
        textureRes.RegisterCallback<ClickEvent>(ev => EditPreferences.textureRes = textureRes.index);
        antiAliasing.RegisterCallback<ClickEvent>(ev => EditPreferences.aa = antiAliasing.index);

        master.RegisterCallback<ClickEvent>(ev => UpdateAudio());
        music.RegisterCallback<ClickEvent>(ev => UpdateAudio());
        sound.RegisterCallback<ClickEvent>(ev => UpdateAudio());
        environment.RegisterCallback<ClickEvent>(ev => UpdateAudio());
        dialogue.RegisterCallback<ClickEvent>(ev => UpdateAudio());

        apply.RegisterCallback<ClickEvent>(ev => ApplyChanges());
        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        
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
        EditPreferences.brightness = brightness.value;

        EditPreferences.UpdateGraphicsSettings();
    }

    private void ApplyChanges(){
        EditPreferences.displayMode = displayMode.index;
        EditPreferences.resolution = resolution.index;
        EditPreferences.quality = quality.index;
        EditPreferences.textureRes = textureRes.index;
        EditPreferences.aa = antiAliasing.index;

        EditPreferences.UpdateGraphicsSettings();
    }
}
