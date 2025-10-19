using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private InputActionSystem _playerInput;
    private Vector2 _mouseWorldPosition;
    public Vector2 MouseWorldPosition => _mouseWorldPosition;

    public Vector2 MoveInput { get; private set; }

    public event System.Action OnPlace;
    public event System.Action OnDelete;

    private void Awake()
    {
        _playerInput = new InputActionSystem();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _playerInput.GridPlacement.Place.performed += OnPlacePerformed;
        _playerInput.GridPlacement.Delete.performed += OnDeletePerformed;
        _playerInput.GridPlacement.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _playerInput.GridPlacement.Move.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        _playerInput.GridPlacement.Place.performed -= OnPlacePerformed;
        _playerInput.GridPlacement.Delete.performed -= OnDeletePerformed;
        _playerInput.Disable();
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        _mouseWorldPosition = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.nearClipPlane));
    }

    private void OnPlacePerformed(InputAction.CallbackContext context) => OnPlace?.Invoke();
    private void OnDeletePerformed(InputAction.CallbackContext context) => OnDelete?.Invoke();
}
