using UnityEngine;
using TMPro;

public class KickChild : Puzzle
{
    [SerializeField] TMP_Text childText;
    [SerializeField] int requiredChildren = 15;
    int kickedChildren = 0;

    void Start()
    {
        kickedChildren = 0;
        childText.text = kickedChildren.ToString();
    }

    public void IncrementChildCounter()
    {
        kickedChildren++;
        childText.text = kickedChildren.ToString();
        if(kickedChildren >= requiredChildren && !base.GetSolved())
        {
            base.Solve();
        }
    }
}
