using UnityEngine;
using TMPro;

public class Trivia : Puzzle
{
    private int currentQuestion = 0;
    [SerializeField] int[] correctAnswers;
    [SerializeField] GameObject[] questions; //any question-specific nonsense you want to enable/disable
    [SerializeField] TriviaQuestion[] questionInfo; //array of TriviaQuestion scriptable objects to hold info
    [SerializeField] TMP_Text[] textHolder; //the bits of text you want to fill with the info from each TriviaQuestion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentQuestion = 0;
        questions[0].SetActive(true);
        for(int i = 1; i < questions.Length; i++)
        {
            questions[i].SetActive(false);
        }
        FillQuestion();
    }

    public void Answer(int ans)
    {
        questions[currentQuestion].SetActive(false);
        if (correctAnswers[currentQuestion] == ans) 
        { 
            currentQuestion++;
            //success sound effect
            BombManager.instance.GetBAM().BeepSFX();
        }
        else
        {
            currentQuestion = 0;
            //fail sound effect
            BombManager.instance.GetBAM().FailureSFX();
        }

        if(currentQuestion == questions.Length)
        {
            base.Solve();
        }
        else
        {
            questions[currentQuestion].SetActive(true);
            FillQuestion();
        }
    }

    void FillQuestion()
    {
        textHolder[0].text = questionInfo[currentQuestion].question;
        for (int i = 1; i < textHolder.Length; i++)
        {
            textHolder[i].text = questionInfo[currentQuestion].answers[i - 1];
        }
    }
}
