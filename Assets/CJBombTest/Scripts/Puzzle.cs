using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    private bool solved = false;
    [SerializeField] BombManager bm;
    [SerializeField] GameObject[] toDisable; //list of all the UI elements that should turn off when you solve this puzzle
    //[SerializeField] Animator animator; //the animator of the thing you want to animate when the puzzle is solved
    //{SerializeField] string animToPlay; //the name of the boolean for the animation you want to play when the puzzle is solved

    void Start()
    {
        bm = BombManager.instance;
    }
    
    public bool GetSolved()
    {
        return solved;
    }

    //call this function when you meet the condition to "solve" this puzzle
    protected void Solve()
    {
        solved = true;
        bm.CheckProgress();
        foreach (GameObject g in toDisable)
        {
            g.SetActive(false);
        }
        //animator.SetBool(animToPlay, true);
    }
}
