using UnityEngine;

public class OpeartionPuzzle : Puzzle
{
    [SerializeField] GameObject ButtonStart;
    [SerializeField] Animator ButtonStartAnim;
    [SerializeField] GameObject ButtonEnd;
    [SerializeField] Animator ButtonEndAnim;
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
        BombManager.instance.GetBAM().ButtonClickSFX();
        ButtonStartAnim.SetBool("open", false);
        ButtonEndAnim.SetBool("open", true);
    }

    public void TouchedEdge()
    {
        if (!gameActive) { return; }
        BombManager.instance.GetBAM().FailureSFX();
        Debug.Log("ouch");
        gameActive = false;
        handler.Show(ButtonStart);
        handler.Hide(ButtonEnd);
        ButtonStartAnim.SetBool("open", true);
        ButtonEndAnim.SetBool("open", false);
    }

    public void EndGame()
    {
        gameActive = false;
        handler.Hide(ButtonEnd);
        ButtonEndAnim.SetBool("open", false);
        BombManager.instance.GetBAM().ButtonClickSFX();

        base.Solve();
    }
}
