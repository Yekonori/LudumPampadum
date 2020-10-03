using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    #region Script Parameters

    [Header("Main Canvas")]

    [SerializeField] private PlayerMovement playerPrefab;

    [SerializeField] private GhostMovement ghostPrefab;
    [SerializeField] private Transform world;
    [SerializeField] private Button startButton;

    [Header("Debug Mode")]

    [SerializeField] private bool onDebugMode;
    [SerializeField] private Button reloadButton;

    #endregion

    #region Fields

    private static GameManager _instance;
    public static GameManager Get
    {
        get { return _instance; }
    }

    [SerializeField] private List<PlayerMovement> players;
    [SerializeField] private List<GhostMovement> ghosts;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (_instance != this)
        {
            _instance = this;
        }

        startButton.onClick.AddListener(LaunchMovement);

        if (onDebugMode)
        {
            SetDebugMode();
        }
    }

    #endregion

    #region Players

    private void LaunchPlayer()
    {
        GhostMovement ghost = Instantiate(ghostPrefab, world);
        ghost.SetListNodes(playerPrefab.ListPoints);

        ghosts.Add(ghost);
        
        playerPrefab.Launch();
    }

    #endregion

    #region Ghosts

    public void LaunchGhosts()
    {
        foreach (GhostMovement ghost in ghosts)
        {
            ghost.Launch();
        }
    }

    public void StopGhosts()
    {
        foreach (GhostMovement ghost in ghosts)
        {
            ghost.Stop();
        }
    }

    public void ResumeGhosts()
    {
        foreach (GhostMovement ghost in ghosts)
        {
            ghost.Resume();
        }
    }

    public void ResetGhosts()
    {
        foreach (GhostMovement ghost in ghosts)
        {
            ghost.ResetNodes();
        }
    }

    #endregion

    #region Movement

    private void LaunchMovement()
    {
        LaunchPlayer();
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
