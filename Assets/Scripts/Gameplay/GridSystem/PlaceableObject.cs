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

    public IEnumerable<Vector2Int> GetWorldCells(Vector2Int gridOrigin)
    {
        foreach (var cell in _occupiedCells)
            yield return gridOrigin + (cell - _pivot);
    }
}

