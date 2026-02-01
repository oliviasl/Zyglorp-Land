using AllIn13DShader;
using UnityEngine;

public class TaskZone : MonoBehaviour, IInteractable
{
    [SerializeField] private Tasks taskToComplete;
   // [SerializeField] public Material ogMat;
   // [SerializeField] public Material outlineMat;

    public void Interact()
    {
        if(taskToComplete != null)
        {
            taskToComplete.taskCompleted = true;
            TaskManager.instance.UpdateTaskUI();
            Debug.Log("Task Completed");
            
        }
        
    }

    
}
