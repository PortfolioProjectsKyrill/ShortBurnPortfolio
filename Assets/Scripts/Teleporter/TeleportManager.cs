using UnityEngine;

/// <summary>
/// This class manages if the player meets the condition to teleport.
/// </summary>
public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance;

    #region Variables

    #region Bools.
    // These bools check if the player can teeleport or not.
    public bool        isFueled   = false;
    public bool        isCooldown = false;
    #endregion

    #region float.
    /// <summary>
    /// We don't want the player to keep teleporting so with this we can control the time in which he can teleport.
    /// </summary>
    public float       cooldownTime;
    #endregion

    #region GameObject
    /// <summary>
    /// We want want to teleport the right GameObject.
    /// </summary>
    public GameObject  player;
    #endregion

    #region Transform.
    /// <summary>
    /// This keeps an array of all the teleporters in the scene. (you need to assign them first.)
    /// </summary>
    public Transform[] otherTeleportations;
    #endregion
    #endregion

    #region Functions.

    #region Awake()
    private void Awake()
    {
        #region Singelton.
        // Checks if there is already a singelton in the scene and removes itself.
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        #endregion
    }
    #endregion

    #endregion

}