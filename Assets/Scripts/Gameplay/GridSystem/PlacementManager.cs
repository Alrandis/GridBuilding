using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject _testBuildingPrefab;
    [SerializeField] private InputManager _inputManager; // если используешь централизованный InputManager
    [SerializeField] private float _moveRepeatDelay = 0.18f;

    private GameObject _currentBuilding;
    private Vector2Int _currentGridPos;
    private Vector2 _moveInput;
    private bool _isActive;
    private float _moveTimer;

    private void OnEnable()
    {
        _inputManager.OnPlace += PlaceBuilding;
    }

    private void OnDisable()
    {
        _inputManager.OnPlace -= PlaceBuilding;
    }

    public void EnableMode()
    {
        if (_isActive) return;
        _isActive = true;
        
        SpawnTestIfNeeded();
        Debug.Log("Placement enabled");
    }

    public void DisableMode()
    {
        if (!_isActive) return;
        _isActive = false;

        // Отменяем текущее превью (не фиксируем)
        if (_currentBuilding != null)
        {
            Destroy(_currentBuilding);
            _currentBuilding = null;
        }
        Debug.Log("Placement disabled");
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
            Vector3 mouseWorld = _inputManager.MouseWorldPosition;
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
