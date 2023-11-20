using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Settings", order = 1)]
public class Settings : ScriptableObject
{
    [Header("Graphics")]
    public Vector2 displayResolution;
    public int graphicsQuality;
    public int textureQuality;
    public int antiAlias;
    public float fov;
    public FullScreenMode fullScreen;

    [Header("Sound/Subtitles")]
    public float masterVolume;
    public float musicVolume;
    public float soundEffects;
    public float voiceVolume;

    [Header("Controls")]
    public KeyCode pickUp;
    public float sensitivity;
}
