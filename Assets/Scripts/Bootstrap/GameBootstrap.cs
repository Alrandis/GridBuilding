using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    public static BuildingDatabase BuildingDatabase { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var configs = BuildingConfigLoader.LoadConfigs();
        BuildingDatabase = new BuildingDatabase(configs);

        Debug.Log($"[Bootstrap] Загружено конфигураций зданий: {configs.Count}");
    }
}

