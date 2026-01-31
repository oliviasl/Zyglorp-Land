using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class InteractHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();

            if (_playerInput != null)
            {
                InputAction interactAction = _playerInput.actions.FindAction("Interact");
                interactAction.performed += Interact;
            }
        }

        public void Interact(InputAction.CallbackContext obj)
        {
            
        }
    }
}