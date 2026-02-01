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
        BombManager.instance.GetBAM().ButtonClickSFX();
        Debug.Log(playerResponse);
        playerText.text = playerResponse;
        if (playerResponse.Equals(answer))
        {
            base.Solve();   
        }
        else if (playerResponse.Length > answer.Length) {
            BombManager.instance.GetBAM().FailureSFX();
            Clear();
        }
    }

    public void Clear()
    {
        playerResponse = "";
        playerText.text = playerResponse;
    }

    public void ClickClear()
    {
        BombManager.instance.GetBAM().ButtonClickSFX();
        Clear();
    }
}
