using UnityEngine;
using Zenject;

public class ImageItem : BaseInteractable
{
    [Header("Note Settings")]
    [SerializeField] private ImageStorageSO _imageStorage;
    [SerializeField] private int _imageIndex;
    [SerializeField] private bool _destroyOnRead = true;

    private ImageDisplayManager _displayManager;

    [Inject]
    private void Construct(ImageDisplayManager displayManager) => _displayManager = displayManager;

    public override string InteractionPrompt => _prompt;

    public override void Interact()
    {
        if (!CanInteract) return;

        Sprite imageSprite = _imageStorage.GetNoteSprite(_imageIndex);
        if (imageSprite != null)
        {
            _displayManager.ShowNote(imageSprite);
        }

        if (_destroyOnRead)
            Destroy(gameObject);
    }
}