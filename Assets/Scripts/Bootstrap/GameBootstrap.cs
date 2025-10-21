using System.Collections;
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
        StartCoroutine(InitializeAfterSceneLoaded());
    }

    private IEnumerator InitializeAfterSceneLoaded()
    {
        // Ждём, пока появится BuildingDatabase.Instance
        yield return new WaitUntil(() => BuildingDatabase.Instance != null);

        BuildingDatabase.Instance.Initialize(_configs);
        Debug.Log("[Bootstrap] MainScene инициализирована");
    }
}
