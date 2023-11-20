using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
 
    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.OnVolumeChanged);
    }
}