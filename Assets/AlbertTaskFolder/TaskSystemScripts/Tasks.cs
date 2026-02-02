using UnityEngine;

public class Tasks : MonoBehaviour
{
    [Header("Task Info")]
    [SerializeField] public bool taskCompleted;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private string animationToPlay;
    [SerializeField] public TaskInfoSO taskInfo; //here we hold our task names and strings
    // we want to use the task manager to take out the strings and place it on whatever UI element we have
    // for the task to be seen on

    


    private void Update()
    {
        if(taskCompleted)
        {
            if(playerAnim != null && animationToPlay != null)
            {
                playerAnim.SetTrigger(animationToPlay); //set the trigger for the animation to play
            }


           
        }
    }
}
