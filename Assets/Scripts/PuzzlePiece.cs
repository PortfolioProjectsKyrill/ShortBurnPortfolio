using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum PieceOutline{
    OnOutline,
    OffOutline
}

public enum PieceParent{
    NoParent,
    HasParent
}

public class PuzzlePiece : MonoBehaviour
{
    [Header("Regulating booleans")]
    public bool canSnap = false;
    private bool canStick;

    private Rigidbody _rb;

    public PieceOutline _pieceState;
    public PieceParent _pieceParent;

    private Vector3 snapTransforms;

    [Header("Stick Cooldown")]
    private float stickTimer = 0;
    [SerializeField] private float stickCooldown;

    [Header("Debug")]
    [SerializeField] private TextMeshPro _debugText;

    public bool _enableDebugs = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //if the player is holding the puzzle piece AKA removing it from the outline
        if (_pieceState == PieceOutline.OffOutline && _pieceParent == PieceParent.HasParent)
        {
            //to not collide with anything
            _rb.isKinematic = false;
            //removes itself from the the list in PuzzleShieldManager.cs and also updates the puzzlepiececount
            RemoveSelf();
        }
        //if the puzzle piece has been put on the outline and the timer has run out
        else if (_pieceState == PieceOutline.OnOutline && _pieceParent == PieceParent.NoParent && canStick == true)
        {
            //to collide with everything again
            _rb.isKinematic = true;
            //starts timer again
            canStick = false;
            //add itself to the list in PuzzleShieldManager.cs to not add itself twice
            AddSelf();
            //snaps to the first position recorded when colliding ONCE
            SnapToOutline();
        }

        //can only stick to new position if the timer runs out
        if (stickTimer < stickCooldown && canStick == false)
        {
            stickTimer += Time.deltaTime;

            //if the timer is more or equal to the assigned cooldown
            if (stickTimer >= stickCooldown)
            {
                //triggers bool
                canStick = true;
                //resets timer
                stickTimer = 0;
            }
        }
    }

    private void Update()
    {
        //regulates states of each puzzle pieces, (2 seperate enums)
        StateDoer();
    }


    #region Outline Checking

    /// <summary>
    /// regulates the enum
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Outline")
        {
            _pieceState = PieceOutline.OnOutline;
            OutlinePos(collision.transform.position);
        }
    }

    /// <summary>
    /// regulates the enum, (gets set on collisionEnter)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.name == "Outline")
        {
            _pieceState = PieceOutline.OffOutline;
        }
    }

    /// <summary>
    /// actually does the snapping to the saved position
    /// </summary>
    private void SnapToOutline()
    {
        //if it is allowed to snap, regulated by if statements that check for enums
        if (canSnap && _rb.isKinematic == false) {
            Debug.Log("isSnapping");
            //sets position
            transform.position = snapTransforms;
            //enables collision
            _rb.isKinematic = true;
        }
    }

    /// <summary>
    /// sets the outline position
    /// </summary>
    /// <param name="snapTransform"></param>
    /// <returns></returns>
    public Vector3 OutlinePos(Vector3 snapTransform)
    {
        if (GameManager.Instance._enableDebugs)
            Debug.Log("set variable snapTransforms");
        return snapTransforms = snapTransform;
    }

    /// <summary>
    /// updates the states constantly
    /// </summary>
    private void StateDoer()
    {
        switch (_pieceState)
        {
            case PieceOutline.OnOutline:
                canSnap = true;
                _rb.useGravity = false;
                break;
            case PieceOutline.OffOutline:
                canSnap = false;
                _rb.useGravity = true;
                break;
        }

        switch (_pieceParent)
        {
            case PieceParent.NoParent:
                if (transform.parent != null)
                {
                    _pieceParent = PieceParent.HasParent;
                }
                break;
            case PieceParent.HasParent:
                if (transform.parent == null)
                {
                    _pieceParent = PieceParent.NoParent;
                }
                break;
        }
    }

    /// <summary>
    /// this.gameObject removes itself from the list in ShieldPuzzleManager.cs
    /// </summary>
    private void RemoveSelf()
    {
        if (ShieldPuzzleManager.instance.puzzlePieceCounter.Contains(gameObject))
        {
            ShieldPuzzleManager.instance.puzzlePieceCounter.Remove(gameObject);
            ShieldPuzzleManager.instance.puzzlePieceCount--;
        }
    }

    /// <summary>
    /// this.gameObject adds itself from the list in ShieldPuzzleManager.cs
    /// </summary>
    private void AddSelf()
    {
        if (!ShieldPuzzleManager.instance.puzzlePieceCounter.Contains(gameObject))
        {
            ShieldPuzzleManager.instance.puzzlePieceCounter.Add(gameObject);
            ShieldPuzzleManager.instance.puzzlePieceCount++;
        }
    }
    #endregion
}
