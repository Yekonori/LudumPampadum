using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerEvent : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnEnter;
    [SerializeField]
    UnityEvent OnExit;
    [SerializeField]
    bool onlyOnce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEnter.Invoke();
            if(onlyOnce == true)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnExit.Invoke();
            if (onlyOnce == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
