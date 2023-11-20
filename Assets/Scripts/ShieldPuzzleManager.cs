using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShieldPuzzleManager : MonoBehaviour
{
    [Tooltip("Assign before runtime/compile, the puzzle pieces found in the scene")]
    public PuzzlePiece[] puzzlePieces;

    private bool _canSpawn;
    private Collider[] cl;
    [Tooltip("Used to send MeshColliders to and perform logic on")]
    private List<Collider> colliders;
    [Tooltip("used in the overlapshere raycast, assign to the layer assigned to the puzzle pieces")]
    public LayerMask _layerMask;
    [Tooltip("how many pieces there are (4)")]
    public int _pieceCount;
    private bool _hasStartedCoroutine;

    public static ShieldPuzzleManager instance;
    [Tooltip("Gameobject that needs to filled with the full shield gameObject")]
    [SerializeField] private GameObject _fullShield;
    [Tooltip("Variable used as a radius in the sphere raycast in the CheckForPieces() function, (default is 5)")]
    [SerializeField] private int _radius;
    [Tooltip("Visual effect is used to give some spice when completing the puzzle (doesn't need to be assigned)")]
    [SerializeField] private VisualEffect _visualEffect;

    [Header("From outline.cs")]
    [Tooltip("boolean used to confirm if all pieces are present on the outline")]
    public bool allPuzzlePieces;
    [Tooltip("tag used to check for pickups, (default: PickUp)")]
    public string _tag;
    [Tooltip("variable used to show how many pieces are in the scene, (Usually 4)")]
    public int puzzlePieceCount;
    [Tooltip("A list to check if pieces are on the outline already")]
    public List<GameObject> puzzlePieceCounter;

    private void Start()
    {
        instance = this;
        _canSpawn = false;
        _hasStartedCoroutine = false;
        allPuzzlePieces = false;

        puzzlePieceCounter = new List<GameObject>(_pieceCount);
        colliders = new List<Collider>(_pieceCount);

        _visualEffect.enabled = false;
    }

    private void Update()
    {
        //if coroutine isn't already running and all the pieces are present on the outline
        if (_hasStartedCoroutine == false && puzzlePieceCount >= 4)
        {
            //start coroutine
            StartCoroutine(CheckForPieces());

            //turns boolean true so coroutine only starts once
            _hasStartedCoroutine = true;
        }

        //if all pieces are present
        if (puzzlePieceCount == _pieceCount)
        {
            allPuzzlePieces = true;
        }
    }

    /// <summary>
    /// only gets called when there are one or more pieces touching the outline
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckForPieces()
    {
        if (GameManager.Instance._enableDebugs)
            Debug.Log("checking for pieces");

        //if the first piece is not null
        if (!_canSpawn && puzzlePieces[0] != null)
        {
            cl = Physics.OverlapSphere(puzzlePieces[0].transform.position, _radius, _layerMask);
            colliders.Clear();
        }

        //add only the meshColliders to the colliders List
        for (int i = 0; i < cl.Length; i++)
        {
            if (cl[i].GetType() == typeof(MeshCollider))
            {
                colliders.Add(cl[i]);
            }
        }

        //if the # of colliders in the colliders list are equal to the amount present in the scene and pieces on the outline are equal or more than 4
        if (colliders.Count == _pieceCount && puzzlePieceCount >= 4)
        {
            StartCoroutine(DestroyPuzzlePieces());
        }

        yield return new WaitForSeconds(0.5f);

        //starts coroutine again if level has not been completed yet
        if (!allPuzzlePieces)
        {
            _hasStartedCoroutine = false;
        }
    }

    /// <summary>
    /// used to calculate the average position between all puzzle piece positions (not really used atm)
    /// </summary>
    /// <returns></returns>
    private Vector3 AveragePuzzleLocation()
    {
        //constructors
        Vector3 l_AveragePOS = Vector3.zero;
        Vector3 l_AllFour = Vector3.zero;

        //add all the positions to vec3
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            l_AllFour += puzzlePieces[i].transform.position;
        }

        //devide by amount of pieces = average
        l_AveragePOS = l_AllFour / 4;

        //return average vector
        return l_AveragePOS;
    }

    private IEnumerator DestroyPuzzlePieces()
    {
        Vector3 vfxPos = AveragePuzzleLocation();

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            //for each puzzle piece destroy one and wait a second
            Destroy(puzzlePieces[i].gameObject);
            //doesn't wait with the last one
            if (i != 3)
            {
                yield return new WaitForSeconds(1);
            }
        }

        //if there is a visual effect assigned via inspector
        if (_visualEffect != null)
        {
            _visualEffect.enabled = true;
            _visualEffect.Play();
        }

        //enable preset shield piece
        _fullShield.SetActive(true);
        GameManager.Instance.cellOneComplete = true;
    }
}
