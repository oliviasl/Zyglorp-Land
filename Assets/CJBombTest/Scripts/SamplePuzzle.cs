using UnityEngine;

public class SamplePuzzle : Puzzle
{
    [SerializeField] GameObject youSuck;

    void Start()
    {
        youSuck.SetActive(false);
    }

    public void CutWire(bool correct)
    {
        if (correct)
        {
            base.Solve();
        }
        else
        {
            youSuck.SetActive(true);
            //he says that you suck
            Debug.Log("You suck");
        }
    }
}
