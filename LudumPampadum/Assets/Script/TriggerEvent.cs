using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerEvent : MonoBehaviour
{
    #region Script Parameters

    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;

    [SerializeField] private bool onlyOnce;

    int triggerCount = 0;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerCount += 1;
            if (triggerCount == 1)
            {
                OnEnter.Invoke();
                if (onlyOnce == true) Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerCount -= 1;
            if(triggerCount == 0) OnExit.Invoke();
        }
    }

    #endregion

    public void SoundDoorEnter()
    {
        AudioManager.instance.Play("SFX_Pressure", true);
        AudioManager.instance.Play("SFX_Door_Open", false);
    }

    public void SoundDoorExit()
    {
        AudioManager.instance.Play("SFX_Door_Close", false);
    }

    public void SoundDead()
    {
        AudioManager.instance.Play("SFX_Dead", false);
    }

}
