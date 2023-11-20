using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private bool _slideToLeft;

    [SerializeField] private Vector3 _endPos;

    private void Start()
    {
        _startPos = GetComponent<RectTransform>().position;
        print(gameObject.name + "'s position is: " + _startPos);

        RegulateSlideSide();
    }

    public IEnumerator SlideIn(Vector3 l_endPos, float sec)
    {
        float time = 0;
        Vector3 startPos = _startPos;
        Vector3 endPos = l_endPos;

        while (time < sec)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time / sec);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator SlideOut(Vector3 l_endPos, float sec)
    {
        float time = 0;
        Vector3 startPos = _startPos;
        Vector3 endPos = l_endPos;

        while (time < sec)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time / sec);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void RegulateSlideSide()
    {
        if (_slideToLeft)
        {
            Vector3 pos = Vector3.zero;
            pos.x = 500;
            _endPos = _startPos + pos;
        }
        else if (!_slideToLeft)
        {
            Vector3 pos = Vector3.zero;
            pos.x = -500;
            _endPos = _startPos + pos;
        }
    }
}
