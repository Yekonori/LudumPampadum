using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Rendering;

public class GameManagerController : MonoBehaviour
{
    #region Script Parameters

    [Header("Players & Ghost")]

    [SerializeField] private CharacterMovement prefab;
    [SerializeField] private Transform world;
    [SerializeField] private Button startButton;

    [Header("Timer")]

    [SerializeField] private float turnTimer;
    [SerializeField] private UIManager uiManager;

    [Header("Debug Mode")]

    [SerializeField] private bool onDebugMode;
    [SerializeField] private Button reloadButton;

    private bool canPlay = true;
    List<CharacterMovement> characterMovements = new List<CharacterMovement>();

    Vector2 input;

    #endregion

    #region Fields

    private static GameManagerController _instance;
    public static GameManagerController Get
    {
        get { return _instance; }
    }

    //private List<GhostMovement> ghosts;

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
    }

    private void Start()
    {
        InitTimer();

        //characterMovements = new List<GhostMovement>();
        characterMovements.Add(prefab);

        startButton.onClick.AddListener(RewindTime);

        if (onDebugMode)
        {
            SetDebugMode();
        }
    }

    private void Update()
    {
        UpdateController();
        if (_canTimerRun)
        {
            _currentTurnTimer -= Time.deltaTime;
            UpdateTimer();
            CheckEndTimer();
        }
    }

   /* private void FixedUpdate()
    {
        if (_canTimerRun)
        {
            _currentTurnTimer -= Time.fixedDeltaTime;
            UpdateTimer();
            CheckEndTimer();
        }
    }*/

    private void UpdateController()
    {
        if(canPlay == true)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (input == Vector2.zero)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
                characterMovements[characterMovements.Count - 1].MoveCharacterWorld(input.x, input.y);
            }

            if (Input.GetButtonDown("Fire3"))
                RewindTime();

        }
    }

    #endregion

    #region Players

    /*private void LaunchPlayer()
    {
        CreateGhost();

        playerPrefab.Launch();
    }*/

    #endregion

    #region Ghosts

    private void CreatePlayer()
    {
        CharacterMovement newPlayer = Instantiate(prefab, characterMovements[0].Positions[0], Quaternion.identity, world);
        //newPlayer.gameObject.SetActive(true);
        characterMovements.Add(newPlayer);
    }

    /*public void LaunchGhosts()
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
    }*/

    #endregion

    #region Movement

    /*private void LaunchMovement()
    {
        LaunchPlayer();
        InitTimer();
    }

    private void StopAllElements()
    {
        StopGhosts();

        LaunchPlayer();

        InitTimer();
    }*/

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
        SetCharactersMovements(1f);
    }

    public void StopTimer()
    {
        _canTimerRun = false;
        SetCharactersMovements(0f);
    }

    private void UpdateTimer()
    {
        uiManager.DrawTimer(_currentTurnTimer, turnTimer);
    }

    private void CheckEndTimer()
    {
        if (_currentTurnTimer <= 0)
        {
            _currentTurnTimer = 0;

            RewindTime();
        }
    }

    /*private void EndTimer()
    {
        StopAllElements();
    }*/

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





    public void SetCharactersMovements(float value)
    {
        for (int i = 0; i < characterMovements.Count; i++)
        {
            characterMovements[i].SetAnimationSpeed(value);
        }
    }

    public void RewindTime()
    {
        canPlay = false;
        characterMovements[characterMovements.Count - 1].RewindReplay();
        StartCoroutine(RewindTimeCoroutine());
    }

    private IEnumerator RewindTimeCoroutine()
    {
        float animationSpeed = -2f;
        SetCharactersMovements(animationSpeed);
        while (_currentTurnTimer < turnTimer)
        {
            /*if (animationSpeed > -3)
                animationSpeed -= 0.05f;*/
            _currentTurnTimer += Time.deltaTime * -animationSpeed;
            UpdateTimer();
            yield return null;
        }
        SetCharactersMovements(1);
        characterMovements[characterMovements.Count - 1].PlayReplay();
        CreatePlayer();
        canPlay = true;
    }

    #endregion
}
