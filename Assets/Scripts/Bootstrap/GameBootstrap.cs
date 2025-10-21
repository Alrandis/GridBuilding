using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    private List<BuildingConfig> _configs;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _configs = BuildingConfigLoader.LoadConfigs();
   
        //_selectionService = new BuildingSelectionService();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainScene"); // Загружаем основную сцену
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Когда сцена загружена, ищем нужные менеджеры
        BuildingDatabase.Instance.Initialize(_configs);

        //if (placementManager != null)
        //    placementManager.Initialize(_buildingDatabase);

        Debug.Log("[Bootstrap] MainScene инициализирована");
    }
}
