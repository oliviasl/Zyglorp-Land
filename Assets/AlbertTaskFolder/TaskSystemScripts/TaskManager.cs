using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance { get; private set; }

    [Header("Task Info")]
    [SerializeField] Tasks[] listOfTasks;
    [SerializeField] public bool taskHasBeenFailed;

    [Header("Timer")]
    [SerializeField] private int displayedTime = 0;
    [SerializeField] private int displayedTimeToShow = 0; //this is stupid wahtever
    [SerializeField] private bool timerStarted;
    [SerializeField] private float timeAnHourTakes;
    [SerializeField] private bool timerIsOn;

    [Header("Timer UI")]
    [SerializeField] private TMP_Text displayTimeText;
    //[SerializeField] private TMP_Text debugElapsedText;

    [Header("Task Input")]
    [SerializeField] private InputActionReference taskAction;
    [SerializeField] private bool taskListIsVisible;

    [Header("Task UI Info")]
    [SerializeField] GameObject completeTaskUICanvas;
    [SerializeField] TMP_Text[] listOfTaskNames;
    [SerializeField] TMP_Text[] listOfTaskNumbers;
    [SerializeField] Image[] scribbleImage; //the scribbled out versions of the task, just put it on top

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

        }

        displayedTime += 1;


        string startingTimeToShow = "9";
        displayTimeText.text = startingTimeToShow + ":00 A.M";

        UpdateTaskUI(); //to start the task stuff
    }
    private void OnEnable()
    {
        taskAction.action.Enable();
    }

    private void OnDisable()
    {
        taskAction.action.Disable();
    }

    private void Update()
    {
        if (taskAction.action.WasPressedThisFrame())
        {
            if (!taskListIsVisible)
            {
                completeTaskUICanvas.SetActive(true);
                taskListIsVisible = true;
            }
            else
            {
                completeTaskUICanvas.SetActive(false);
                taskListIsVisible = false;
            }

        }

        if (timerStarted)
        {
            if (!timerIsOn)
            {
                StartCoroutine(WaitToIncreaseTime());
                timerIsOn = true;
            }
        }

        if(taskHasBeenFailed)
        {
            // run alert task failed on manager
        }
    }

    private void CheckTaskCompletion()
    {
        if (!listOfTasks[0].taskCompleted && displayedTime == 2)
        {
            taskHasBeenFailed = true;
            listOfTasks[0].taskCompleted = true;
        }
        else if (!listOfTasks[1].taskCompleted && displayedTime == 4)
        {
            taskHasBeenFailed = true;
            listOfTasks[1].taskCompleted = true;
        }
        else if (!listOfTasks[2].taskCompleted && displayedTime == 6)
        {
            taskHasBeenFailed = true;
            listOfTasks[2].taskCompleted = true;
        }
        else if (!listOfTasks[3].taskCompleted && displayedTime == 8)
        {
            taskHasBeenFailed = true;
            listOfTasks[3].taskCompleted = true;
        }
       
        UpdateTaskUI();
        
    }
    public void UpdateTaskUI()
    {
        for (int i = 0; i < listOfTasks.Length; i++)
        {
            if (listOfTasks[i] != null)
            {
                if (listOfTasks[i].taskCompleted)
                {
                    //do task completed stuff
                    scribbleImage[i].gameObject.SetActive(true);
                }
                else //task is not complete
                {
                    string taskName = listOfTasks[i].name;
                    listOfTaskNames[i].text = taskName;

                }
            }

        }


    }

    IEnumerator WaitToIncreaseTime()
    {
        while (displayedTime <= 17)
        {
            yield return new WaitForSeconds(timeAnHourTakes);
            displayedTime += 1;

            string timeOfDay;

            if (displayedTime+8 < 12)
            {
                timeOfDay = "A.M";
            }
            else
            {
                timeOfDay = "P.M";
            }

            displayedTimeToShow = (displayedTime+ 8)  % 12;
            if (displayedTimeToShow == 0)
            {
                displayedTimeToShow = 12;
            }
            displayTimeText.text = displayedTimeToShow.ToString() + ":00 " + timeOfDay;

            CheckTaskCompletion();

            Debug.Log(displayedTime);
            Debug.Log(displayedTimeToShow);
        }
    }
}
