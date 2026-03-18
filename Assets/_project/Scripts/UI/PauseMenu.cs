using DG.Tweening;
using UnityEngine;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform _topLid;
    [SerializeField] private RectTransform _bottomLid;
    [SerializeField] private CanvasGroup _menuPanel;

    [Header("Animation Settings")]
    [SerializeField] private float _blinkDuration = 0.3f;
    [SerializeField] private float _lidHeight = 540f;
    [SerializeField] private Ease _closeEase = Ease.InOutQuad;
    [SerializeField] private Ease _openEase = Ease.OutBack;

    private GameEvents _gameEvents;
    private bool _isPaused = false;
    private Sequence _currentAnimation;

    [Inject]
    private void Construct(GameEvents gameEvents) => _gameEvents = gameEvents;

    private void Start()
    {
        _topLid.anchoredPosition = new Vector2(0, _lidHeight);
        _bottomLid.anchoredPosition = new Vector2(0, -_lidHeight);

        _menuPanel.alpha = 0f;
        _menuPanel.interactable = false;
        _menuPanel.blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                Unpause();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (_isPaused) return;

        _isPaused = true;

        _currentAnimation?.Kill();

        _currentAnimation = DOTween.Sequence()
            .Append(_topLid.DOAnchorPosY(0, _blinkDuration).SetEase(_closeEase).SetUpdate(true))
            .Join(_bottomLid.DOAnchorPosY(0, _blinkDuration).SetEase(_closeEase).SetUpdate(true))
            .AppendCallback(() => {
                Time.timeScale = 0f;
                _gameEvents.Paused();
                _menuPanel.interactable = true;
                _menuPanel.blocksRaycasts = true;
            })
            .Append(_menuPanel.DOFade(1f, _blinkDuration).SetEase(_openEase).SetUpdate(true))
            .SetUpdate(true);
    }

    public void Unpause()
    {
        if (!_isPaused) return;

        _currentAnimation?.Kill();

        _currentAnimation = DOTween.Sequence()
            .Append(_menuPanel.DOFade(0f, _blinkDuration * 0.5f).SetEase(_openEase).SetUpdate(true))
            .AppendCallback(() => {
                _menuPanel.interactable = false;
                _menuPanel.blocksRaycasts = false;
                _gameEvents.Resume();
                Time.timeScale = 1f;
            })
            .Append(_topLid.DOAnchorPosY(_lidHeight, _blinkDuration).SetEase(_openEase).SetUpdate(true))
            .Join(_bottomLid.DOAnchorPosY(-_lidHeight, _blinkDuration).SetEase(_openEase).SetUpdate(true))
            .OnComplete(() => {
                _isPaused = false;
            })
            .SetUpdate(true);
    }
}