using UnityEngine;
using System.Collections.Generic;

public class PlaceableObject : MonoBehaviour
{
    [Tooltip("Список клеток, которые занимает объект, в локальных координатах относительно pivot.")]
    [SerializeField]
    private List<Vector2Int> _occupiedCells = new List<Vector2Int>() { Vector2Int.zero };

    [Tooltip("Опорная точка (положение центра привязки на сетке)")]
    [SerializeField]
    private Vector2Int _pivot = Vector2Int.zero;

    public IReadOnlyList<Vector2Int> OccupiedCells => _occupiedCells;
    public Vector2Int Pivot => _pivot;

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

