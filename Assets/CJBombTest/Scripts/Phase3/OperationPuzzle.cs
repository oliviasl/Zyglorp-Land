using UnityEngine;

public class OpeartionPuzzle : Puzzle
{
    [SerializeField] GameObject ButtonStart;
    [SerializeField] GameObject ButtonEnd;
    bool gameActive = false;
    ShowHide handler;

    void Start()
    {
        handler = ShowHide.instance;

        handler.Hide(ButtonEnd);
    }

    public void StartGame()
    {
        gameActive = true;
        handler.Hide(ButtonStart);
        handler.Show(ButtonEnd);
    }

    public void TouchedEdge()
    {
        if (!gameActive) { return; }
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
