using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent unityEvent; 

    public void CallEvent()
    {
        unityEvent.Invoke();
    }
}
