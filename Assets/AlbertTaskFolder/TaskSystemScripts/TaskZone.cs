using UnityEngine;

public class TaskZone : MonoBehaviour, IInteractable
{
    [SerializeField] private Tasks taskToComplete;

    public void Interact()
    {
        taskToComplete.taskCompleted = true;
        TaskManager.instance.UpdateTaskUI();
        Debug.Log("Task Completed");
    }

    
}
