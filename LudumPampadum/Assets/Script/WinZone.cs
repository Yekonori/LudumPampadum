using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{

    [SerializeField]
    Animator fadeScreenAnim;

    public void LoadNextZone(string nextLevel)
    {
        StartCoroutine(LoadZoneCoroutine(nextLevel));
    }

    private IEnumerator LoadZoneCoroutine(string nextLevel)
    {
        Animator anim = Camera.main.GetComponent<Animator>();
        if(anim != null)
            anim.SetTrigger("Feedback");
        fadeScreenAnim.SetTrigger("Feedback");
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(nextLevel);
    }

    public void LoadNextZone(int nextLevel)
    {
        StartCoroutine(LoadZoneCoroutine(nextLevel));
    }

    private IEnumerator LoadZoneCoroutine(int nextLevel)
    {
        Animator anim = Camera.main.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Feedback");
        fadeScreenAnim.SetTrigger("Feedback");
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(nextLevel);
    }
}
