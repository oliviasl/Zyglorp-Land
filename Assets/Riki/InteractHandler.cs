using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class InteractHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        private float minimumProximity = 2.5f;
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
            if (AlienManager.Instance == null) return;
            
            float closestDistance = minimumProximity;
            Child victim = null;
            
            foreach (var child in AlienManager.Instance.children)
            {
                float dist = Vector3.Distance(transform.position, child.transform.position);
                if (dist < closestDistance && child.state != Child.ChildState.Abused)
                {
                    closestDistance = dist;
                    victim = child;
                }
            }

            if (victim != null)
            {
                victim.Abuse();
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + (minimumProximity * Camera.main.transform.forward));
        }
    }
}