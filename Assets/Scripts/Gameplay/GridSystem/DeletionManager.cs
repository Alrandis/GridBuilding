using UnityEngine;
using UnityEngine.InputSystem;

public class DeletionManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private InputManager _inputManager;

    private bool _isActive;
    private void OnEnable()
    {
        _inputManager.OnDelete += TryDeleteUnderCursor;
    }

    private void OnDisable()
    {
        _inputManager.OnDelete -= TryDeleteUnderCursor;
    }

    public void EnableMode()
    {
        if (_isActive) return;
        _isActive = true;
        Debug.Log("Deletion enabled");
    }

    public void DisableMode()
    {
        if (!_isActive) return;
        _isActive = false;
     
        Debug.Log("Deletion disabled");
    }

    private void TryDeleteUnderCursor()
    {
        if (!_isActive) return;

        Vector3 mouseWorld = _inputManager.MouseWorldPosition;

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
