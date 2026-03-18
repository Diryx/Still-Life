using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Slider))]
public class UISliderHandler : MonoBehaviour
{
    [SerializeField] private VolumeType _volumeType;

    private Slider _slider;
    private SettingsManager _settingsManager;
    private AudioManager _audioManager;

    public enum VolumeType
    {
        Master,
        Music,
        SFX
    }

    [Inject]
    private void Construct(SettingsManager settingsManager, AudioManager audioManager)
    {
        _settingsManager = settingsManager;
        _audioManager = audioManager;
    }

    private void Awake() => _slider = GetComponent<Slider>();

    private void Start()
    {
        switch (_volumeType)
        {
            case VolumeType.Master:
                _settingsManager.OnMasterVolumeChanged += UpdateSliderValue;
                break;
            case VolumeType.Music:
                _settingsManager.OnMusicVolumeChanged += UpdateSliderValue;
                break;
            case VolumeType.SFX:
                _settingsManager.OnSFXVolumeChanged += UpdateSliderValue;
                break;
        }

        InitializeSlider();
    }

    private void InitializeSlider()
    {
        float initialValue = GetCurrentVolume();

        _slider.SetValueWithoutNotify(initialValue);
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void UpdateSliderValue(float newValue) => _slider.SetValueWithoutNotify(newValue);


    private void OnSliderValueChanged(float value)
    {
        Debug.Log($"OnSliderValueChanged: {_volumeType} = {value}");

        switch (_volumeType)
        {
            case VolumeType.Master:
                _settingsManager.SetMasterVolume(value);
                break;
            case VolumeType.Music:
                _settingsManager.SetMusicVolume(value);
                break;
            case VolumeType.SFX:
                _settingsManager.SetSFXVolume(value);
                break;
        }
    }

    private float GetCurrentVolume()
    {
        return _volumeType switch
        {
            VolumeType.Master => _settingsManager.CurrentSettings.masterVolume,
            VolumeType.Music => _settingsManager.CurrentSettings.musicVolume,
            VolumeType.SFX => _settingsManager.CurrentSettings.sfxVolume,
            _ => 1f
        };
    }

    private void OnEnable()
    {
        switch (_volumeType)
        {
            case VolumeType.Master:
                _settingsManager.OnMasterVolumeChanged += UpdateSliderValue;
                break;
            case VolumeType.Music:
                _settingsManager.OnMusicVolumeChanged += UpdateSliderValue;
                break;
            case VolumeType.SFX:
                _settingsManager.OnSFXVolumeChanged += UpdateSliderValue;
                break;
        }
    }

    private void OnDisable()
    {
        switch (_volumeType)
        {
            case VolumeType.Master:
                _settingsManager.OnMasterVolumeChanged -= UpdateSliderValue;
                break;
            case VolumeType.Music:
                _settingsManager.OnMusicVolumeChanged -= UpdateSliderValue;
                break;
            case VolumeType.SFX:
                _settingsManager.OnSFXVolumeChanged -= UpdateSliderValue;
                break;
        }

        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        switch (_volumeType)
        {
            case VolumeType.Master:
                _settingsManager.OnMasterVolumeChanged -= UpdateSliderValue;
                break;
            case VolumeType.Music:
                _settingsManager.OnMusicVolumeChanged -= UpdateSliderValue;
                break;
            case VolumeType.SFX:
                _settingsManager.OnSFXVolumeChanged -= UpdateSliderValue;
                break;
        }

        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}