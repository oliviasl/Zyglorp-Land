using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager instance { get; private set; }

    [SerializeField] Phase[] phases; // the array holding all the puzzles of each phase
    [SerializeField] int currentPhase = 0;
    [SerializeField] BombRotate rotator;
    [SerializeField] Animator bombAnimator;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPhase = 0;
        UpdateAllFaces();
    }

    //gets called automatiaclly whenever a puzzle is Solved
    //checks to see if all puzzles in the phase are solved and, if so, progresses phase
    public void CheckProgress()
    {
        bool allSolved = true;
        foreach (Puzzle p in phases[currentPhase].GetPuzzles())
        {
            if (!p.GetSolved())
            {
                allSolved = false;
            }
        }

        if (allSolved)
        {
            ProgressPhase();
        }
    }

    //progresses phase
    void ProgressPhase()
    {
        currentPhase++;
        if (currentPhase == phases.Length)
        {
            //you win
            Debug.Log("You win!");
        }
        else
        {
            bombAnimator.SetInteger("stage", currentPhase);
            // animator is handling this now...
            UpdateAllFaces();
        }

    }

    //sets all faces to disabled, then passes the new set of faces into BombRotate
    public void UpdateAllFaces()
    {
        foreach (Phase p in phases)
        {
            foreach (GameObject f in p.GetFaces())
            {
                ShowHide.instance.Hide(f);
            }
        }

        if (rotator == null) return;
        rotator.UpdateFaces(phases[currentPhase].GetFaces());
    }
}
