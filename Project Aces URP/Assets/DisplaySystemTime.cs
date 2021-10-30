using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DisplaySystemTime : MonoBehaviour
{
    Text dateText;
    private void Awake() {
        dateText = GetComponent<Text>();
    }

    void LateUpdate()
    {
        var time = DateTime.Now;
        int hour = time.Hour;
        int minute = time.Minute;
        string dayPart = "AM";

        if(hour > 12){
            hour -= 12;
            dayPart = "PM";
        }
        if(hour == 0){
            hour = 12;
        }

        string finalTime = hour.ToString() + ":" + minute.ToString("0#") + dayPart;
        dateText.text = finalTime;

        
    }
}
