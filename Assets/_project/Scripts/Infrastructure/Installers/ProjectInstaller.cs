using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Infrastructure.GameData
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private SO.AudioStorageSO _audioStorage;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Controllers.SettingsManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<Controllers.AudioManager>().AsSingle().NonLazy();
            Container.Bind<GameEvents>().AsSingle();
            Container.Bind<Controllers.SceneController>().AsSingle().NonLazy();
            Container.BindInstance(_audioMixer).AsSingle();
            Container.BindInstance(_audioStorage).AsSingle();
            Container.BindInstance(_musicSource).WithId("MusicSource");
            Container.BindInstance(_sfxSource).WithId("SFXSource");
        }
    }
}