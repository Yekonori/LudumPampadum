﻿using System.Collections;
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
    [SerializeField] private int maxGhost;

    [Header("Timer")]

    [SerializeField] private float turnTimer;
    [SerializeField] private float fastForwardBonus = 1f;
    [SerializeField] private UIManager uiManager;

    [Header("Feedback")]
    [SerializeField] Animator rewindFeedback;

    private float fastForward = 1f;
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

    int layerMask = 1 << 0;

    private int numberOfScene;
    private int activeSceneIndex;

    CameraController camera;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (_instance != this)
        {
            _instance = this;
        }
        camera = Camera.main.GetComponent<CameraController>();
        numberOfScene = SceneManager.sceneCountInBuildSettings;
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        InitTimer();

        SetUIButtons();

        characterMovements.Add(Instantiate(prefab, prefab.transform.position, Quaternion.identity, world));
        prefab.gameObject.SetActive(false);

        uiManager.DrawEntity(maxGhost);
        camera.SetFocus(characterMovements[0].transform);
        camera.transform.position = characterMovements[0].transform.position;
    }

    private void Update()
    {
        UpdateController();
        if (_canTimerRun)
        {
            _currentTurnTimer -= (Time.deltaTime * fastForward);
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
            if (Input.GetMouseButtonDown(0))
                CastRayWorld();
            else
            {
                input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (input == Vector2.zero && characterMovements[characterMovements.Count - 1].MoveAuto == false && !Input.GetButton("Fire1"))
                {
                    StopTimer();
                }
                else if (input != Vector2.zero)
                {
                    StartTimer();
                    characterMovements[characterMovements.Count - 1].MoveCharacterCamera(input.x, input.y);
                    characterMovements[characterMovements.Count - 1].MoveAuto = false;
                }
                if (Input.GetButtonDown("Fire3"))
                    RewindTime();
                if (Input.GetButtonDown("Fire2"))
                    ReloadScene();

                if (Input.GetButtonDown("Fire1"))
                {
                    StartTimer(fastForwardBonus);                  
                }
                else if (Input.GetButtonUp("Fire1"))
                    StopTimer();

            }
        }
    }


    private void CastRayWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            characterMovements[characterMovements.Count - 1].MoveAutoTo(new Vector3(hit.point.x, 0, hit.point.z));
            StartTimer();
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
        CharacterMovement currentPlayer = characterMovements[characterMovements.Count - 1];
        currentPlayer.isAlixModel = true;
        currentPlayer.UpdateModel();

        CharacterMovement newPlayer = Instantiate(prefab, characterMovements[0].Positions[0].Positions, Quaternion.identity, world);
        newPlayer.gameObject.SetActive(true);
        characterMovements.Add(newPlayer);

        uiManager.DrawEntity(maxGhost - (characterMovements.Count - 1));
        AudioManager.instance.Play("SFX_Spawn", false);

        camera.SetFocus(newPlayer.transform);
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

    public void StartTimer(float time = 1f)
    {
        fastForward = time;
        _canTimerRun = true;
        SetCharactersMovements(fastForward);
    }

    public void StopTimer()
    {
        fastForward = 1f;
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

    #region Buttons

    private void SetUIButtons()
    {
        uiManager.startButton.onClick.AddListener(RewindTime);
        uiManager.reloadButton.onClick.AddListener(ReloadScene);
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
        if (canPlay == true)
        {
            canPlay = false;
            characterMovements[characterMovements.Count - 1].RewindReplay();
            StartCoroutine(RewindTimeCoroutine());
            AudioManager.instance.Play("SFX_Rewind_Start", false);
            AudioManager.instance.Play("SFX_Rewinding", false);
            AudioManager.instance.MusicPitchEffectOn();
        }
    }

    private IEnumerator RewindTimeCoroutine()
    {
        StopTimer();
        float animationSpeed = 0f;
        rewindFeedback.SetBool("Rewind", true);
        uiManager.RewindOverlayFeedback(true);
        for (int i = 0; i < characterMovements.Count; i++)
        {
            characterMovements[i].SetAnimationSpeed(animationSpeed);
            characterMovements[i].gameObject.layer = 9;
        }

        while (_currentTurnTimer < turnTimer)
        {
            if (animationSpeed > -3)
            {
                animationSpeed -= 0.02f;
                SetCharactersMovements(animationSpeed);
            }
            _currentTurnTimer += Time.deltaTime * Mathf.Abs(animationSpeed);
            UpdateTimer();
            yield return null;
        }

        rewindFeedback.SetBool("Rewind", false);
        uiManager.RewindOverlayFeedback(false);
        for (int i = 0; i < characterMovements.Count; i++)
        {
            characterMovements[i].SetAnimationSpeed(1);
            characterMovements[i].gameObject.layer = 8;
            characterMovements[i].SetPosition(prefab.transform.position);
            characterMovements[i].PlayReplay();
        }

        if (characterMovements.Count - 1 < maxGhost)
        {
            characterMovements[characterMovements.Count - 1].PlayReplay();
            CreatePlayer();
        }
        else
        {
            characterMovements[characterMovements.Count - 1].InReplay = false;
            characterMovements[characterMovements.Count - 1].CanRecord = true;
            characterMovements[characterMovements.Count - 1].ClearPosition();
        }

        canPlay = true;
        AudioManager.instance.Stop("SFX_Rewinding");
        AudioManager.instance.Play("SFX_Rewind_End", false);
        AudioManager.instance.MusicPitchEffectOff();
    }

    #endregion

    // Test changement de niveau
    public void WinLevel(Transform priority)
    {
        camera.SetFocusPriority(priority);
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3);
        if (activeSceneIndex == numberOfScene - 1) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(activeSceneIndex + 1);
    }
}
