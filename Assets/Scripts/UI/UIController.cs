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
    [SerializeField] private Button _building1Button;
    [SerializeField] private Button _building2Button;
    [SerializeField] private Button _building3Button;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;

    private void Awake()
    {
        // Основные режимы
        _placeModeButton.onClick.AddListener(EnablePlacementMode);
        _deleteModeButton.onClick.AddListener(EnableDeletionMode);

        // Кнопки выбора зданий (пока только пример, можно передавать prefab)
        _building1Button.onClick.AddListener(() => SelectBuilding(1));
        _building2Button.onClick.AddListener(() => SelectBuilding(2));
        _building3Button.onClick.AddListener(() => SelectBuilding(3));

        // Сохранение / загрузка
        _saveButton.onClick.AddListener(SaveData);
        _loadButton.onClick.AddListener(LoadData);
    }

    #region Mode Switching
    private void EnablePlacementMode()
    {
        _deletionManager.DisableMode();
        _placementManager.EnableMode();
        Debug.Log("Switched to Placement Mode");
    }

    private void EnableDeletionMode()
    {
        _placementManager.DisableMode();
        _deletionManager.EnableMode();
        Debug.Log("Switched to Deletion Mode");
    }
    #endregion

    #region Building Selection
    private void SelectBuilding(int index)
    {
        // Здесь можно передавать prefab в PlacementManager
        // Например:
        // _placementManager.SetCurrentBuilding(_buildingPrefabs[index]);
        Debug.Log($"Selected building {index}");
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
