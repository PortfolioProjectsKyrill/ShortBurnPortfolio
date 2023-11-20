using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath[] cineSmoothPath;

    [SerializeField] private CinemachineVirtualCamera[] _vCams;
    [SerializeField] public List<CinemachineSmoothPath.Waypoint[]> listofwaypointarrays;
    [SerializeField] public List<CinemachineSmoothPath.Waypoint> _currentVCamWaypoints;//this is a list bc it can have more than usual waypoints
    [Tooltip("")]
    public CinemachineSmoothPath.Waypoint[] _1stVCamWaypoints;
    public CinemachineSmoothPath.Waypoint[] _2ndVCamWaypoints;
    public CinemachineSmoothPath.Waypoint[] _3rdVCamWaypoints;

    [Tooltip("current Gameobject used to send to vcams")]
    [SerializeField] private GameObject _currentLookAtGameobject;
    [Tooltip("fill in the inspector, match index along with levelcount")]
    [SerializeField] private GameObject[] lookatGameobjects;

    [Tooltip("The time is takes a CinemachineVirtualCamera to lerp from point A to B")]
    [SerializeField] private float CameraLerpTime;

    private void Awake()
    {
        listofwaypointarrays = new List<CinemachineSmoothPath.Waypoint[]>();
        _currentVCamWaypoints = new List<CinemachineSmoothPath.Waypoint>();
    }

    private void Start()
    {
        //assign waypoints, lookouts, follow
        AddAllArrays();
        AssignWaypointsAtStart();
        DisableAllVCams();
    }

    private void Update()
    {
        DecideCurrentWaypointArray(CutsceneManager.Instance._cellIndex);

        if (GameManager.Instance.cutSceneIsPlaying)
            GameManager.Instance.DisableAllPlayers(true);
        else
            GameManager.Instance.crossHair.enabled = true;
    }

    /// <summary>
    /// Sets all the vars of the dolly track so we can switch to different cut scenes with the same gameObject
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="lookat"></param>
    /// <param name="levelindex"></param>
    private void AssignWayPoints(CinemachineSmoothPath.Waypoint[] positions, Transform lookat, int levelindex)
    {
        //assign positions
        cineSmoothPath[levelindex].m_Waypoints = positions;
        //enable the camera
        for (int i = 0; i < _vCams.Length; i++)
        {
            if (i != levelindex)
            {
                _vCams[i].enabled = false;
            }
            else if (i == levelindex)
            {
                _vCams[i].enabled = true;
            }
        }
        _vCams[levelindex].enabled = true;
        //set follow and look at
        _vCams[levelindex].LookAt = lookat;
        _vCams[levelindex].Follow = lookat;
    }

    public void PlayTrack(int levelIndex)
    {
        EnableVCam(levelIndex);//enable vcams
        GameManager.Instance.cutSceneIsPlaying = true;

        DecideLookat(levelIndex);//assign follow, lookat in vcam
        StartCoroutine(PathPosLerp(_vCams[levelIndex], CameraLerpTime, levelIndex));//disables the vcams at end of coroutine
    }

    private void DecideLookat(int cellIndex)
    {
        _currentLookAtGameobject = lookatGameobjects[cellIndex];
    }

    /// <summary>
    /// called in start
    /// </summary>
    private void DisableAllVCams()
    {
        //disable every vcam connected to dolly
        for (int i = 0; i < _vCams.Length; i++)
        {
            _vCams[i].enabled = false;
        }
    }

    /// <summary>
    /// Lerps the CinemachineVirtualCamera's from point 0 to 1 with time.
    /// </summary>
    /// <param name="vcam"></param>
    /// <param name="sec"></param>
    /// <returns></returns>
    private IEnumerator PathPosLerp(CinemachineVirtualCamera vcam, float sec, int l_levelIndex)
    {
        float time = 0;
        float startPos = 0;
        float endPos = 1;

        while (time < sec)
        {
            vcam.GetComponent<CinemachineTrackedDolly>().m_PathPosition = Mathf.Lerp(startPos, endPos, time / sec);
            time += Time.deltaTime;
            yield return null;
        }

        vcam.GetComponent<CinemachineTrackedDolly>().m_PathPosition = endPos;

        GameManager.Instance.cutSceneIsPlaying = false;//used to disable player during cutscenes
        DisableVCam(l_levelIndex);
    }

    private void AssignWaypointsAtStart()
    {
        for (int i = 0; i < listofwaypointarrays.Count; i++)
        {
            AssignWayPoints(listofwaypointarrays[i], lookatGameobjects[i].transform, i);
        }
    }

    private void AddAllArrays()
    {
        listofwaypointarrays.Add(_1stVCamWaypoints);
        listofwaypointarrays.Add(_2ndVCamWaypoints);
        listofwaypointarrays.Add(_3rdVCamWaypoints);
    }

    private void DecideCurrentWaypointArray(int l_cellIndex)
    {
        _currentVCamWaypoints = listofwaypointarrays[l_cellIndex].ToList();
    }

    private void EnableVCam(int l_cellIndex)
    {
        _vCams[l_cellIndex].enabled = true;
        _vCams[l_cellIndex].Priority = 50;
    }

    private void DisableVCam(int l_cellIndex)
    {
        _vCams[l_cellIndex].Priority = 0;
        _vCams[l_cellIndex].enabled = false;
    }
}
