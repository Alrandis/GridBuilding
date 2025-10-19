using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private Image _icon;

    private Button _button;

    public event Action<GameObject> OnBuildingSelected;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnBuildingSelected?.Invoke(_prefabToSpawn);
    }
}
