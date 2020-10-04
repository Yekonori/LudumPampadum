using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject selectLevel;
    public GameObject creditPanel;

    public void DiplayMain()
    {
        mainPanel.SetActive(true);
        selectLevel.SetActive(false);
        creditPanel.SetActive(false);
    }

    public void DisplaySelectionLevel()
    {
        mainPanel.SetActive(false);
        selectLevel.SetActive(true);
        creditPanel.SetActive(false);
    }

    public void DisplayCredit()
    {
        mainPanel.SetActive(false);
        selectLevel.SetActive(false);
        creditPanel.SetActive(true);
    }

    public void StartGame(int sceneIndex)
    {
        Debug.Log("Starting level " + sceneIndex);
        //SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit()
#endif
    }
}

