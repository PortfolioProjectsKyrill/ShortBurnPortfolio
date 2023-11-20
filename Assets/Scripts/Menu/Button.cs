using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent onMouseDown;//for when the button gets clicked
    public UnityEvent onMouseEnter;//for when the button gets selected
    public UnityEvent onMouseExit;//for when the button is not selected anymore

    [Header("Popin/Out")]
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float _endPosOffset;   
    [SerializeField] private float _popOutAnimDuration;
    [SerializeField] private float _popOutMoveLength;

    private Color _normalColor;
    [SerializeField] private Color _popOutColor;

    private void Awake()
    {
        startPos = transform.localPosition;
        Vector3 temp = startPos;
        //add some offset so the player has some feedback
        temp.z -= _endPosOffset;
        endPos = temp;
    }

    private void OnMouseDown()
    {
        Debug.Log("button has been pressed");
        onMouseDown.Invoke();
    }

    private void OnMouseEnter()
    {
        onMouseEnter.Invoke();
    }

    private void OnMouseExit()
    {
        onMouseExit.Invoke();
    }

    /// <summary>
    /// When hovered lerps to certain set position to see the button is selected
    /// </summary>
    public void PopOut()
    {
        StartCoroutine(PopOutButton(endPos, _popOutAnimDuration));
    }

    /// <summary>
    /// Restores button to original not selected state
    /// </summary>
    public void PopIn()
    {
        StartCoroutine(PopInButton(startPos, _popOutAnimDuration));
    }

    public IEnumerator PopInButton(Vector3 l_endPos, float sec)
    {
        float time = 0;
        //set start position for use in Slerp
        Vector3 startPos = transform.localPosition;

        //move from og pos, to final button position
        while (time < sec)
        {
            transform.localPosition = Vector3.Slerp(startPos, l_endPos, time / sec);
            time += Time.deltaTime;
            yield return null;
        }

        //double make sure that slerp finishes
        transform.localPosition = l_endPos;
        //change exposure/vibrance slightly
        GetComponent<SpriteRenderer>().color = _normalColor;
    }

    public IEnumerator PopOutButton(Vector3 l_endPos, float sec)
    {
        _normalColor = GetComponent<SpriteRenderer>().color;

        float time = 0;
        Vector3 startPos = transform.localPosition;

        //move from og pos, to final button position
        while (time < sec)
        {
            transform.localPosition = Vector3.Slerp(startPos, l_endPos, time / sec);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = l_endPos;
        //change exposure/vibrance slightly
        GetComponent<SpriteRenderer>().color = _popOutColor;
    }
}
