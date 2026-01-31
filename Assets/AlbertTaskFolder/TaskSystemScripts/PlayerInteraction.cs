using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    private IInteractable currentInteractable = null;
    [SerializeField] private InputActionReference interactAction; 

    [Header("Interact UI")]
    [SerializeField] private GameObject interactUIObjext; //if we are doing an interact UI

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void Update()
    {
        if (currentInteractable != null && interactAction.action.WasPressedThisFrame())
        {
            currentInteractable.Interact();
            interactUIObjext.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Near interactable object:" + other.name);
            interactUIObjext.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("Left interactable object");
            interactUIObjext.SetActive(false);
        }
    }
}
