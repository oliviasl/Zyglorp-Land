using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance { get; private set; }

    [Header("Task Info")]
    [SerializeField] Tasks[] listOfTasks;

    [Header("Timer")]
    [SerializeField] private int actualTime;
    [SerializeField] private int displayedTime;

    [Header("Task UI Info")]
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

        UpdateTaskUI(); //to start the task stuff
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateTaskUI()
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
    public void StartTimer()
    {
        // where we start the timer after the game officialyl starts
    }
}
