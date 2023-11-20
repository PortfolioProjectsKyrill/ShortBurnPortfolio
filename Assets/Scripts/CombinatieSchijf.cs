using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CombinatieSchijf : MonoBehaviour
{
    public int combiNumber;

    private Vector3 rot;

    private readonly float _rotAmount = 13.846153846153846153846153846154f;

    public bool numberMatches;

    private void Start()
    {
        combiNumber = 1;
    }

    private void MoveUp()
    {
        rot.x += _rotAmount;
        transform.Rotate(Vector3.up, _rotAmount);
        combiNumber--;
    }
    private void MoveDown()
    {
        rot.x -= _rotAmount;
        transform.Rotate(Vector3.down, _rotAmount);
        combiNumber++;
    }

    private void OnMouseOver()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //scroll up
            MoveUp();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //scroll down
            MoveDown();
        }
    }
}
