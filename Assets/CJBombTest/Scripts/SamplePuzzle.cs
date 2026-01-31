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
            //he says that you suck
            youSuck.SetActive(true);
            //play air horn sound effect or something
        }
    }
}
