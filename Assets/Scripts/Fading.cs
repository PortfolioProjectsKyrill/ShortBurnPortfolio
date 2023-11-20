using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public static Fading instance; // Instance of the FadeScreen MonoBehavior

    //Delegate and after fade action method
    public delegate void ActionAferFadeMethod();
    public ActionAferFadeMethod actionAfterFade;

    [SerializeField] private float _fadeStep; // How fast the fade is happening
    [SerializeField] private float _timeBeforeAction; // How much time do we wait before calling the actionAfterFade method
    private bool _callActionAfterFade; // Do we call an action after the fade?

    private float _target; // Target value of the fade screen
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
        if (!enabled)
        {
            enabled = enabled;
        }
    }

    private void Update()
    {
        //Fade out
        if (_canvasGroup.alpha < _target)
        {
            _canvasGroup.alpha += _fadeStep;
        }

        //Fade in
        if (_canvasGroup.alpha > _target)
        {
            _canvasGroup.alpha -= _fadeStep;
        }

        //If faded in
        if (_canvasGroup.alpha <= 0)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            if (_callActionAfterFade)
            {
                StartCoroutine(CallAction());
            }
        }

        //If Faded out
        if (_canvasGroup.alpha >= _fadeStep)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        if (_canvasGroup.alpha >= 1f && _callActionAfterFade)
        {
            StartCoroutine(CallAction());
        }
    }

    /// <summary>
    /// Fade the screen out, use callAction = true to call an action after the fade
    /// </summary>
    public void FadeOut(bool l_callAction = false)
    {
        _target = 1;
        _callActionAfterFade = l_callAction;
    }

    /// <summary>
    /// Fade the screen in, use callAction = true to call an action after the fade
    /// </summary>
    public void FadeIn(bool l_callAction = false)
    {
        _target = 0;
        _callActionAfterFade = l_callAction;
    }

    /// <summary>
    /// Wait before calling an action after fade
    /// </summary>
    private IEnumerator CallAction()
    {
        yield return new WaitForSeconds(_timeBeforeAction);
        actionAfterFade();
    }
}
