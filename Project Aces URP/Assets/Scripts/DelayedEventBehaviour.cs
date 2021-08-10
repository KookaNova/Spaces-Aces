using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEventBehaviour : MonoBehaviour
{
    public UnityEvent initialEvent, endEvent;

    public void ActivateEvents(float waitTime){
        initialEvent.Invoke();
        StartCoroutine(EventDelay(waitTime));
    }

    private IEnumerator EventDelay(float waitTime){
        yield return new WaitForSeconds(waitTime);
        OnEndEvent();

    }

    private void OnEndEvent(){
        endEvent.Invoke();
    }
}
