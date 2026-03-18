using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 2f;
    [SerializeField] private float _MaxYRotation = 90f;
    [SerializeField] private float _MinYRotation = 90f;
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private bool _IsSmoothing = true;

    private GameEvents _gameEvents;
    private bool _isCameraLocked = false;
    private float yRotation;
    private float xRotation;
    private float currentYRotation;
    private float currentXRotation;
    private float yRotationVelocity;
    private float xRotationVelocity;

    [Inject]
    private void Construct(GameEvents gameEvents)
    {
        _gameEvents = gameEvents;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentYRotation = transform.eulerAngles.y;
        currentXRotation = transform.eulerAngles.x;
    }

    private void Update()
    {
        if (_isCameraLocked == true) return;

        float mouseX = Input.GetAxis("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -_MinYRotation, _MaxYRotation);
    }

    private void LateUpdate()
    {
        if (_IsSmoothing)
            SmoothRotate();
        else
            NotSmmoothRotate();
    }

    private void SmoothRotate()
    {
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationVelocity, _smoothTime);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, _smoothTime);
        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
        _player.rotation = Quaternion.Euler(0, currentYRotation, 0);
    }

    private void NotSmmoothRotate()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        _player.rotation = Quaternion.Euler(0, yRotation, 0);
    }




    private void OnEnable()
    {
        _gameEvents.OnUIOpened += LockCamera;
        _gameEvents.OnUIClosed += UnlockCamera;
        _gameEvents.OnPause += CursorVisible;
        _gameEvents.OnResume += CursorUnvisible;
    }
    private void OnDisable()
    {
        _gameEvents.OnUIOpened -= LockCamera;
        _gameEvents.OnUIClosed -= UnlockCamera;
        _gameEvents.OnPause -= CursorVisible;
        _gameEvents.OnResume -= CursorUnvisible;
    }
    private void LockCamera() => _isCameraLocked = true;
    private void UnlockCamera() => _isCameraLocked = false;
    private void CursorUnvisible()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void CursorVisible()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}