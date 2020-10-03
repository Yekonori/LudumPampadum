using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Script Parameters

    [Header("Main Canvas")]

    [SerializeField] private bool onDebugMode;
    [SerializeField] private Button reloadButton;

    #endregion

    #region Fields

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (onDebugMode)
        {
            reloadButton.gameObject.SetActive(true);
            reloadButton.onClick.AddListener(ReloadScene);
        }
    }

    #endregion

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
