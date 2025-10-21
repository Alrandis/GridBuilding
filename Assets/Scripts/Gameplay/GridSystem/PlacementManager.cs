using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private InputManager _inputManager; // если используешь централизованный InputManager
    [SerializeField] private float _moveRepeatDelay = 0.18f;

    private GameObject _currentBuilding;
    private GameObject _buildingPrefab;

    private Vector2Int _currentGridPos;
    private Vector2 _moveInput;
    private bool _isActive;
    private float _moveTimer;

    private int _currentRotation = 0; // угол в градусах, кратный 90

    private void OnEnable()
    {
        _inputManager.OnPlace += PlaceBuilding;
        _inputManager.OnRotate += HandleRotation;
    }

    private void OnDisable()
    {
        _inputManager.OnPlace -= PlaceBuilding;
        _inputManager.OnRotate -= HandleRotation;
    }

    public void EnableMode()
    {
        if (_isActive) return;
        _isActive = true;
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
        _moveInput = _inputManager.MoveInput;
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

    private void HandleRotation()
    {
        _currentRotation += 90;
        if (_currentRotation >= 360) _currentRotation = 0;
        UpdateBuildingPosition();
    }

    private void UpdateBuildingPosition()
    {
        if (_currentBuilding == null) return;

        var placeable = _currentBuilding.GetComponent<PlaceableObject>();
        if (placeable == null) return;

        // Берём первый повёрнутый cell (обычно pivot)
        var cells = placeable.GetWorldCells(_currentGridPos, _currentRotation);

        // Считаем позицию в мире через pivot
        Vector3 worldPos = Vector3.zero;
        foreach (var cell in cells)
        {
            worldPos += _gridManager.GetWorldPosition(cell.x, cell.y);
        }
        worldPos /= cells.Count(); // центрируем превью по всем клеткам
        worldPos.z = 0;

        _currentBuilding.transform.position = worldPos;

        // Применяем поворот
        _currentBuilding.transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
    }


    private void PlaceBuilding()
    {
        if (_currentBuilding == null) return;

        var placeable = _currentBuilding.GetComponent<PlaceableObject>();
        if (placeable == null)
        {
            Debug.LogWarning("Prefab missing PlaceableObject!");
            return;
        }

        var cells = placeable.GetWorldCells(_currentGridPos, _currentRotation);
        if (!_gridManager.IsAreaFree(cells))
        {
            Debug.Log("Can't place — cells occupied!");
            return;
        }

        //_gridManager.OccupyCells(placeable.GetWorldCells(_currentGridPos), _currentBuilding);
        _gridManager.OccupyCells(cells, _currentBuilding);
        _currentBuilding = null;
        _currentRotation = 0; // сбрасываем угол для следующего здания
    }

    public void SetBuilding(GameObject go)
    {
        _buildingPrefab = go;
        SpawnBuilding();
    }

    public void SpawnBuilding()
    {
        if (_currentBuilding != null) Destroy(_currentBuilding);
        _currentGridPos = Vector2Int.zero;
        _currentBuilding = Instantiate(_buildingPrefab, _gridManager.GetWorldPosition(0, 0), Quaternion.identity);
    }
}
