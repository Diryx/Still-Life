using TMPro;
using UnityEngine;
using Zenject;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 3f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private TMP_Text _promptText;

    private Camera _cam;
    private GameEvents _gameEvents;
    private BaseInteractable _currentInteractable;

    private bool _isUIOpen = false;

    [Inject]
    private void Construct(Camera mainCamera, GameEvents gameEvents)
    {
        _cam = mainCamera;
        _gameEvents = gameEvents;

        _gameEvents.OnUIOpened += () => _isUIOpen = true;
        _gameEvents.OnUIClosed += () => _isUIOpen = false;
    }

    private void Update()
    {
        if (_isUIOpen) return;

        CheckInteractable();

        if (_currentInteractable != null && Input.GetKeyDown(_interactKey))
        {
            _currentInteractable.Interact();
        }
    }

    private void CheckInteractable()
    {
        Ray ray = _cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _interactableMask))
        {
            BaseInteractable interactable = hit.collider.GetComponent<BaseInteractable>();
            if (interactable != null && interactable.CanInteract)
            {
                if (_currentInteractable != interactable)
                {
                    _currentInteractable = interactable;
                    ShowPrompt(interactable.InteractionPrompt);
                }
                return;
            }
        }

        _currentInteractable = null;
        HidePrompt();
    }

    private void ShowPrompt(string text)
    {
        if (_promptText != null)
        {
            _promptText.text = text;
            _promptText.gameObject.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (_promptText != null)
            _promptText.gameObject.SetActive(false);
    }
}