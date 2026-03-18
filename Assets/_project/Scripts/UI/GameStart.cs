using UnityEngine;
using Zenject;
using DG.Tweening;

public class GameStart : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private Ease _fadeEase;

    private CanvasGroup _canvasGroupPanel;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    private void Awake()
    {
        _canvasGroupPanel = GetComponent<CanvasGroup>();

        _canvasGroupPanel.alpha = 1f;
        _canvasGroupPanel.interactable = true;
        _canvasGroupPanel.blocksRaycasts = true;
    }

    private void Start()
    {
        _audioManager.PlayMusic(0);
        PlayIntroAnimation();
    }

    private void PlayIntroAnimation() => _canvasGroupPanel.DOFade(0f, _fadeDuration).SetEase(_fadeEase).OnComplete(() => Destroy(gameObject));
}