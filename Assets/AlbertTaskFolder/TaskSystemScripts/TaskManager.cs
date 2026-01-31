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

    [Header("Timer")]
   // [SerializeField] float actualElaspedTime;
    [SerializeField] private int displayedTime;
    private int displayedTimeToShow = 0; //this is stupid wahtever
    [SerializeField] private bool timerStarted;
    [SerializeField] private bool isUsingAM;
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
        if(taskAction.action.WasPressedThisFrame())
        {
            if(!taskListIsVisible)
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
            if(!timerIsOn)
            {
                StartCoroutine(WaitToIncreaseTime());
                timerIsOn = true;
            }
           

        }
    }

    public void UpdateTaskUI()
    {
        for (int i = 0; i < listOfTasks.Length; i++)
        {
            if(listOfTasks[i] != null)
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

    private void TimerHandler()
    {
        if(displayedTime == 9)
        {
            isUsingAM = true;
            displayedTimeToShow = 9;
        }
        else if (displayedTime == 10)
        {
            isUsingAM = true;
            displayedTimeToShow = 10;
        }
        else if (displayedTime == 11)
        {
            isUsingAM = true;
            displayedTimeToShow = 11;
        }
        else if (displayedTime == 12)
        {
            
            displayedTimeToShow = 12;
        }
        else if (displayedTime == 13)
        {
            
            displayedTimeToShow = 1;
        }
        else if (displayedTime == 14)
        {
            
            displayedTimeToShow = 2;
        }
        else if (displayedTime == 15)
        {

            displayedTimeToShow = 3;
        }
        else if (displayedTime == 16)
        {

            displayedTimeToShow = 4;
        }
        else if (displayedTime == 17)
        {

            displayedTimeToShow = 5;
        }



    }

    IEnumerator WaitToIncreaseTime()
    {
        
        while(displayedTime < 17)
        {
            yield return new WaitForSeconds(timeAnHourTakes);
            displayedTime += 1;

            TimerHandler();

            string timeOfDay;

            if (isUsingAM)
            {
                timeOfDay = "A.M";
            }
            else
            {
                timeOfDay = "P.M";
            }

            displayTimeText.text = displayedTimeToShow.ToString() + ":00 " + timeOfDay;

            Debug.Log(displayedTime);
            Debug.Log(displayedTimeToShow);
        }

        
        
    }
    public void StartTimer()
    {
        // where we start the timer after the game officialyl starts
        timerStarted = true;
    }
}
