using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject _testBuildingPrefab; // для теста

    private GameObject _currentBuilding;
    private Vector2Int _currentGridPos;
    private InputActiong _playerInput;
    private Vector2 _moveInput;

    [SerializeField] private float _moveRepeatDelay = 0.2f; // задержка между шагами при удержании клавиши
    private float _moveTimer;

    private void Awake()
    {
        _playerInput = new InputActiong(); // нужно сгенерировать через Input Actions
        _playerInput.Enable();

        _playerInput.GridPlacement.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        _playerInput.GridPlacement.Move.canceled += context => _moveInput = Vector2.zero;
        _playerInput.GridPlacement.Place.performed += context => PlaceBuilding();
    }

    private void Update()
    {
        if (_currentBuilding == null) return;

        HandleMovement();
        UpdateBuildingPosition();
    }

    private void HandleMovement()
    {
        bool movedByKeyboard = false;

        // Движение по клавиатуре
        if (_moveInput.sqrMagnitude > 0)
        {
            _moveTimer -= Time.deltaTime;

            if (_moveTimer <= 0f)
            {
                Vector2Int delta = new Vector2Int(
                    Mathf.RoundToInt(_moveInput.x),
                    Mathf.RoundToInt(_moveInput.y)
                );

                if (delta != Vector2Int.zero)
                {
                    _currentGridPos += delta;
                    _currentGridPos = new Vector2Int(
                        Mathf.Clamp(_currentGridPos.x, -1000, 1000),
                        Mathf.Clamp(_currentGridPos.y, -1000, 1000)
                    );

                    _moveTimer = _moveRepeatDelay; // сброс таймера
                    movedByKeyboard = true;
                }
            }
        }
        else
        {
            _moveTimer = 0f; // сброс при отпускании
        }

        // Если не двигались с клавиатуры — следуем за мышью
        if (!movedByKeyboard)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorld.z = 0;
            _currentGridPos = _gridManager.GetGridPosition(mouseWorld);
        }
        else
        {
            // Если двигались клавиатурой — подвинем курсор к новой позиции
            Vector3 worldPos = _gridManager.GetWorldPosition(_currentGridPos.x, _currentGridPos.y);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            Mouse.current.WarpCursorPosition(screenPos);
            InputState.Change(Mouse.current.position, screenPos); // обновляем внутреннее состояние Input System
        }
    }

    private void UpdateBuildingPosition()
    {
        _currentBuilding.transform.position = _gridManager.GetWorldPosition(_currentGridPos.x, _currentGridPos.y);
    }

    private void PlaceBuilding()
    {
        if (_currentBuilding == null) return;
        // Здесь можно добавить проверку занятости
        _currentBuilding = null;
    }

    #region TEST SPAWN (для теста нажатием E)
    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E)
        {
            SpawnTestBuilding();
        }
    }

    private void SpawnTestBuilding()
    {
        if (_currentBuilding != null) Destroy(_currentBuilding);

        _currentGridPos = Vector2Int.zero;
        _currentBuilding = Instantiate(_testBuildingPrefab, _gridManager.GetWorldPosition(0, 0), Quaternion.identity);
    }
    #endregion
}
