using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatValue : MonoBehaviour
{
    public float value;

    private void Update()
    {
        value = GetComponent<Slider>().value;
    }
}
