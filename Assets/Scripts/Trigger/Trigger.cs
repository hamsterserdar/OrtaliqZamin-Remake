using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent unityEvent,DialogueEvent;
    public bool DialogMode;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            unityEvent.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(DialogMode)
        {
            DialogueEvent.Invoke();
        }
    }
}
