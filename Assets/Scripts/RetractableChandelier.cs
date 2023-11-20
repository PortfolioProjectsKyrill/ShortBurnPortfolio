using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractableChandelier : MonoBehaviour
{
    [SerializeField] private float _floorHeight, _ceilingHeight;

    public bool movingChandelier;
    private float _requiredHeight;
    private void Update()
    {
        if (movingChandelier)
        {
            //If at desired height
            if ((_requiredHeight == _ceilingHeight && transform.position.y > _requiredHeight - 0.05f) ||
            (_requiredHeight == _floorHeight && transform.position.y < _requiredHeight + 0.05f))
            {
                movingChandelier = false;
            }
            MoveChandelier(_requiredHeight);
        }
    }
    /// <summary>
    /// Moves the chandelier to given height.
    /// </summary>
    /// <param name="l_height">Desired height.</param>
    private void MoveChandelier(float l_height)
    {
        float l_smoothness = 0.25f;
        Vector3 targetPosition = new Vector3(transform.position.x, l_height, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, l_smoothness * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Small Character"))
        {
            _requiredHeight = (_requiredHeight == _floorHeight) ? _ceilingHeight : _floorHeight;
            movingChandelier = true;
        }
    }

    public void SetRequiredHeight()
    {
        _requiredHeight = (_requiredHeight == _floorHeight) ? _ceilingHeight : _floorHeight;
        movingChandelier = true;
    }
}
