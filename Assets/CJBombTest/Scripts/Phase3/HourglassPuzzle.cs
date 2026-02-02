using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HourglassPuzzle : Puzzle
{
    float HourglassTimer = 10f;
    bool Flippable = true;

    int Count = 3;
    int HourglassCounter = 0;

    [SerializeField] Animator hourglassAnim;
    [SerializeField] TMP_Text hourglassCounter;

    public void Hourglassed()
    {
        StartCoroutine(Timer());
    }
    public void Flip()
    {
        if (Flippable)
        {
            Hourglassed();
            BombManager.instance.GetBAM().ButtonClickSFX();
        }
    }

    IEnumerator Timer()
    {
        Debug.Log("Flipped hourglass at timestamp : " + Time.time);
        Flippable = false;
        hourglassAnim.SetBool("flip", true);
        yield return new WaitForSeconds(HourglassTimer);
        hourglassAnim.SetBool("flip", false);

        BombManager.instance.GetBAM().DingSFX();
        Flippable = true;
        Debug.Log("Hourglass finished at timestamp : " + Time.time);
        HourglassCounter++;
        hourglassCounter.text = (Count - HourglassCounter).ToString();
        if (HourglassCounter == Count)
        {
            base.Solve();
        }
    }
}
