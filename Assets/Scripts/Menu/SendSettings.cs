using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SendSettings : MonoBehaviour
{
    [Header("Graphics")]
    public DropDown displayResolution;
    public DropDown graphicsQuality;
    public DropDown textureQuality;
    public DropDown antiAlias;
    public GameObject fov;
    public DropDown fullScreen;

    [Header("Sound/Subtitles")]
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider soundEffects;
    public Slider voiceVolume;

    [Header("Controls")]
    public InputField pickUp;
    public Slider sensitivity;

    [Header("ScriptableObject")]
    [SerializeField] private Settings _currentSettings;

    //private void Update()
    //{
    //    if (_currentSettings != null)
    //    {
    //        Apply();
    //    }
    //}

    //private void Apply()
    //{
    //    //Graphics
    //    _currentSettings.displayResolution = displayResolution._selectedOption.GetComponent<Vector2Value>().vector2;
    //    _currentSettings.graphicsQuality = graphicsQuality._selectedOption.GetComponent<IntValue>().Int;
    //    _currentSettings.textureQuality = textureQuality._selectedOption.GetComponent<IntValue>().Int;
    //    _currentSettings.antiAlias = antiAlias._selectedOption.GetComponent<IntValue>().Int;
    //    _currentSettings.fov = fov.GetComponent<FloatValue>().value;
    //    _currentSettings.fullScreen = fullScreen._selectedOption.GetComponent<FullScreenMode>().fullScreenMode;

    //    //Audio | aan te passen zodra de slider werkelijk is gemaakt
    //    _currentSettings.masterVolume = masterVolume.GetComponent<FloatValue>().value;
    //    _currentSettings.musicVolume = musicVolume.GetComponent<FloatValue>().value;
    //    _currentSettings.soundEffects = musicVolume.GetComponent<FloatValue>().value;
    //    _currentSettings.voiceVolume = musicVolume.GetComponent<FloatValue>().value;

    //    //controls | aan te passen zodra de slider werkelijk is gemaakt
    //    _currentSettings.pickUp = pickUp.GetComponent<KeyCodeValue>().keyCode;
    //    _currentSettings.sensitivity = sensitivity.GetComponent<FloatValue>().value;

    //    Debug.Log("has been applied");
    //}
}
