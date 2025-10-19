using UnityEngine;
using UnityEngine.InputSystem;

public class DeletionManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private InputManager _inputManager;

    private Camera _camera;
    private bool _isActive;

    private void Awake()
    {
        _camera = Camera.main; 
    }

    public void EnableMode()
    {
        if (_isActive) return;
        _isActive = true;
        SubscribeInput();
        Debug.Log("Deletion enabled");
    }

    public void DisableMode()
    {
        if (!_isActive) return;
        _isActive = false;
        UnsubscribeInput();
        Debug.Log("Deletion disabled");
    }

    private void SubscribeInput()
    {
        if (_inputManager == null) return;
        var input = _inputManager.InputActions.GridPlacement;
        input.Delete.performed += ctx => TryDeleteUnderCursor();
    }

    private void UnsubscribeInput()
    {
        if (_inputManager == null) return;
        var input = _inputManager.InputActions.GridPlacement;
        input.Delete.performed -= ctx => TryDeleteUnderCursor();
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
