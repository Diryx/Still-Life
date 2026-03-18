using UnityEngine;
using Zenject;

public class ActiveButton : ButtonParent
{
    [SerializeField] private Active _active;
    [SerializeField] private SceneReference _sceneReference;

    private SceneController _sceneController;
    private PauseMenu _pauseMenu;

    [Inject]
    private void Construct(SceneController sceneController, [InjectOptional] PauseMenu pauseMenu)
    {
        _sceneController = sceneController;
        _pauseMenu = pauseMenu;
    }

    public enum Active
    {
        None = 0,
        changeScene = 1,
        exit = 2,
        continueGame = 3
    }

    protected override void ButtonAction()
    {
        base.ButtonAction();

        switch (_active)
        {
            case Active.changeScene:
                ChangeScene();
                break;
            case Active.exit:
                ExitGame();
                break;
            case Active.continueGame:
                ContinueGame();
                break;
            case Active.None:
                break;
        }
    }

    private void ChangeScene() => _sceneController.LoadScene(_sceneReference.SceneIndex);
    private void ExitGame() => Application.Quit();
    private void ContinueGame() => _pauseMenu.Unpause();
}