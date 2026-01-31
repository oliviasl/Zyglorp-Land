using UnityEngine;

public class SamplePuzzle : Puzzle
{
    public void SaySomething(string text)
    {
        Debug.Log(text);
        if (text.Equals("Yippee"))
        {
            base.Solve();
        }
    }
}
