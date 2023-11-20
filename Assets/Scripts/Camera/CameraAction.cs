using Cinemachine;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    public static CameraAction Instance;

    #region Values.
  [SerializeField] private float weight;
    [SerializeField] private bool  isNextCamera;
  
    [SerializeField] private CinemachineMixingCamera cinemachineMixingCamera;
    #endregion

    #region Weight explanation.
    // cinemachineMixingCamera.m_Weight0 = the main angle/camera.
    // cinemachineMixingCamera.m_Weight1 = the other camera/angle you want.
    #endregion

    #region Functions.

    #region Awake()
    private void Awake()
    {
        #region Singelton.
        // Checks if there is already a singelton in the scene and removes itself.
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        #endregion
    }
    #endregion

    #region Start()
    private void Start() { ResetCameraWeight(); }
    #endregion

    #region HandleAnimation(bool)
    /// <summary>
    /// This handles the values given by the animation. 
    /// </summary>
    /// <param name="isNextCamera"></param>
    public void HandleAnimationAction(bool isNextCamera) => ToNextCamera(isNextCamera);
    #endregion

    #region ToNextCamera(bool)
    /// <summary>
    /// If you want to change the camera this will change the values of the camera's weight (priority). 
    /// </summary>
    /// <param name="isNextCamera"></param>
    public void ToNextCamera(bool isNextCamera)
    {
        if (isNextCamera)
        {
            cinemachineMixingCamera.m_Weight1 += weight;
        }
        else
        {
            cinemachineMixingCamera.m_Weight0 += weight;
        }
    }
    #endregion

    #region ResetCameraWeight()
    /// <summary>
    /// Since CineMachine saves during play we need to reset the values after we are done. 
    /// </summary>
    public void ResetCameraWeight()
    {
        // The why m_Weight0 = 1 is so that in the mix our main camera is the one we are using instead of the other angle.
        cinemachineMixingCamera.m_Weight0 = 1;
        cinemachineMixingCamera.m_Weight1 = 0;
    }
    #endregion

    #endregion
}