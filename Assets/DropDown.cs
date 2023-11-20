using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DropDown : MonoBehaviour
{
    [Header("Spawning")]
    [Tooltip("Assign the different options you want to have in the DropDown")]
    public GameObject[] Options;
    private Vector3 _movePos;
    private Vector3 _PosOffset = Vector3.zero;
    [SerializeField] private float _moveOffset;

    [Header("Folding In/Out")]
    [SerializeField] Vector3[] _foldOutPositions;
    [SerializeField] Vector3[] _foldInPositions;
    [SerializeField] private float _foldOutTime;
    [SerializeField] private float _foldOutOffset;
    [SerializeField] private bool _isFoldedOut;

    [Header("Logic")]
    public GameObject _selectedOption;
    public GameObject _previousOption;
    private bool _canFoldBack;
    private bool _isSelected;
    [SerializeField] private float _checkingTime;
    [SerializeField] private bool _hasbeenAdded;

    [Header("Colors/Text")]
    [SerializeField] private UnityEngine.Color _defaultColor;
    [SerializeField] private TextMeshProUGUI[] _textmp;

    private void Awake()
    {
        _foldOutPositions = new Vector3[Options.Length];
        _foldInPositions = new Vector3[Options.Length];
        _textmp = new TextMeshProUGUI[Options.Length];
    }

    private void Start()
    {
        SpawnOptionsCorrectly();
        SetDefaultPositions();
        SetFoldOutPositions();
        FillTextArray();
        DisableAllText();

        _defaultColor = Options[0].GetComponent<SpriteRenderer>().color;

        if (!_selectedOption)
        {
            _selectedOption = Options[0];
        }
    }

    private void Update()
    {
        if (CheckForSelection() && !_isFoldedOut && !_hasbeenAdded)
        {
            _hasbeenAdded = true;
            for (int i = 0; i < Options.Length; i++)
            {
                StartCoroutine(FoldOut(i, _foldOutTime));
            }
            AddBoxColliderOptions();
        }
        else if (!CheckForSelection() && _isFoldedOut && _hasbeenAdded)
        {
            _hasbeenAdded = false;
            for (int i = 0; i < Options.Length; i++)
            {
                StartCoroutine(FoldIn(i, _foldOutTime));
            }
            RemoveBoxColliderOptions();
        }
    }

    private void SpawnOptionsCorrectly()
    {
        //Vec3.zero + z offset 
        _PosOffset.z += _moveOffset;

        for (int i = 0; i < Options.Length; i++)
        {
            //set the POS of each option gameObject to vector3.zero + offset
            Options[i].transform.localPosition = _PosOffset;

            //add float offset (effectively) to Vector3 used to move
            _PosOffset.z += _moveOffset;
        }
    }

    private void OnMouseEnter()
    {
        _isSelected = true;
    }

    private void OnMouseExit()
    {
        _isSelected = false;
    }

    private void SetFoldOutPositions()
    {
        float l_tempOffsetY = _foldOutOffset;
        float l_tempOffsetZ = _moveOffset;

        for (int i = 0; i < Options.Length; i++)
        {
            _foldOutPositions[i].y -= l_tempOffsetY;
            _foldOutPositions[i].z -= l_tempOffsetZ;
            l_tempOffsetZ += _moveOffset;
            l_tempOffsetY += _foldOutOffset;
        }
    }

    private void SetDefaultPositions()
    {
        for (int i = 0; i < Options.Length; i++)
        {
            _foldInPositions[i] = Options[i].transform.localPosition;
        }
    }

    private void AddBoxColliderOptions()
    {
        for (int i = 0; i < Options.Length; i++)
        {
            Options[i].AddComponent<BoxCollider>();
        }
    }

    private void RemoveBoxColliderOptions()
    {
        for (int i = 0; i < Options.Length; i++)
        {
            if (Options[i].GetComponent<BoxCollider>())
            {
                Destroy(Options[i].GetComponent<BoxCollider>());
            }
        }
    }

    private bool CheckForSelection()
    {
        int l_amountSelected = 0;

        for (int i = 0; i < Options.Length; i++)
        {
            if (Options[i].GetComponent<CheckForSelection>()._isSelected)
            {
                l_amountSelected++;
            }
        }

        if (_isSelected)
        {
            l_amountSelected++;
        }

        if (l_amountSelected > 0)
        {
            return true;
        }
        else { return false; }
    }

    #region Folding Coroutines
    private IEnumerator FoldOut(int index, float sec)
    {
        float time = 0;
        //set start position for use in Slerp
        Vector3 startPos = _foldInPositions[index];
        Vector3 l_endPos = _foldOutPositions[index];

        //move from og pos, to final button position
        while (time < sec)
        {
            Options[index].transform.localPosition = Vector3.Lerp(startPos, l_endPos, time / sec);
            time += Time.deltaTime;
            yield return null;
        }

        //double make sure that slerp finishes
        Options[index].transform.localPosition = l_endPos;

        _isFoldedOut = true;

        ActivateAllText();
    }

    private IEnumerator FoldIn(int index, float sec)
    {
        yield return new WaitForSeconds(_checkingTime);

        if (!CheckForSelection())
        {
            _canFoldBack = true;
        }

        if (_canFoldBack)
        {
            DisableAllText();

            float time = 0;
            //set start position for use in Slerp
            Vector3 startPos = _foldOutPositions[index];
            Vector3 l_endPos = _foldInPositions[index];

            //move from og pos, to final button position
            while (time < sec)
            {
                Options[index].transform.localPosition = Vector3.Lerp(startPos, l_endPos, time / sec);
                time += Time.deltaTime;
                yield return null;
            }

            //double make sure that slerp finishes
            Options[index].transform.localPosition = l_endPos;

            _canFoldBack = false;

            _isFoldedOut = false;
        }
        else
        {
            yield return null;
        }
    }

    /// <summary>
    /// highlights the currently selected settings in the dropdown menu
    /// Run this when the selected setting changes
    /// </summary>
    public void HighlightSelectedSettingControl()
    {
        //put up the exposure of the button a bit
        _selectedOption.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
    }

    public void DeHighlightSelectedSettingControl()
    {
        if (_previousOption)
            _previousOption.GetComponent<SpriteRenderer>().color = _defaultColor;
    }

    public void ReplaceCurrentSelection()
    {
        if (_selectedOption)
            _previousOption = _selectedOption;
    }

    private void ActivateAllText()
    {
        for (int i = 0; i < _textmp.Length; i++)
        {
            _textmp[i].enabled = true;
        }
    }

    private void DisableAllText()
    {
        for (int i = 0; i < _textmp.Length; i++)
        {
            _textmp[i].enabled = false;
        }
    }

    private void FillTextArray()
    {
        for (int i = 0; i < Options.Length; i++)
        {
            _textmp[i] = Options[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    #endregion
}
