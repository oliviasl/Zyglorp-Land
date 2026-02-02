using UnityEngine;

[CreateAssetMenu(fileName = "TaskInfoSO", menuName = "Scriptable Objects/TaskInfoSO")]
public class TaskInfoSO : ScriptableObject
{
    [SerializeField] public string taskNameText;
    [SerializeField] public int timeWhenTaskIsDue;
    [SerializeField] public int timeToShowForTask;

    [TextArea(3, 10)]
    [SerializeField] public string taskInfoText;
}
