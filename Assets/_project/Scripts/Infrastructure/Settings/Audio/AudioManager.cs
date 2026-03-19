using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Infrastructure.Controllers
{
    public class AudioManager : IInitializable
    {
        private AudioMixer _audioMixer;
        private SO.AudioStorageSO _audioStorage;
        private SettingsManager _settingsManager;

        private const string MasterVolumeParam = "MasterVolume";
        private const string MusicVolumeParam = "MusicVolume";
        private const string SFXVolumeParam = "SFXVolume";

        [Inject(Id = "MusicSource")]
        private AudioSource _musicSource;
        [Inject(Id = "SFXSource")]
        private AudioSource _sfxSource;

        [Inject]
        private void Construct(AudioMixer audioMixer, SO.AudioStorageSO audioStorage, SettingsManager settingsManager)
        {
            _audioMixer = audioMixer;
            _audioStorage = audioStorage;
            _settingsManager = settingsManager;
        }

        public void Initialize()
        {
            _musicSource.loop = true;
            _sfxSource.loop = false;

            _settingsManager.OnMasterVolumeChanged += SetMasterVolume;
            _settingsManager.OnMusicVolumeChanged += SetMusicVolume;
            _settingsManager.OnSFXVolumeChanged += SetSFXVolume;

            SetMasterVolume(_settingsManager.CurrentSettings.masterVolume);
            SetMusicVolume(_settingsManager.CurrentSettings.musicVolume);
            SetSFXVolume(_settingsManager.CurrentSettings.sfxVolume);
        }

        public void SetMasterVolume(float value)
        {
            float dB = value <= 0.0001f ? -80f : Mathf.Max(Mathf.Log10(value) * 20, -80f);
            _audioMixer.SetFloat(MasterVolumeParam, dB);
        }

        public void SetMusicVolume(float value)
        {
            float dB = value <= 0.0001f ? -80f : Mathf.Max(Mathf.Log10(value) * 20, -80f);
            _audioMixer.SetFloat(MusicVolumeParam, dB);
        }

        public void SetSFXVolume(float value)
        {
            float dB = value <= 0.0001f ? -80f : Mathf.Max(Mathf.Log10(value) * 20, -80f);
            _audioMixer.SetFloat(SFXVolumeParam, dB);
        }

        public void PlayMusic(int index)
        {
            AudioClip clip = _audioStorage.GetMusicClip(index);
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void PlayUISFX(int index)
        {
            AudioClip clip = _audioStorage.GetUISound(index);
            _sfxSource.PlayOneShot(clip);
        }

        public void PlayGameSFX(int index)
        {
            AudioClip clip = _audioStorage.GetGameSound(index);
            _sfxSource.PlayOneShot(clip);
        }
    }
}