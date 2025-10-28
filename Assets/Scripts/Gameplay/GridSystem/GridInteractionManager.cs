using System;
using UnityEngine;

public enum InteractionMode
{
    Idle,
    Placement,
    Deletion
}

public class GridInteractionManager : MonoBehaviour
{
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private DeletionManager _deletionManager;

    private InteractionMode _currentMode = InteractionMode.Idle;

    // Событие для UI/View — подписывайтесь, чтобы обновлять визуал
    public event Action<InteractionMode> OnModeChanged;

    public InteractionMode CurrentMode => _currentMode;

    private void Start()
    {
        // Убеждаемся, что все менеджеры выключены вначале
        if (_placementManager != null) _placementManager.DisableMode();
        if (_deletionManager != null) _deletionManager.DisableMode();
        SetMode(InteractionMode.Idle);
    }

    public void SetMode(InteractionMode mode)
    {
        if (_currentMode == mode) return;

        // Выключаем текущее
        switch (_currentMode)
        {
            case InteractionMode.Placement:
                if (_placementManager != null) _placementManager.DisableMode();
                break;
            case InteractionMode.Deletion:
                if (_deletionManager != null) _deletionManager.DisableMode();
                break;
        }

        // Включаем новое
        _currentMode = mode;
        switch (_currentMode)
        {
            case InteractionMode.Placement:
                if (_placementManager != null) _placementManager.EnableMode();
                break;
            case InteractionMode.Deletion:
                if (_deletionManager != null) _deletionManager.EnableMode();
                break;
            case InteractionMode.Idle:
                // ничего дополнительно
                break;
        }

        OnModeChanged?.Invoke(_currentMode);
        Debug.Log($"Mode changed: {_currentMode}");
    }

    public void TogglePlacementMode()
    {
        SetMode(_currentMode == InteractionMode.Placement ? InteractionMode.Idle : InteractionMode.Placement);
    }

    public void ToggleDeletionMode()
    {
        SetMode(_currentMode == InteractionMode.Deletion ? InteractionMode.Idle : InteractionMode.Deletion);
    }

    // Для UI: быстро выключить все режимы
    public void SetIdle()
    {
        SetMode(InteractionMode.Idle);
    }
}
