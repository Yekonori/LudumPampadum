using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Script Parameters

    [Header("Main Canvas")]

    [SerializeField] private GhostMovement ghostPrefab;
    [SerializeField] private Transform world;
    [SerializeField] private Button startButton;

    [Header("Debug Mode")]

    [SerializeField] private bool onDebugMode;
    [SerializeField] private Button reloadButton;

    #endregion

    #region Fields

    [SerializeField] private List<PlayerMovement> players;

    #endregion

    #region Unity Methods

    private void Start()
    {
        startButton.onClick.AddListener(LaunchMovement);

        if (onDebugMode)
        {
            SetDebugMode();
        }
    }

    #endregion

    #region Players

    private void GetAllPlayers()
    {
        players = new List<PlayerMovement>();

        List<GameObject> playersGo = GameObject.FindGameObjectsWithTag("Player").ToList();

        foreach(GameObject player in playersGo)
        {
            players.Add(player.GetComponent<PlayerMovement>());
        }


    }

    private void LaunchMovement()
    {
        GetAllPlayers();

        foreach(PlayerMovement player in players)
        {
            GhostMovement ghost = Instantiate(ghostPrefab, world);
            ghost.SetListNodes(player.ListPoints);

            player.Launch();
        }
    }

    #endregion

    #region Debug Mode

    private void SetDebugMode()
    {
        reloadButton.gameObject.SetActive(true);
        reloadButton.onClick.AddListener(ReloadScene);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
