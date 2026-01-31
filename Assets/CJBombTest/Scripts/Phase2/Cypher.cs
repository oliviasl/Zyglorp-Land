using UnityEngine;
using TMPro;

public class Cypher : Puzzle
{
    [SerializeField] string answer = "";
    [SerializeField] TMP_Text playerText;
    string playerResponse = "";

    void Start()
    {
        Clear();
    }

    public void PressKey(string s)
    {
        playerResponse += s;
        Debug.Log(playerResponse);
        playerText.text = playerResponse;
        if (playerResponse.Equals(answer))
        {
            base.Solve();   
        }
        else if (playerResponse.Length > answer.Length) {
            Clear();
        }
    }

    public void Clear()
    {
        playerResponse = "";
        playerText.text = playerResponse;
    }
}
