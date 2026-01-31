using UnityEngine;

public class Trivia : Puzzle
{
    private int currentQuestion = 0;
    [SerializeField] int[] correctAnswers;
    [SerializeField] GameObject[] questions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentQuestion = 0;
        questions[0].SetActive(true);
        for(int i = 1; i < questions.Length; i++)
        {
            questions[i].SetActive(false);
        }
    }

    public void Answer(int ans)
    {
        questions[currentQuestion].SetActive(false);
        if (correctAnswers[currentQuestion] == ans) 
        { 
            currentQuestion++;
        }
        else
        {
            currentQuestion = 0;
        }

        if(currentQuestion == questions.Length)
        {
            base.Solve();
        }

        questions[currentQuestion].SetActive(true);
    }
}
