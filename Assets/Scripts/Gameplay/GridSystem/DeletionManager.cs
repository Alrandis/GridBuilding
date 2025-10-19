using UnityEngine;
using UnityEngine.InputSystem;

public class DeletionManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private LayerMask _buildingLayer;

    private Camera _camera;

    private InputActiong _playerInput;

    private void Awake()
    {
        _camera = Camera.main;

        // Удаление по клику мышью (например, правая кнопка)
        _playerInput = new InputActiong();
        _playerInput.Enable();
        _playerInput.GridPlacement.Delete.performed += context =>
        {
            TryDeleteUnderCursor();
            Debug.Log("Нажатие сработало");
        };
       
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1)) TryDeleteUnderCursor();
    //}

    private void OnDestroy()
    {
        _playerInput.Disable();
    }

    private void TryDeleteUnderCursor()
    {
        Vector3 mouseWorld = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0;

        Vector2Int gridPos = _gridManager.GetGridPosition(mouseWorld);

        if (_gridManager.IsOccupied(gridPos))
        {
            _gridManager.RemoveBuilding(gridPos);
            Debug.Log($"Removed building at {gridPos}");
        }
        else
        {
            Debug.Log($"No building found at {gridPos}");
        }
    }
}
