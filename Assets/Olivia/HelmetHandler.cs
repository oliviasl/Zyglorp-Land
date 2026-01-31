using UnityEngine;
using UnityEngine.InputSystem;

public class HelmetHandler : MonoBehaviour
{
    [SerializeField] private Canvas helmetUICanvas;
    
    private PlayerInput _playerInput;
    private bool _bIsHelmetOn;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput != null)
        {
            InputAction helmetAction = _playerInput.actions.FindAction("MoveHelmet");
            helmetAction.performed += ToggleHelmetUI;
        }
    }

    private void Start()
    {
        _bIsHelmetOn = false;
        helmetUICanvas.enabled = false;
    }

    private void ToggleHelmetUI(InputAction.CallbackContext obj)
    {
        helmetUICanvas.enabled = !_bIsHelmetOn;
        _bIsHelmetOn = !_bIsHelmetOn;
    }
    
}
