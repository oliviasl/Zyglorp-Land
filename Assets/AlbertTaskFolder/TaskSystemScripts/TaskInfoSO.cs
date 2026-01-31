using UnityEngine;

[CreateAssetMenu(fileName = "TaskInfoSO", menuName = "Scriptable Objects/TaskInfoSO")]
public class TaskInfoSO : ScriptableObject
{
    [SerializeField] private string taskNameText;
    [SerializeField] private int timeWhenTaskIsDue;

    [TextArea(3, 10)]
    [SerializeField] private string taskInfoText;
}
