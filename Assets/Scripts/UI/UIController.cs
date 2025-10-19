using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private DeletionManager _deletionManager;

    [Header("Buttons")]
    [SerializeField] private Button _placeModeButton;
    [SerializeField] private Button _deleteModeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private BuildingButton[] _buildingButtons;

    [SerializeField] private GameObject _buildingPanel;

    private void Awake()
    {
        _buildingPanel.SetActive(false);
        // Основные режимы
        _placeModeButton.onClick.AddListener(EnablePlacementMode);
        _deleteModeButton.onClick.AddListener(EnableDeletionMode);

        for (int i = 0; i < _buildingButtons.Length; i++)
        {
            _buildingButtons[i].OnBuildingSelected += SelectBuilding;
        }

        // Сохранение / загрузка
        _saveButton.onClick.AddListener(SaveData);
        _loadButton.onClick.AddListener(LoadData);
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
        // Пока заглушка
        Debug.Log("Save triggered");
        // Позже вызывать SaveSystem или аналог
    }

    private void LoadData()
    {
        // Пока заглушка
        Debug.Log("Load triggered");
        // Позже вызывать LoadSystem или аналог
    }
    #endregion
}
