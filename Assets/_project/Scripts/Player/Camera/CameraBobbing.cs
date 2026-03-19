using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Player
{
    public class CameraBobbing : MonoBehaviour
    {
        [Header("Walk Settings")]
        [SerializeField] private float _walkVerticalForce;
        [SerializeField] private float _walkHorizontalForce;
        [SerializeField] private float _walkDuration;
        [SerializeField] private Ease _walkEase = Ease.InSine;

        [Header("Run Settings")]
        [SerializeField] private float _runSpeedMultiplier;
        [SerializeField] private float _runTiltAngle;
        [SerializeField] private float _runHorizontalForce;
        [SerializeField] private float _runVerticalForce;
        [SerializeField] private Ease _runEase = Ease.InSine;

        [Header("Transition Settings")]
        [SerializeField] private float _transitionDuration = 0.3f;
        [SerializeField] private Ease _transitionEase = Ease.OutQuad;

        private Movement _movement;
        private CameraController _cam;
        private Infrastructure.GameEvents _gameEvents;
        private Vector3 _baseCamPosition;
        private Vector3 _baseCamRotation;
        private Sequence _bobbingSequence;
        private bool _isBobbing;
        private bool _wasRunning;
        private Tween _transitionTween;

        [Inject]
        private void Construct(Movement movement, CameraController cam, Infrastructure.GameEvents gameEvents)
        {
            _movement = movement;
            _cam = cam;
            _gameEvents = gameEvents;
        }

        private void Start()
        {
            _baseCamPosition = transform.localPosition;
            _baseCamRotation = transform.localEulerAngles;
        }

        private void OnEnable()
        {
            if (_movement != null && _movement.IsMoving)
                StartBobbing();

            _gameEvents.OnUIOpened += StopBobbing;
        }

        private void OnDisable()
        {
            StopBobbing();
            transform.localPosition = _baseCamPosition;
            transform.localRotation = Quaternion.Euler(_baseCamRotation);

            _gameEvents.OnUIOpened -= StopBobbing;
        }

        private void Update()
        {
            if (_movement == null) return;

            CheckBobbing();
            _wasRunning = _movement.IsRunning;
        }

        private void CheckBobbing()
        {
            if (_movement.IsMoving && !_isBobbing && (_transitionTween == null || !_transitionTween.IsActive()))
                StartBobbing();
            else if (!_movement.IsMoving && _isBobbing)
                StopBobbing();

            if (_isBobbing && _movement.IsMoving)
            {
                if (_movement.IsRunning != _wasRunning)
                    RestartBobbing();
            }
        }

        private void StartBobbing()
        {
            if (_isBobbing) return;

            _transitionTween?.Kill(false);

            _isBobbing = true;
            CreateBobbingSequence();
        }

        private void RestartBobbing()
        {
            if (_bobbingSequence != null && _bobbingSequence.IsActive())
            {
                _bobbingSequence.Kill(false);
                _bobbingSequence = null;
            }

            CreateBobbingSequence();
        }

        private void CreateBobbingSequence()
        {
            if (_bobbingSequence != null && _bobbingSequence.IsActive())
                _bobbingSequence.Kill();

            _bobbingSequence = DOTween.Sequence();

            if (_movement.IsRunning)
            {
                float runDuration = _walkDuration / _runSpeedMultiplier;

                _bobbingSequence.Append(
                    DOTween.Sequence()
                        .Join(transform.DOLocalMoveY(_baseCamPosition.y - _runVerticalForce, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalMoveX(_baseCamPosition.x + _runHorizontalForce, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalRotate(new Vector3(_runTiltAngle, 0, 0), runDuration / 2).SetEase(_runEase).SetRelative(false)));

                _bobbingSequence.Append(
                    DOTween.Sequence()
                        .Join(transform.DOLocalMoveY(_baseCamPosition.y, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalMoveX(_baseCamPosition.x + _runHorizontalForce * 0.5f, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalRotate(Vector3.zero, runDuration / 2).SetEase(_runEase).SetRelative(false)));

                _bobbingSequence.Append(
                    DOTween.Sequence()
                        .Join(transform.DOLocalMoveY(_baseCamPosition.y - _runVerticalForce, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalMoveX(_baseCamPosition.x - _runHorizontalForce, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalRotate(new Vector3(_runTiltAngle, 0, 0), runDuration / 2).SetEase(_runEase).SetRelative(false)));

                _bobbingSequence.Append(
                    DOTween.Sequence()
                        .Join(transform.DOLocalMoveY(_baseCamPosition.y, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalMoveX(_baseCamPosition.x - _runHorizontalForce * 0.5f, runDuration / 2).SetEase(_runEase))
                        .Join(transform.DOLocalRotate(Vector3.zero, runDuration / 2).SetEase(_runEase).SetRelative(false)));
            }
            else
            {
                float halfWalk = _walkDuration / 2;

                _bobbingSequence.Append(transform.DOLocalMoveY(_baseCamPosition.y - _walkVerticalForce, halfWalk).SetEase(_walkEase))
                    .Join(transform.DOLocalMoveX(_baseCamPosition.x + _walkHorizontalForce, halfWalk).SetEase(_walkEase));
                _bobbingSequence.Append(transform.DOLocalMoveY(_baseCamPosition.y, halfWalk).SetEase(_walkEase))
                    .Join(transform.DOLocalMoveX(_baseCamPosition.x - _walkHorizontalForce, halfWalk).SetEase(_walkEase)); ;
            }

            _bobbingSequence.SetLoops(-1, LoopType.Restart);
            _bobbingSequence.Play();
        }

        public void StopBobbing()
        {
            if (!_isBobbing) return;

            _isBobbing = false;

            if (_bobbingSequence != null && _bobbingSequence.IsActive())
            {
                _bobbingSequence.Kill(false);
                _bobbingSequence = null;
            }

            _transitionTween?.Kill(false);

            _transitionTween = DOTween.Sequence()
                .Join(transform.DOLocalMove(_baseCamPosition, _transitionDuration).SetEase(_transitionEase))
                .Join(transform.DOLocalRotate(_baseCamRotation, _transitionDuration).SetEase(_transitionEase))
                .OnComplete(() => { _transitionTween = null; }).Play();
        }

        private void OnDestroy()
        {
            _transitionTween?.Kill(false);
            if (_bobbingSequence != null && _bobbingSequence.IsActive())
                _bobbingSequence.Kill();
        }
    }
}