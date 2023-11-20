using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDoor : MonoBehaviour
{
    public bool isLocked = true; // Flag to track if the door is locked

    private HingeJoint doorHinge; // Reference to the hinge joint component

    [SerializeField] private bool _isCellOne;
    [SerializeField] private bool _isCellTwo;
    [SerializeField] private bool _isCellThree;

    private void Start()
    {
        // Get the hinge joint component attached to the door object
        doorHinge = GetComponent<HingeJoint>();

        // Lock the door initially
        LockDoor();
    }

    private void Update()
    {
        // Check if the puzzle has been completed and the door is locked
        if (GameManager.Instance.cellOneComplete && _isCellOne && isLocked)
        {
            UnlockDoor();
            Debug.Log("unlocked first door");
        }
        else if (GameManager.Instance.cellTwoComplete && _isCellTwo && isLocked)
        {
            UnlockDoor();
        }
        else if (GameManager.Instance.cellThreeComplete && _isCellThree && isLocked)
        {
            UnlockDoor();
        }
        else
        {
            return;
        }
    }

    private void LockDoor()
    {
        // Disable the motor to lock the door
        doorHinge.useMotor = false;

        // Update the lock flag
        isLocked = true;
    }

    private void UnlockDoor()
    {
        // Enable the motor to unlock the door
        doorHinge.useMotor = true;

        // Update the lock flag
        isLocked = false;
    }
}
