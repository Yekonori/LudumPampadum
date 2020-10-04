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

    [Header("Players & Ghost")]

    [SerializeField] private PlayerMovement playerPrefab;

    [SerializeField] private GhostMovement ghostPrefab;
    [SerializeField] private Transform world;

    [Header("Timer")]

    [SerializeField] private float turnTimer;
    [SerializeField] private UIManager uiManager;

    #endregion

    #region Fields

    private static GameManager _instance;
    public static GameManager Get
    {
        get { return _instance; }
    }

    private List<GhostMovement> ghosts;

    private float _currentTurnTimer;
    private bool _canTimerRun;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (_instance != this)
        {
            _instance = this;
        }

        ghosts = new List<GhostMovement>();
    }

    private void Start()
    {
        InitTimer();

        SetUIButtons();

        uiManager.DrawEntity(ghosts.Count);
    }

    private void FixedUpdate()
    {
        if (_canTimerRun)
        {
            _currentTurnTimer -= Time.fixedDeltaTime;

            UpdateTimer();

            CheckEndTimer();
        }
    }

    #endregion

    #region Players

    private void LaunchPlayer()
    {
        CreateGhost();

        playerPrefab.Launch();
    }

    #endregion

    #region Ghosts

    private void CreateGhost()
    {
        GhostMovement ghost = Instantiate(ghostPrefab, playerPrefab.ListPoints[0], Quaternion.identity, world);
        ghost.SetListNodes(playerPrefab.ListPoints);

        ghosts.Add(ghost);

        uiManager.DrawEntity(ghosts.Count);
    }

    public void LaunchGhosts()
    {
        StartTimer();

        foreach (GhostMovement ghost in ghosts)
        {
            ghost.Launch();
        }
    }

    public void StopGhosts()
    {
        StopTimer();

        foreach (GhostMovement ghost in ghosts)
        {
            ghost.Stop();
        }
    }

    public void ResumeGhosts()
    {
        StartTimer();

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

    public bool HasGhosts()
    {
        return ghosts.Count >= 1 ? true : false;
    }

    #endregion

    #region Movement

    private void LaunchMovement()
    {
        LaunchPlayer();
        InitTimer();
    }

    private void StopAllElements()
    {
        StopGhosts();

        LaunchPlayer();

        InitTimer();
    }

    #endregion

    #region Timer

    private void InitTimer()
    {
        _currentTurnTimer = turnTimer;
        UpdateTimer();
    }

    public void StartTimer()
    {
        _canTimerRun = true;
    }

    public void StopTimer()
    {
        _canTimerRun = false;
    }

    private void UpdateTimer()
    {
        uiManager.DrawTimer(_currentTurnTimer, turnTimer);
       // timerText.text = string.Format("Timer : {0}", Mathf.RoundToInt(_currentTurnTimer).ToString());
    }

    private void CheckEndTimer()
    {
        if (_currentTurnTimer <= 0)
        {
            _currentTurnTimer = 0;

            EndTimer();
        }
    }

    private void EndTimer()
    {
        StopAllElements();
    }

    #endregion

    #region Buttons

    private void SetUIButtons()
    {
        uiManager.startButton.onClick.AddListener(LaunchMovement);
        uiManager.reloadButton.onClick.AddListener(ReloadScene);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
