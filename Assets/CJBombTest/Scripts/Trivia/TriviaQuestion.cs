using UnityEngine;

[CreateAssetMenu(fileName = "TriviaQuestion", menuName = "Scriptable Objects/TriviaQuestion")]
public class TriviaQuestion : ScriptableObject
{
    public string question;
    public string[] answers;
}
