using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public sealed class SettingsController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer; // optional - leave empty if not using a mixer
    [SerializeField] private string volumeMixerParam = "MasterVolume";

    [Header("Mouse Sensitivity")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private float minSensitivity = 0.05f;
    [SerializeField] private float maxSensitivity = 0.5f;

    private const string VolumePrefKey = "MasterVolume";
    private const string SensitivityPrefKey = "MouseSensitivity";

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0.0001f; // avoid log10(0) = -infinity when using a mixer
            volumeSlider.maxValue = 1f;
            volumeSlider.value = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
            volumeSlider.onValueChanged.AddListener(SetVolume);
            SetVolume(volumeSlider.value);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = minSensitivity;
            sensitivitySlider.maxValue = maxSensitivity;
            sensitivitySlider.value = PlayerPrefs.GetFloat(SensitivityPrefKey, 0.1f);
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
    }

    public void SetVolume(float value)
    {
        if (audioMixer != null)
        {
            // AudioMixer volume is logarithmic (decibels), not linear
            audioMixer.SetFloat(volumeMixerParam, Mathf.Log10(value) * 20f);
        }
        else
        {
            AudioListener.volume = value;
        }

        PlayerPrefs.SetFloat(VolumePrefKey, value);
        PlayerPrefs.Save();
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(SensitivityPrefKey, value);
        PlayerPrefs.Save();
    }
}