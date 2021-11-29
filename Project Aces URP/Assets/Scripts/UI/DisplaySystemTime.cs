using System;
using UnityEngine;
using UnityEngine.UIElements;

public class DisplaySystemTime : MonoBehaviour
{   
    Label systemTime;

    private void OnEnable() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        systemTime = root.Q<Label>("SystemTime");
    }

    void LateUpdate()
    {
        var time = DateTime.Now;
        int hour = time.Hour;
        int minute = time.Minute;
        string dayPart = "AM";

        if(hour >= 12){
            hour -= 12;
            dayPart = "PM";
        }
        if(hour == 0){
            hour = 12;
        }

        string finalTime = hour.ToString() + ":" + minute.ToString("0#") + dayPart;
        systemTime.text = finalTime;
    }
}
