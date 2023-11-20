using UnityEngine;

/// <summary>
/// This class controls the state of the animator.
/// </summary>
public class AnimationController : MonoBehaviour
{
    #region Variables 

    #region Animator
    private Animator animator;
    #endregion

    #region Bool
    [SerializeField] private bool animatorDisabling;
    #endregion

    #endregion

    #region Functions.
    #region Start()
    private void Start() 
    {
        // This is to get the component so we can change it values.
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Update()
    private void Update() { if (animatorDisabling) { StopAnimation(); } }
    #endregion
 
    #region StopAnimation()
    /// <summary>
    /// If you want to disable the animator from keep setting of the animation.
    /// </summary>
    public void StopAnimation() { animator.enabled = false; }
    #endregion
    #endregion

}