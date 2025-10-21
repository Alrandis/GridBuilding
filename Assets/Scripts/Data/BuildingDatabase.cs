using System.Collections.Generic;
using UnityEngine;

public class BuildingDatabase
{
    private Dictionary<string, BuildingConfig> _configsById;

    public BuildingDatabase(List<BuildingConfig> configs)
    {
        _configsById = new Dictionary<string, BuildingConfig>();
        foreach (var config in configs)
        {
            if (!_configsById.ContainsKey(config.Id))
                _configsById.Add(config.Id, config);
        }
    }

    public BuildingConfig GetById(string id)
    {
        if (_configsById.TryGetValue(id, out var config))
            return config;

        Debug.LogError($"[BuildingDatabase] Не найден конфиг здания с Id = {id}");
        return null;
    }

    public IEnumerable<BuildingConfig> GetAll() => _configsById.Values;
}
