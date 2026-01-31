using UnityEngine;

public class Phase : MonoBehaviour
{
    [SerializeField] GameObject[] faces;
    [SerializeField] Puzzle[] puzzles;

    public GameObject[] GetFaces()
    {
        return faces;
    }

    public Puzzle[] GetPuzzles()
    {
        return puzzles;
    }
}
