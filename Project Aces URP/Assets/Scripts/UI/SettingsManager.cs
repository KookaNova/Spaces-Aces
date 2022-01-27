using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsManager : VisualElement
{
    public new class UxmlFactory : UxmlFactory<SettingsManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{ }

    Slider brightness;

    public SettingsManager(){
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt)
    {
        //set
        brightness = this.Q<Slider>("Brightness");

        //inputs
        brightness.RegisterCallback<ClickEvent>(ev => ChangeBrightness(brightness.value));
    }

    private void ChangeBrightness(float newValue){
        EditPreferences.brightness = brightness.value;
        EditPreferences.UpdateGraphicsSettings();
        Debug.Log("Brightness Change");
    }
}
