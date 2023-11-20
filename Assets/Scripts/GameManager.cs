using System.Collections;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("General")]
    public static GameManager Instance;
    public CharacterController playerMovement;
    public bool gameCompleted = false;

    [Header("InspectRayCast.cs")]
    public GameObject vCam;

    [SerializeField] private GameObject _tempEndScreen;

    [Tooltip("Array of Fps Controllers, fill manually (big first then small)")]
    public SC_FPSController[] _fpsController;

    [Tooltip("Array of booleans, made to keep track of which character can move in combination with the pause menu (doesn't need to be filled)")]
    public bool[] _fpsControllerBools;

    public bool tempEndScene;

    // Bool check for if a cell has been completed.
    public bool cellOneComplete;
    public bool cellTwoComplete;
    public bool cellThreeComplete;

    public bool cutSceneIsPlaying;
    public Image crossHair;

    [SerializeField] private bool[] _isCell;
    private readonly int _cellAmount = 3;
    [SerializeField] private TempDoor[] tempDoor;
    public bool _enableDebugs = false;

    [Tooltip("Needs assigning in editor")]
    [SerializeField] private CutsceneManager _cutsceneManager;

    #region Private Functions

    #region Awake() 

    private void Awake()
    {
        // singelton check.
        Instance = this;

        _fpsControllerBools = new bool[2];

        _isCell = new bool[_cellAmount];

        // Hides the cursor and locks it in place so that it doesn't mess up the game.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion Awake()

    private void Start()
    {
        // In the start of the game we want the screen to fade from a black screen to the scene.
        if (Fading.instance != null)
        {
            Fading.instance.FadeIn();
        }
    }

    #region Update()

    private void Update()
    {
        // Checks if there is a cutscene manager
        if (_cutsceneManager != null)
        {
            // This is so the right cutscene will be played in the right cell.
            int index = _cutsceneManager.CurrentCellNumber();

            for (int i = 0; i < _isCell.Length; i++)
            {
                if (i == index)
                {
                    _isCell[i] = true;
                }
                else
                {
                    _isCell[i] = false;
                }
            }
        }
    }

    #endregion Update()
    #endregion Private Functions

    #region General Functions.

    #region CursorManager()

    /// <summary>
    /// This function we can control the state of the mouse cursor so that we can enable it when we want it and turn it off if we don't.
    /// </summary>
    public void CursorManager()
    {
        if (UnityEngine.Cursor.visible == true || UnityEngine.Cursor.lockState == CursorLockMode.Confined)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        else if (UnityEngine.Cursor.visible == false || UnityEngine.Cursor.lockState == CursorLockMode.Locked)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        }
    }

    #endregion CursorManager()

    #region UnlockTransitionDoor()

    /// <summary>
    /// Use this function to unlock the door with the 
    /// </summary>
    public void UnlockTransitionDoor()
    {
        if (_isCell[0])
        {
            tempDoor[0]._doorUnlocked = true;
        }
        else if (_isCell[1])
        {
            tempDoor[1]._doorUnlocked = true;
        }
        else if (_isCell[2])
        {
            tempDoor[2]._doorUnlocked = true;
        }
    }

    #endregion UnlockTransitionDoor()

    #region TempEndScene()

    /// <summary>
    /// Because we had no assets in the other cells we use this for a simple fix.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TempEndScene()
    {
        gameCompleted = true;

        _tempEndScreen.SetActive(true);

        CanvasGroup canvasGroup = _tempEndScreen.GetComponent<CanvasGroup>();

        for (int i = 0; i < 100; i++)
        {
            canvasGroup.alpha += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

        DisableAllPlayers(false);

        Time.timeScale = 0f;
    }

    #endregion TempEndScene()

    #region DisableAllPlayers(bool disableCrosshair)

    /// <summary>
    /// This function allows us to disable the player movements.
    /// </summary>
    public void DisableAllPlayers(bool disableCrosshair)
    {
        for (int i = 0; i < _fpsControllerBools.Length; i++)
        {
            _fpsController[i].canMove = false;
        }

        if (disableCrosshair)
            crossHair.enabled = false;
    }

    #endregion DisableAllPlayers(bool disableCrosshair)
    #endregion General Functions.
}