using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HourglassPuzzle : Puzzle
{
    float HourglassTimer = 10f;
    bool Flippable = true;

    int Count = 3;
    int HourglassCounter = 0;

    [SerializeField] Animator hourglassAnim;

    void Start()
    {
    }

    public void Hourglassed()
    {
        StartCoroutine(Timer());
    }
    public void Flip()
    {
        if (Flippable)
        {
            Hourglassed();
        }
    }

    IEnumerator Timer()
    {
        Debug.Log("Flipped hourglass at timestamp : " + Time.time);
        Flippable = false;
        hourglassAnim.SetBool("flip", true);
        yield return null;
        hourglassAnim.SetBool("flip", false);

        yield return new WaitForSeconds(HourglassTimer);

        
        Flippable = true;
        Debug.Log("Hourglass finished at timestamp : " + Time.time);
        HourglassCounter++;
        if (HourglassCounter == Count)
        {
            base.Solve();
        }
    }
}
