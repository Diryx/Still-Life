using UnityEngine;
using Zenject;

namespace Interaction.Object
{
    public class ImageItem : BaseInteractable
    {
        [SerializeField] private Infrastructure.SO.ImageStorageSO _imageStorage;
        [SerializeField] private int _imageIndex;
        [SerializeField] private bool _destroyOnRead = true;

        private UI.Panels.ImageDisplayManager _displayManager;

        [Inject]
        private void Construct(UI.Panels.ImageDisplayManager displayManager)
            => _displayManager = displayManager;

        public override string InteractionPrompt => _prompt;

        public override void Interact()
        {
            if (!CanInteract) return;

            Sprite imageSprite = _imageStorage.GetNoteSprite(_imageIndex);
            if (imageSprite != null)
                _displayManager.ShowNote(imageSprite);

            if (_destroyOnRead)
                Destroy(gameObject);
        }
    }
}