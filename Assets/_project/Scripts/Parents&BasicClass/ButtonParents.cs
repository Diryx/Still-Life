using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ButtonParent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int _sfxIndexClick = 0;
    [SerializeField] private int _sfxIndexPointerEnter = 1;

    protected Button _button;
    protected RectTransform _rectTransform;
    protected AudioManager _audioManager;
    protected bool _isPointerOver = false;
    private Vector3 _originalScale;

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
        _originalScale = _rectTransform.localScale;
    }

    protected virtual void Start()
    {
        if (_button != null)
            _button.onClick.AddListener(ButtonAction);
    }

    protected virtual void ButtonAction() => _audioManager.PlayUISFX(_sfxIndexClick);

    private void OnEnable()
    {
        if (_button != null)
            _button.onClick.AddListener(ButtonAction);
    }

    private void OnDisable()
    {
        if (_button != null)
            _button.onClick.RemoveListener(ButtonAction);

        if (_rectTransform != null)
            _rectTransform.DOKill();

        _isPointerOver = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_rectTransform == null) return;

        _audioManager.PlayUISFX(_sfxIndexPointerEnter);
        _isPointerOver = true;
        _rectTransform.DOScale(_originalScale * 1.05f, 0.2f);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (_rectTransform == null) return;

        _isPointerOver = false;
        _rectTransform.DOScale(_originalScale, 0.2f);
    }
}