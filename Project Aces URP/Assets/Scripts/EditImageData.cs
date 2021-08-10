using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EditImageData : MonoBehaviour
{
    private Image image;
    void OnEnable(){
        image = GetComponent<Image>();
    }

    public void ChangeImageColor(float hue){
        image.color = Color.HSVToRGB(hue, .80f, .85f);

    }
}
