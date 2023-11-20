using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script handles the scenes so from resetting to loading a new scene. 
/// </summary>
public class SceneScript : MonoBehaviour
{
    public static SceneScript instance;

    #region Variables.
    [Tooltip("Use to assign key to use the pause menu")]
    public KeyCode _pauseKey;

    [Tooltip("Either pausepanel Gameobject which you want to enabled/disable or empty gameObject when used without???")]
    [SerializeField] private GameObject _pausePanel; // In some scenario's where you don't want a pause menu just use a empty GameObject.
    #endregion

    #region Functions

    #region Awake()
    private void Awake()
    {
        // This if statement checks if there's a singleton already in the scene and destroy itself if it does and fills it if it doesn't.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Update()
    private void Update()
    {
        /// This if statement checks if the player pressed escape and it is not the start menu. 
        if (Input.GetKeyDown(_pauseKey) && !IsStartMenu())
        {
            TogglePauseMenu();
        }

        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.gameCompleted && GameManager.Instance != null)
            {
                //big character
                if (GameManager.Instance._fpsController[0].canMove)
                {
                    GameManager.Instance._fpsControllerBools[0] = true;
                    GameManager.Instance._fpsControllerBools[1] = false;
                }
                //small character
                else if (GameManager.Instance._fpsController[1].canMove)
                {
                    GameManager.Instance._fpsControllerBools[0] = false;
                    GameManager.Instance._fpsControllerBools[1] = true;
                }
            }
        }
    }
    #endregion

    #region InputScene(string)
    /// <summary>
    /// This function makes it so when you put the scene name in the hierarchy it will load the scene. 
    /// </summary>
    /// <param name="sceneName"></param>
    public void InputScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region Quit()
    /// <summary>
    /// This function makes it so the game quits.
    /// </summary>
    public void Quit() { Application.Quit(); }
    #endregion

    #region RestartScene()
    /// <summary>
    /// This function gets the current scene and sends it to the InputScene where the scene gets loaded again..
    /// </summary>
    public void RestartScene() => InputScene(SceneManager.GetActiveScene().name);
    #endregion

    #region Check for start menu()
    /// <summary>
    /// This returns a true or false if you are in the start menu.
    /// This is so that the player can't pause in the start menu.
    /// </summary>
    /// <returns></returns>
    private bool IsStartMenu() { return SceneManager.GetActiveScene().name == "StartMenu"; }
    #endregion

    #region Toggle pause()
    /// <summary>
    /// This function is so when the game is paused you can resume playing it if you call the function.
    /// </summary>
    private void ResumeGame()
    {
        Time.timeScale = 1;

        if (!GameManager.Instance.gameCompleted && GameManager.Instance != null)
        {
            if (GameManager.Instance._fpsControllerBools[0])
            {
                GameManager.Instance._fpsController[0].canMove = true;
            }
            else if (GameManager.Instance._fpsControllerBools[1])
            {
                GameManager.Instance._fpsController[1].canMove = true;
            }
        }
        _pausePanel.SetActive(false);
    }

    /// <summary>
    /// This function makes it so the time pauses and the pausePanel gets activated.
    /// </summary>
    public void TogglePauseMenu()
    {
        if (_pauseKey == KeyCode.Escape)
        {

            Debug.Log("if you cant click on the buttons, change the keycode and try again. Esc also tabs out of game view and bugs it...");
        }

        GameManager.Instance.CursorManager();

        if (_pausePanel.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            Time.timeScale = 0;
            for (int i = 0; i < GameManager.Instance._fpsController.Length; i++)
            {
                GameManager.Instance._fpsController[i].canMove = false;
            }
            _pausePanel.SetActive(true);
        }
    }
    #endregion
    #endregion
}