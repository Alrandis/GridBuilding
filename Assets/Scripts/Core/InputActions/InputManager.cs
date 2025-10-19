using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputActionSystem _inputActions;
    public InputActionSystem InputActions => _inputActions;

    private void Awake()
    {
        _inputActions = new InputActionSystem();
        _inputActions.Enable();
    }
}
