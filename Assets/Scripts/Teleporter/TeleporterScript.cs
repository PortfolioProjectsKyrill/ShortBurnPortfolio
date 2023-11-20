using UnityEngine;
using System.Collections;

/// <summary>
/// This Class makes it so the player can teleport.
/// </summary>
public class TeleporterScript : MonoBehaviour
{
    #region Variables
    #region Bool.
    /// <summary>
    /// This bool checks if the player is standing on the teleport.
    /// </summary>
    public bool isStandingOn = false;
    #endregion

    #region Int.
    /// <summary>
    /// This is so you can say which number you want to teleport to.
    /// </summary>
    [SerializeField] private int teleportNumber;
    #endregion

    #endregion

    #region Functions

    #region Update()
    private void Update()
    {
        // These sequence of checks is for to teleporter to work.
        if (TeleportManager.Instance != null)
        {
            if (TeleportManager.Instance.isFueled && isStandingOn && !TeleportManager.Instance.isCooldown && Input.GetKeyDown(KeyCode.T))
            {
                TeleportPlayer(teleportNumber);
            }
        }

        if (!TeleportManager.Instance.isFueled && Input.GetKeyDown(KeyCode.T))
        {
            TextManager.Instance.WhatToDisplay(TextManager.Instance.dialogueCell2[4]);
        }
    }
    #endregion  

    #region TeleportPlayer(int) and Cooldown().
    /// <summary>
    /// This function is to teleport the player to the designated number given.
    /// </summary>
    /// <param name="numberToTeleport"></param>
    private void TeleportPlayer(int numberToTeleport)
    {
        TeleportManager.Instance.player.GetComponent<CharacterController>().enabled = false;

        TeleportManager.Instance.player.transform.position = TeleportManager.Instance.otherTeleportations[numberToTeleport].position;
        StartCoroutine(Cooldown());

        TeleportManager.Instance.player.GetComponent<CharacterController>().enabled = true;
    }

    #region Cooldown()
    /// <summary>
    /// We don't want the player to infinitely keep teleporting so with this cooldown we stop that from happening.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cooldown()
    {
        TeleportManager.Instance.isCooldown = true;
        yield return new WaitForSeconds(TeleportManager.Instance.cooldownTime);
        TeleportManager.Instance.isCooldown = false;
        StopCoroutine(Cooldown());
    }
    #endregion
    #endregion

    #region Collision Checks.

    #region Enter(Collision)
    private void OnCollisionEnter(Collision other)
    {
        // For this check it is to see if the telporter is getting fueled.
        if (other.gameObject.CompareTag("FuelCell"))
        {
            TeleportManager.Instance.isFueled = true;
            Destroy(other.gameObject);
        }
    }
    #endregion

    #region Stay(Collision)
    private void OnCollisionStay(Collision other)
    {
        // if the player is standimg on the teleporter.
        if (other.gameObject.CompareTag("Small Character"))
        {
            isStandingOn = true;
        }
    }
    #endregion

    #region Exit(Collision).
    private void OnCollisionExit(Collision other)
    {
        // If the player exits the collision it is not standing on the teleporter so we have to shut it off. 
        if (other.gameObject.CompareTag("Small Character"))
        {
            isStandingOn = false;
        }
    }
    #endregion

    #endregion
    #endregion
}