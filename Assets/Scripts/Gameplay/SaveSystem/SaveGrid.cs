using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class GridSaveData
{
    public List<BuildingData> Buildings = new();
}

[System.Serializable]
public class BuildingData
{
    public string BuildingId;
    public Vector2Int Position;
    public float Rotation;
}

public class SaveGrid : MonoBehaviour, ISaveable
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private BuildingDatabase _buildingDatabase;

    public object CaptureState()
    {
        var data = new GridSaveData();
        var added = new HashSet<PlacedBuilding>();

        foreach (var entry in _gridManager.GetPlacedBuildings())
        {
            if (!added.Add(entry.Value)) continue;

            var buildingId = entry.Value.BuildingId;
            data.Buildings.Add(new BuildingData
            {
                BuildingId = buildingId,
                Position = entry.Key,
                Rotation = entry.Value.GameObject.transform.eulerAngles.z
            });
        }

        return data;
    }

    public void RestoreState(object state)
    {
        var data = (GridSaveData)state;

        _gridManager.ClearGrid();

        foreach (var buildingData in data.Buildings)
        {
            var config = _buildingDatabase.GetById(buildingData.BuildingId);
            if (config == null)
            {
                Debug.LogWarning($"[SaveGrid] Config not found for {buildingData.BuildingId}");
                continue;
            }

            // Загружаем префаб
            var prefab = Resources.Load<GameObject>(config.PrefabPath);
            if (prefab == null)
            {
                Debug.LogWarning($"[SaveGrid] Prefab not found at path {config.PrefabPath}");
                continue;
            }

            // Получаем мировую позицию pivot-а
            var worldPos = _gridManager.GetWorldPosition(buildingData.Position.x, buildingData.Position.y);

            // Создаём объект
            var go = Instantiate(prefab, worldPos, Quaternion.Euler(0, 0, buildingData.Rotation));

            // Переводим локальные координаты занятых клеток из конфигурации в мировые
            var occupiedCells = new List<Vector2Int>();
            foreach (var localCell in config.OccupiedCells)
            {
                var rotated = RotateCell(localCell, buildingData.Rotation);
                var globalCell = buildingData.Position + rotated;
                occupiedCells.Add(globalCell);
            }

            // Помечаем все эти клетки занятыми
            _gridManager.OccupyCells(occupiedCells, go, buildingData.BuildingId);
        }
    }

    private Vector2Int RotateCell(Vector2Int cell, float rotation)
    {
        int rot = Mathf.RoundToInt(rotation) % 360;
        return rot switch
        {
            0 => cell,
            90 => new Vector2Int(-cell.y, cell.x),
            180 => new Vector2Int(-cell.x, -cell.y),
            270 => new Vector2Int(cell.y, -cell.x),
            _ => cell
        };
    }

}
