using UnityEngine;

[CreateAssetMenu(fileName = "CoffeeWindow", menuName = "Scriptable Objects/CoffeeWindow")]
public class CoffeeWindow : ScriptableObject
{
    public string prompt;
    public string[] responses;
}
