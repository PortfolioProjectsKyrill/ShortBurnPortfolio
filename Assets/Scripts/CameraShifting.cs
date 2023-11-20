using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShifting : MonoBehaviour
{
    [SerializeField] private Quaternion[] rotations;
    private void Start()
    {
        rotations[0] = transform.rotation;
    }

    public void RotateSelf()
    {
        if (transform.rotation == rotations[0])
        {
            StartCoroutine(RotateCam(rotations[1], 1.5f));
        }
        else if (transform.rotation == rotations[1])
        {
            StartCoroutine(RotateCam(rotations[0], 1.5f));
        }
    }

    private IEnumerator RotateCam(Quaternion l_endRot, float sec)
    {
        float time = 0;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = l_endRot;

        while (time < sec)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, time / sec);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = l_endRot;
    }
}
