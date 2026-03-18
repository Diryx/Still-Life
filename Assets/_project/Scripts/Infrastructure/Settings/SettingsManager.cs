using System;
using System.IO;
using Zenject;
using UnityEngine;

public class SettingsManager : IInitializable, IDisposable
{
    private SettingsData _settings;
    private string _filePath;
    private bool _isInitialized = false;

    public event Action<float> OnMasterVolumeChanged;
    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;

    public SettingsData CurrentSettings => _settings;

    [Inject]
    private void Construct() => _filePath = Path.Combine(Application.streamingAssetsPath, "Settings.json");

    public void Initialize()
    {
        if (_isInitialized) return;

        LoadSettings();
        _isInitialized = true;

        OnMasterVolumeChanged?.Invoke(_settings.masterVolume);
        OnMusicVolumeChanged?.Invoke(_settings.musicVolume);
        OnSFXVolumeChanged?.Invoke(_settings.sfxVolume);
    }

    public void LoadSettings()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _settings = JsonUtility.FromJson<SettingsData>(json);
            }
            else
            {
                _settings = new SettingsData();
                SaveSettings();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load settings: {e.Message}");
            _settings = new SettingsData();
        }
    }

    public void SaveSettings()
    {
        try
        {
            string json = JsonUtility.ToJson(_settings, true);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save settings: {e.Message}");
        }
    }

    public void SetMasterVolume(float value)
    {
        if (_settings.masterVolume != value)
        {
            _settings.masterVolume = value;
            OnMasterVolumeChanged?.Invoke(value);
            SaveSettings();
        }
    }

    public void SetMusicVolume(float value)
    {
        if (_settings.musicVolume != value)
        {
            _settings.musicVolume = value;
            OnMusicVolumeChanged?.Invoke(value);
            SaveSettings();
        }
    }

    public void SetSFXVolume(float value)
    {
        if (_settings.sfxVolume != value)
        {
            _settings.sfxVolume = value;
            OnSFXVolumeChanged?.Invoke(value);
            SaveSettings();
        }
    }

    public void Dispose() => SaveSettings();
}