using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private DeletionManager _deletionManager;
    [SerializeField] private SaveManager _saveManager;

    [Header("Buttons")]
    [SerializeField] private Button _placeModeButton;
    [SerializeField] private Button _deleteModeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private List<BuildingButton> _buildingButtons = new();

    [SerializeField] private GameObject _buildingPanel;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _buttonPrefab; // Префаб кнопки с компонентом BuildingButton

    private void Awake()
    {
        _buildingPanel.SetActive(false);
        // Основные режимы
        _placeModeButton.onClick.AddListener(EnablePlacementMode);
        _deleteModeButton.onClick.AddListener(EnableDeletionMode);

        // Сохранение / загрузка
        _saveButton.onClick.AddListener(SaveData);
        _loadButton.onClick.AddListener(LoadData);
    }

    private IEnumerator Start()
    {
        // Ждем пока BuildingDatabase.Instance появится и инициализируется
        yield return new WaitUntil(() =>
            BuildingDatabase.Instance != null &&
            BuildingDatabase.Instance.IsInitialized
        );

        List<BuildingConfig> buildingConfigs = BuildingDatabase.Instance.GetAll().ToList();

        foreach (BuildingConfig config in buildingConfigs)
        {
            CreateButtonForBuilding(config);
        }
    }

    private void CreateButtonForBuilding(BuildingConfig config)
    {
        GameObject buildingGameObj = Instantiate(_buttonPrefab, _content.transform);

        BuildingButton buildingButton = buildingGameObj.GetComponent<BuildingButton>();
        buildingButton.Initialize(config.DisplayName, config.ImagePath, config.PrefabPath); 
        buildingButton.OnBuildingSelected += SelectBuilding;

        _buildingButtons.Add(buildingButton);
    }

    #region Mode Switching
    private void EnablePlacementMode()
    {
        _buildingPanel.SetActive(true);
        _deletionManager.DisableMode();
        _placementManager.EnableMode();
        Debug.Log("Switched to Placement Mode");
    }

    private void EnableDeletionMode()
    {
        _buildingPanel.SetActive(false);
        _placementManager.DisableMode();
        _deletionManager.EnableMode();
        Debug.Log("Switched to Deletion Mode");
    }
    #endregion

    #region Building Selection
    private void SelectBuilding(GameObject building)
    {
        _placementManager.SetBuilding(building);
        Debug.Log($"Selected building {building}");
    }
    #endregion

    #region Save / Load
    private void SaveData()
    {
        _saveManager.SaveGame();
        Debug.Log("Save triggered");
        
    }

    private void LoadData()
    {
        _saveManager.LoadGame();
        Debug.Log("Load triggered");
    }
    #endregion
}
