using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button), typeof(Shadow))]
public class ButtonParent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int _sfxIndexClick = 0;
    [SerializeField] private int _sfxIndexPointerEnter = 1; 
    [SerializeField] private float _shadowHoverStrength = 10f;
    [SerializeField] private float _shadowNormalStrength = 5f;
    [SerializeField] private float _shadowDuration = 0.2f;
    [SerializeField] private Vector2 _shadowDirection = new Vector2(1, 1);
    [SerializeField] private string _animBoolParam = "";
    [SerializeField] private bool _useAnimation = true;

    protected Button _button;
    protected Shadow _shadow;
    protected RectTransform _rectTransform;
    protected AudioManager _audioManager;
    private Animator _animator;
    protected bool _isPointerOver = false;
    private Vector3 _originalScale;

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _shadow = GetComponent<Shadow>();
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _originalScale = _rectTransform.localScale;
    }

    protected virtual void Start()
    {
        _button.onClick.AddListener(ButtonAction);
        _shadow.effectDistance = _shadowDirection * _shadowNormalStrength;

        if (_animator != null && _useAnimation)
            _animator.SetBool(_animBoolParam, false);
    }

    protected virtual void ButtonAction() => _audioManager.PlayUISFX(_sfxIndexClick);

    private void OnEnable() => _button.onClick.AddListener(ButtonAction);

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ButtonAction);
        _rectTransform.DOKill();
        _isPointerOver = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        DOTween.To(() => _shadow.effectDistance,
            x => _shadow.effectDistance = x,
            _shadowDirection * _shadowHoverStrength,
            _shadowDuration);

        _audioManager?.PlayUISFX(_sfxIndexPointerEnter);
        _rectTransform.DOScale(_originalScale * 1.05f, _shadowDuration);

        if (_animator != null && _useAnimation)
            _animator.SetBool(_animBoolParam, true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        DOTween.To(() => _shadow.effectDistance,
            x => _shadow.effectDistance = x,
            _shadowDirection * _shadowNormalStrength,
            _shadowDuration);

        _rectTransform.DOScale(_originalScale, _shadowDuration);

        if (_animator != null && _useAnimation)
            _animator.SetBool(_animBoolParam, false);
    }
}