using UnityEngine.SceneManagement;

namespace Infrastructure.Controllers
{
    public class SceneController
    {
        private int _currentSceneIndex;

        public SceneController() => _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        public void LoadScene(int sceneIndex)
        {
            if (sceneIndex != _currentSceneIndex)
            {
                SceneManager.LoadScene(sceneIndex);
                _currentSceneIndex = sceneIndex;
            }
        }
    }
}