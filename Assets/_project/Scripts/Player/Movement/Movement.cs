using UnityEngine;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _stepSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityMultiplier;

        [Header("Having Permissions")]
        [SerializeField] private bool _canJumping = false;
        [SerializeField] private bool _canRunning = false;

        private Rigidbody _rb;
        private CameraController _cam;
        private Vector3 _moveDirection;
        private Infrastructure.GameEvents _gameEvents;
        private bool _isGrounded;
        private bool _isJumping;
        private bool _isMoving;
        private bool _isStepping;
        private bool _isRunning;
        private bool _isMovementLocked = false;
        private float horizontal;
        private float vertical;

        public bool IsMoving => _isMoving;
        public bool IsStepping => _isStepping;
        public bool IsRunning => _isRunning;
        public bool IsGrounded => _isGrounded;
        public bool IsJumping => _isJumping;
        public void LockMovement() => _isMovementLocked = true;
        public void UnlockMovement() => _isMovementLocked = false;

        [Inject]
        private void Construct(CameraController cam, Infrastructure.GameEvents gameEvents)
        {
            _cam = cam;
            _gameEvents = gameEvents;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            if (_isMovementLocked == true) return;

            GetInput();
            CheckStepOrRun();
            Jump();
        }

        private void FixedUpdate()
        {
            if (_isMovementLocked == true) return;

            Move();
            Gravity();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col != null && col.gameObject.GetComponent<Floor>())
            {
                _isGrounded = true;
                _isJumping = false;
            }
        }

        private void OnCollisionExit(Collision col)
        {
            if (col != null && col.gameObject.GetComponent<Floor>())
                _isGrounded = false;
        }

        private void OnEnable()
        {
            _gameEvents.OnUIOpened += LockMovement;
            _gameEvents.OnUIClosed += UnlockMovement;
        }

        private void OnDisable()
        {
            _gameEvents.OnUIOpened -= LockMovement;
            _gameEvents.OnUIClosed -= UnlockMovement;
        }


        private void GetInput()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        private void Move()
        {
            Vector3 cameraForward = _cam.transform.forward;
            Vector3 cameraRight = _cam.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            _moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

            float currentSpeed = (_canRunning && _isRunning) ? _runSpeed : _stepSpeed;

            Vector3 velocity = _moveDirection * currentSpeed;
            velocity.y = _rb.linearVelocity.y;

            _rb.linearVelocity = velocity;
        }

        private void CheckStepOrRun()
        {
            _isMoving = _moveDirection.magnitude > 0.1f;

            if (_isMoving && Input.GetKey(KeyCode.LeftShift) && _canRunning)
            {
                _isRunning = true;
                _isStepping = false;
            }
            else if (_isMoving && !Input.GetKey(KeyCode.LeftShift))
            {
                _isRunning = false;
                _isStepping = true;
            }
            else
            {
                _isRunning = false;
                _isStepping = false;
            }
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && _canJumping)
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _isJumping = true;
            }
        }

        private void Gravity()
        {
            if (_isGrounded != true)
            {
                Vector3 gravityForce = Physics.gravity * (_gravityMultiplier - 1f);
                _rb.AddForce(gravityForce, ForceMode.Acceleration);
            }
        }

        public void ForceStopMovement()
        {
            _rb.linearVelocity = Vector3.zero;
            _isMoving = false;
            _isRunning = false;
            _isStepping = false;
        }
    }
}