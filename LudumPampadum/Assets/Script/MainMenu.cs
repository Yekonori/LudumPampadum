using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject _playButton;
    [Space]
    public GameObject selectLevel;
    public GameObject _returnButton;
    [Space]
    public GameObject creditPanel;
    public GameObject _returnButton2;

    private EventSystem _eventSystem;

    private void Start()
    {
        _eventSystem = EventSystem.current;

        //_playButton = mainPanel.transform.GetChild(0).gameObject;
        //_returnButton = selectLevel.transform.GetChild(2).gameObject;
        //_returnButton2 = creditPanel.transform.GetChild(1).gameObject;

        _eventSystem.SetSelectedGameObject(_playButton);
    }

    public void DiplayMain()
    {
        mainPanel.SetActive(true);
        selectLevel.SetActive(false);
        creditPanel.SetActive(false);
        _eventSystem.SetSelectedGameObject(_playButton);
    }

    public void DisplaySelectionLevel()
    {
        mainPanel.SetActive(false);
        selectLevel.SetActive(true);
        creditPanel.SetActive(false);
        _eventSystem.SetSelectedGameObject(_returnButton);
    }

    public void DisplayCredit()
    {
        mainPanel.SetActive(false);
        selectLevel.SetActive(false);
        creditPanel.SetActive(true);
        _eventSystem.SetSelectedGameObject(_returnButton2);
    }

    public void StartGame(int sceneIndex)
    {
        Debug.Log("Starting level " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
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

