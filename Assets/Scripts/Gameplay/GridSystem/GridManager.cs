using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float _cellSize = 1f;

    private Dictionary<Vector2Int, GameObject> _placedBuildings = new();

    public float CellSize => _cellSize;

    // Только для тестирования. Позже удалю
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var gridPos = GetGridPosition(mouseWorld);
            Debug.Log($"Clicked cell: {gridPos}");
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        // Сдвигаем в центр клетки
        return new Vector3(
            x * _cellSize + _cellSize / 2f,
            y * _cellSize + _cellSize / 2f,
            0
        );
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / _cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / _cellSize);
        return new Vector2Int(x, y);
    }

    public bool IsOccupied(Vector2Int gridPos)
    {
        return _placedBuildings.ContainsKey(gridPos);
    }

    public void PlaceBuilding(Vector2Int gridPos, GameObject building)
    {
        if (IsOccupied(gridPos)) return;
        _placedBuildings.Add(gridPos, building);
    }

    public void RemoveBuilding(Vector2Int gridPos)
    {
        if (!_placedBuildings.ContainsKey(gridPos)) return;
        Destroy(_placedBuildings[gridPos]);
        _placedBuildings.Remove(gridPos);
    }
}
