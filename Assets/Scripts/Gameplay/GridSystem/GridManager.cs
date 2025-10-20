using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlacedBuilding
{
    public GameObject GameObject { get; }
    public Vector2Int[] Cells { get; }

    public PlacedBuilding(GameObject go, IEnumerable<Vector2Int> cells)
    {
        GameObject = go;
        Cells = cells.ToArray(); // <-- преобразуем IEnumerable в массив
    }
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private float _cellSize = 1f;

    private Dictionary<Vector2Int, PlacedBuilding> _placedBuildings = new();

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

    public bool IsAreaFree(IEnumerable<Vector2Int> cells)
    {
        foreach (var cell in cells)
        {
            if (_placedBuildings.ContainsKey(cell))
                return false;
        }
        return true;
    }

    public void OccupyCells(IEnumerable<Vector2Int> cells, GameObject building)
    {
        PlacedBuilding placedBuilding = new PlacedBuilding(building, cells);

        foreach (var cell in placedBuilding.Cells)
        {
            _placedBuildings[cell] = placedBuilding;
        }
    }

    public void RemoveBuilding(Vector2Int gridPos)
    {
        if (!_placedBuildings.ContainsKey(gridPos)) return;
        PlacedBuilding building = _placedBuildings[gridPos];

        // Удаляем все ключи из словаря
        foreach (var cell in building.Cells)
            _placedBuildings.Remove(cell);

        // Удаляем объект со сцены
        Destroy(building.GameObject);
    }
}
