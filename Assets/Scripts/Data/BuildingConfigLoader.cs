using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BuildingConfigLoader
{
    private const string CONFIG_PATH = "Configs/buildings.json";

    public static List<BuildingConfig> LoadConfigs()
    {
        string fullPath = Path.Combine(Application.dataPath, CONFIG_PATH);

        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[ConfigLoader] Не найден файл конфигурации: {fullPath}");
            return new List<BuildingConfig>();
        }

        string json = File.ReadAllText(fullPath);
        var wrapper = JsonUtility.FromJson<BuildingConfigWrapper>(json);
        Debug.Log("Загрузка произошла успешно");
        return wrapper?.Buildings ?? new List<BuildingConfig>();
    }

    [System.Serializable]
    private class BuildingConfigWrapper
    {
        public List<BuildingConfig> Buildings;
    }
}
