using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForSelection : MonoBehaviour
{
    public bool _isSelected;
    private DropDown _dropDown;

    private void Awake()
    {
        _dropDown = GetComponentInParent<DropDown>();
    }

    private void OnMouseEnter()
    {
        _isSelected = true;
    }

    private void OnMouseExit()
    {
        _isSelected = false;
    }

    private void OnMouseDown()
    {
        _dropDown.ReplaceCurrentSelection();
        _dropDown._selectedOption = gameObject;
        _dropDown.DeHighlightSelectedSettingControl();
        _dropDown.HighlightSelectedSettingControl();
    }
}
