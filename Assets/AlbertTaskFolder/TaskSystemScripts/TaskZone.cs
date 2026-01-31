using UnityEngine;

public class TaskZone : MonoBehaviour, IInteractable
{
    [SerializeField] private Tasks taskToComplete;

    public void Interact()
    {
        taskToComplete.taskCompleted = true;
    }
}
