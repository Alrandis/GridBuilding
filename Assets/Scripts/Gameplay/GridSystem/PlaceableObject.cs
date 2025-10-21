using UnityEngine;
using System.Collections.Generic;

public class PlaceableObject : MonoBehaviour
{
    [Tooltip("Уникальный Id для здания, что нужен, чтобы подставить данные из конфига.")]
    [SerializeField] private string _configId;
    public string ConfigId => _configId;
    [Tooltip("Список клеток, которые занимает объект, в локальных координатах относительно pivot.")]
    [SerializeField] private List<Vector2Int> _occupiedCells = new List<Vector2Int>() { Vector2Int.zero };
    [Tooltip("Опорная точка (положение центра привязки на сетке)")]
    [SerializeField] private Vector2Int _pivot = Vector2Int.zero;

    public void Initialize()
    {
        var config = BuildingDatabase.Instance.GetById(_configId);
        if (config == null) return;

        _occupiedCells = new List<Vector2Int>(config.OccupiedCells);
        _pivot = config.Pivot;
    }

    public IEnumerable<Vector2Int> GetWorldCells(Vector2Int gridOrigin, int rotationDegrees = 0)
    {
        foreach (var cell in _occupiedCells)
        {
            Vector2Int local = cell - _pivot;
            Vector2Int rotated = RotateCell(local, rotationDegrees);
            yield return gridOrigin + rotated;
        }
    }

    private Vector2Int RotateCell(Vector2Int cell, int rotationDegrees)
    {
        switch (rotationDegrees % 360)
        {
            case 0: return cell;
            case 90: return new Vector2Int(-cell.y, cell.x);
            case 180: return new Vector2Int(-cell.x, -cell.y);
            case 270: return new Vector2Int(cell.y, -cell.x);
            default:
                Debug.LogWarning("Rotation must be a multiple of 90!");
                return cell;
        }
    }
}

