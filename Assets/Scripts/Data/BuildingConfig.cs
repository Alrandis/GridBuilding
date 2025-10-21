using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BuildingConfig
{
    public string Id;
    public string PrefabPath;
    public Vector2Int Pivot;
    public List<Vector2Int> OccupiedCells;
    public string DisplayName;
    public int Cost;
}
