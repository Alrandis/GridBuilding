using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDatabase : MonoBehaviour
{
    [SerializeField] private Dictionary<string, BuildingConfig> _configsById;

    public static BuildingDatabase Instance;
    public bool IsInitialized { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Initialize(List<BuildingConfig> configs)
    {
        _configsById = new Dictionary<string, BuildingConfig>();
        foreach (var config in configs)
        {
            if (!_configsById.ContainsKey(config.Id))
                _configsById.Add(config.Id, config);
        }
        IsInitialized = true;
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
