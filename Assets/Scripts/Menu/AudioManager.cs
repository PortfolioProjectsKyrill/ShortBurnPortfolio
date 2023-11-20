using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string     volumeParameterName = "MainVolume";

    private AudioMixerGroup    audioGroup;
    private AudioMixerSnapshot defaultSnapshot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioGroup = audioMixer.FindMatchingGroups("Master")[0];
        defaultSnapshot = audioMixer.FindSnapshot("Default");
    }

    public void OnVolumeChanged(float volume)
    {
        float volumeDB = Mathf.Log10(volume) * 20f;

        audioMixer.SetFloat("MainVolume", volumeDB);
    }

    public void MuteAudio()
    {
        audioMixer.SetFloat(volumeParameterName, -80f);
    }

    public void RestoreDefaultVolume()
    {
        defaultSnapshot.TransitionTo(0f);
    }
}