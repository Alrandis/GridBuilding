using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _label;

    private Button _button;

    public event Action<GameObject> OnBuildingSelected;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void Initialize(string displayName, string imagePath, string prefabPath)
    { 
        _label.text = displayName;

        // Загружаю иконку и префаб из ресурсов
        var sprite = Resources.Load<Sprite>(imagePath);
        if (sprite != null)
            _icon.sprite = sprite;
        else
            Debug.LogWarning($"[BuildingButton] Не найдена иконка по пути: {imagePath}");
        
        var prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab != null)
            _prefabToSpawn = prefab;
        else
            Debug.LogWarning($"[BuildingButton] Не найден перфаб по пути: {prefabPath}");
    }

    private void OnClick()
    {
        OnBuildingSelected?.Invoke(_prefabToSpawn);
    }
}
