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
        Camera.main.GetComponent<Animator>().SetTrigger("Feedback");
        fadeScreenAnim.SetTrigger("Feedback");
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(nextLevel);
    }
}
