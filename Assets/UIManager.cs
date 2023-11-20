using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private Color _normalColor;
    [SerializeField] private Color _popOutColor;

    [SerializeField] private CinemachineVirtualCamera[] vCams;

    [Header("ScriptableObjects")]
    [SerializeField] private Settings _currentSettings;
    [SerializeField] private Settings _mainSettings;

    [Header("Settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private LiftHandler _bigLiftHandler;
    [SerializeField] private SC_FPSController _fpsController;

    private void Awake()
    {
        if (Instance != null)
            Instance = this;
        else
            Destroy(Instance);
    }

    private void Start()
    {

    }

    private void Update()
    {
        ApplySettings();
    }

    public void SwitchCameras()
    {
        if (vCams[0].enabled && !vCams[1].enabled)
        {
            vCams[1].enabled = true;
            vCams[0].enabled = false;
        }
        else if (!vCams[0].enabled && vCams[1].enabled)
        {
            vCams[0].enabled = true;
            vCams[1].enabled = false;
        }
    }

    private void ApplySettings()
    {
        if (!CheckSettingChanges())//if settings aren't the same
        {
            _mainSettings = _currentSettings;
            Debug.Log("Settings have been changed (I hope...)");
            //SetSettings();
        }
    }

    private bool CheckSettingChanges()
    {
        if (_currentSettings != _mainSettings)//if they arent the same
        {
            Debug.Log("false");
            return false;
        }
        else
        {
            Debug.Log("true");
            return true;
        }
    }

    //private void SetSettings()
    //{
    //    //Graphics
    //    Screen.SetResolution((int)_mainSettings.displayResolution.x, (int)_mainSettings.displayResolution.y, _mainSettings.fullScreen);
    //    QualitySettings.SetQualityLevel(_mainSettings.graphicsQuality);
    //    QualitySettings.masterTextureLimit = _mainSettings.textureQuality;
    //    QualitySettings.antiAliasing = _mainSettings.antiAlias;
    //    _camera.fieldOfView = _mainSettings.fov;

    //    //Sound
    //    _mixer.SetFloat("Master", _mainSettings.masterVolume);
    //    _mixer.SetFloat("Music", _mainSettings.musicVolume);
    //    _mixer.SetFloat("SoundFX", _mainSettings.soundEffects);
    //    _mixer.SetFloat("VoiceOver", _mainSettings.voiceVolume);

    //    //Controls
    //    _bigLiftHandler.key = _mainSettings.pickUp;
    //    _fpsController.lookSpeed = _mainSettings.sensitivity;
    //}
}
