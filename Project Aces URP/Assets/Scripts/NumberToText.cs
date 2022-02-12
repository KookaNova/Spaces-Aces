using System;
using UnityEngine;
using UnityEngine.UI;

public class NumberToText : MonoBehaviour
{
    private Text thisText;

    private void OnEnable() {
        thisText = GetComponent<Text>();
    }

    public void ConvertToText(int number){
        thisText.text = number.ToString();
    }
    public void ConvertToText(byte number){
        thisText.text = number.ToString();
    }
    public void ConvertToText(Single number){
        thisText.text = number.ToString();
    }
    public void ConvertToText(long number){
        thisText.text = number.ToString();
    }
    public void ConvertToText(double number){
        thisText.text = number.ToString();
    }


}
