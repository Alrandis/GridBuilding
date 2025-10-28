using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public Dictionary<string, object> Data = new();
}

public class SaveManager : MonoBehaviour
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
    private List<ISaveable> _saveables = new();

    private void Awake()
    {
        // Находим все объекты, которые реализуют ISaveable
        _saveables = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<ISaveable>()
            .ToList();
    }

    public void SaveGame()
    {
        var saveFile = new SaveFile();

        foreach (var saveable in _saveables)
        {
            saveFile.Data[saveable.GetType().Name] = saveable.CaptureState();
        }

        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All, // сохраняем типы, чтобы потом корректно десериализовать
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(saveFile, jsonSettings);
        File.WriteAllText(SavePath, json);

        Debug.Log($"[SaveManager] Игра сохранена: {SavePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("[SaveManager] Нет сохранённого файла!");
            return;
        }

        string json = File.ReadAllText(SavePath);

        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        var saveFile = JsonConvert.DeserializeObject<SaveFile>(json, jsonSettings);

        foreach (var saveable in _saveables)
        {
            if (saveFile.Data.TryGetValue(saveable.GetType().Name, out object state))
            {
                saveable.RestoreState(state);
            }
        }

        Debug.Log("[SaveManager] Игра загружена");
    }
}
