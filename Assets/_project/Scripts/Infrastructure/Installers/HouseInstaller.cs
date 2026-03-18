using UnityEngine;
using Zenject;

public class HouseInstaller : MonoInstaller
{
    [SerializeField] private Movement _movement;
    [SerializeField] private CameraController _cam;
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private ImageDisplayManager _imageDisplayManager;
    [SerializeField] private PauseMenu _pauseMenu;

    public override void InstallBindings()
    {
        Container.Bind<Movement>().FromInstance(_movement).AsSingle();
        Container.Bind<CameraController>().FromInstance(_cam).AsSingle();
        Container.Bind<Camera>().FromInstance(_cameraMain).AsSingle();
        Container.Bind<PlayerInteraction>().FromInstance(_playerInteraction).AsSingle();
        Container.Bind<ImageDisplayManager>().FromInstance(_imageDisplayManager).AsSingle();
        Container.Bind<PauseMenu>().FromInstance(_pauseMenu).AsSingle();
    }
}
