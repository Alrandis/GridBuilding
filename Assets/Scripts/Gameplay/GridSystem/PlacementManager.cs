using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject _testBuildingPrefab;
    [SerializeField] private InputManager _inputManager; // если используешь централизованный InputManager

    private GameObject _currentBuilding;
    private Vector2Int _currentGridPos;
    private Vector2 _moveInput;
    private bool _isActive;

    // таймер/повтор сдерживания (если нужно)
    [SerializeField] private float _moveRepeatDelay = 0.18f;
    private float _moveTimer;

    public void EnableMode()
    {
        if (_isActive) return;
        _isActive = true;
        SubscribeInput();
        SpawnTestIfNeeded();
        Debug.Log("Placement enabled");
    }

    public void DisableMode()
    {
        if (!_isActive) return;
        _isActive = false;
        UnsubscribeInput();
        // Отменяем текущее превью (не фиксируем)
        if (_currentBuilding != null)
        {
            Destroy(_currentBuilding);
            _currentBuilding = null;
        }
        Debug.Log("Placement disabled");
    }

    private void SubscribeInput()
    {
        if (_inputManager == null) return;
        var input = _inputManager.InputActions.GridPlacement;
        input.Move.performed += OnMovePerformed;
        input.Move.canceled += ctx => _moveInput = Vector2.zero;
        input.Place.performed += ctx => PlaceBuilding();
    }

    private void UnsubscribeInput()
    {
        if (_inputManager == null) return;
        var input = _inputManager.InputActions.GridPlacement;
        input.Move.performed -= OnMovePerformed;
        input.Place.performed -= ctx => PlaceBuilding(); // safe unsubscribe
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (!_isActive) return;
        if (_currentBuilding == null) return;

        HandleMovement();
        UpdateBuildingPosition();
    }

    private void HandleMovement()
    {
        bool movedByKeyboard = false;

        if (_moveInput.sqrMagnitude > 0f)
        {
            _moveTimer -= Time.deltaTime;
            if (_moveTimer <= 0f)
            {
                Vector2Int delta = new Vector2Int(Mathf.RoundToInt(_moveInput.x), Mathf.RoundToInt(_moveInput.y));
                if (delta != Vector2Int.zero)
                {
                    _currentGridPos += delta;
                    _moveTimer = _moveRepeatDelay;
                    movedByKeyboard = true;
                }
            }
        }
        else
        {
            _moveTimer = 0f;
        }

        if (!movedByKeyboard)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorld.z = 0;
            _currentGridPos = _gridManager.GetGridPosition(mouseWorld);
        }
        else
        {
            // синхронизируем курсор с позицией
            Vector3 worldPos = _gridManager.GetWorldPosition(_currentGridPos.x, _currentGridPos.y);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            Mouse.current.WarpCursorPosition(screenPos);
            InputState.Change(Mouse.current.position, screenPos);
        }
    }

    private void UpdateBuildingPosition()
    {
        _currentBuilding.transform.position = _gridManager.GetWorldPosition(_currentGridPos.x, _currentGridPos.y);
    }

    private void PlaceBuilding()
    {
        if (_currentBuilding == null) return;
        if (_gridManager.IsOccupied(_currentGridPos))
        {
            Debug.Log("Cell occupied, can't place.");
            return;
        }

        _gridManager.PlaceBuilding(_currentGridPos, _currentBuilding);
        _currentBuilding = null;
    }

    // Тест-спавн (вызов через UIController или при включении режима)
    public void SpawnTestBuildingAtOrigin()
    {
        if (_currentBuilding != null) Destroy(_currentBuilding);
        _currentGridPos = Vector2Int.zero;
        _currentBuilding = Instantiate(_testBuildingPrefab, _gridManager.GetWorldPosition(0, 0), Quaternion.identity);
    }

    private void SpawnTestIfNeeded()
    {
        if (_currentBuilding == null && _testBuildingPrefab != null)
            SpawnTestBuildingAtOrigin();
    }
}
