using UnityEngine;
using Zenject;
using static Zenject.CheatSheet;

namespace Infrastructure.GameData
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private Player.Movement _movement;
        [SerializeField] private Player.CameraController _cam;
        [SerializeField] private Camera _cameraMain;
        [SerializeField] private Player.PlayerInteraction _playerInteraction;
        [SerializeField] private UI.Panels.ImageDisplayManager _imageDisplayManager;
        [SerializeField] private UI.Panels.PauseMenu _pauseMenu;

        public override void InstallBindings()
        {
            Container.Bind<Player.Movement>().FromInstance(_movement).AsSingle();
            Container.Bind<Player.CameraController>().FromInstance(_cam).AsSingle();
            Container.Bind<Camera>().FromInstance(_cameraMain).AsSingle();
            Container.Bind<Player.PlayerInteraction>().FromInstance(_playerInteraction).AsSingle();
            Container.Bind<UI.Panels.ImageDisplayManager>().FromInstance(_imageDisplayManager).AsSingle();
            Container.Bind<UI.Panels.PauseMenu>().FromInstance(_pauseMenu).AsSingle();
        }
    }
}
