using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Panels
{
    public class ImageDisplayManager : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _imagePanel;
        [SerializeField] private KeyCode _closeKey = KeyCode.E;

        private Infrastructure.GameEvents _gameEvents;
        private bool _isDisplaying = false;

        [Inject]
        private void Construct(Infrastructure.GameEvents gameEvents)
        {
            _gameEvents = gameEvents;
        }

        private void Update()
        {
            if (_isDisplaying && Input.GetKeyDown(_closeKey))
                HideNote();
        }

        public void ShowNote(Sprite sprite)
        {
            if (sprite == null) return;

            _image.sprite = sprite;
            _imagePanel.SetActive(true);
            _gameEvents.UIOpened();

            _isDisplaying = true;
        }

        public void HideNote()
        {
            _imagePanel.SetActive(false);
            _gameEvents.UIClosed();

            _isDisplaying = false;
        }
    }
}