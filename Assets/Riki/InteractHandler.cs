using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class InteractHandler : MonoBehaviour
    {
        [SerializeField] private AlienManager manager;
        private PlayerInput playerInput;
        private float minimumProximity = 10f;
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            if (playerInput != null)
            {
                InputAction interactAction = playerInput.actions.FindAction("Interact");
                interactAction.performed += Interact;
            }
        }

        public void Interact(InputAction.CallbackContext obj)
        {
            float closestDistance = minimumProximity;
            Child victim = null;
            foreach (var child in manager.children)
            {
                float dist = Vector3.Distance(transform.position, child.transform.position);
                if (dist < closestDistance && child.state != Child.ChildState.Abused)
                {
                    child.Abuse();
                    closestDistance = dist;
                    victim = child;
                }
            }

            if (victim != null)
            {
                victim.Abuse();
            }
        }
    }
}