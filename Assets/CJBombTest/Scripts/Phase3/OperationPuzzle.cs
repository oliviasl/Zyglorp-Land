using UnityEngine;

public class OpeartionPuzzle : Puzzle
{
    [SerializeField] GameObject ButtonStart;
    [SerializeField] GameObject ButtonEnd;
    bool gameActive = false;
    ShowHide handler = ShowHide.instance;

    public void StartGame()
    {
        gameActive = true;
        handler.Hide(ButtonStart);
        handler.Show(ButtonEnd);
    }

    public void TouchedEdge()
    {
        Debug.Log("ouch");
        gameActive = false;
        handler.Show(ButtonStart);
        handler.Hide(ButtonEnd);
    }

    public void EndGame()
    {
        gameActive = false;
        handler.Hide(ButtonEnd);

        base.Solve();
    }
}
