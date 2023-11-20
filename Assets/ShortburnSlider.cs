using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortburnSlider : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject _startEmpty;
    [SerializeField] private GameObject _endEmpty;

    [SerializeField] private float _sliderValue;

    private float beginValue;
    private float endValue;
    private void Start()
    {
        beginValue = _startEmpty.transform.localPosition.x;
        endValue = _endEmpty.transform.localPosition.x;
    }

    private void Update()
    {
        _sliderValue = Mathf.Clamp(value, 0, 1);


    }
}
