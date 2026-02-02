using UnityEngine;
using System.Collections;
using TMPro;

public class GlorpIt : Puzzle
{
    [SerializeField] float waitTime = 3f; //how long you have to react before the game considers a lack of input a fail
    [SerializeField] int[] answers; //correct answers. Should be 0, 1, or 2
    [SerializeField] AudioClip[] clips; //list of audio clips
    [SerializeField] Animator[] animators; //list of animators
    [SerializeField] TMP_Text tempIndicator; //THIS INDICATOR IS TEMP, DELETE IT ONCE WE GET AUDIO
    [SerializeField] string[] tempGlorps; //THIS SET IS TEMP DELETE IT ONCE WE GET AUDIO
    [SerializeField] AudioSource audioS;
    bool correct = false;
    bool takingAnswers = false;
    int currentClip = 0;
    Coroutine currentCoroutine = null;

    void Start()
    {
        correct = false;
        takingAnswers = false;
        currentClip = 0;
        currentCoroutine = null;
        audioS = gameObject.GetComponent<AudioSource>();
    }

    private void NewClip()
    {
        currentCoroutine = StartCoroutine(ClipWait());
    }

    public void BeginGlorp()
    {
        AudioManager.instance.bombMusic.volume = 0.4f;
        NewClip();
    }

	// called by bomb animaiton manager
    public void Glorp(int ans)
    {
        animators[ans].ResetTrigger("glorping");
        animators[ans].SetTrigger("glorping");
        if(ans == answers[currentClip] && takingAnswers)
        {
            if(ans == 0)
            {
                BombManager.instance.GetBAM().GlorpSFX();
            }
            else if (ans == 1)
            {
                BombManager.instance.GetBAM().FlorpSFX();
            }
            else if (ans == 2)
            {
                BombManager.instance.GetBAM().ZorpSFX();
            }

            correct = true;
            
        }
        else
        {
            Fail();
        }
    }

    IEnumerator ClipWait()
    {
        takingAnswers = true;
        correct = false;
        //TEMP MEASURE UNTIL WE GET SOUND CLIPS:
        //tempIndicator.text = tempGlorps[answers[currentClip]];
        //Debug.Log("Answer Now");

        //play the sound effect from clips[currentClip]
        audioS.clip = clips[answers[currentClip]];
        audioS.Play();
        //while sound effect is playing, yield return null;
        while (audioS.isPlaying)
        {
            yield return null;
        }
       
        yield return new WaitForSeconds(waitTime);
        takingAnswers = false;
        CheckGlorp();
    }

    private void CheckGlorp()
    {
        if (correct)
        {
            currentClip++;
            if (currentClip == answers.Length)
            {
                base.Solve();
                AudioManager.instance.bombMusic.volume = 1f;
            }
            else
            {
                //play the next clip
                NewClip();
            }
        }
        else
        {
            Fail();
        }
    }

    private void Fail()
    {
        Debug.Log("You failed");
        currentClip = 0;
        //play failure noise
        BombManager.instance.GetBAM().FailureSFX();
        StopCoroutine(currentCoroutine);
        AudioManager.instance.bombMusic.volume = 1f;

        //TEMP MEASURE:
        //tempIndicator.text = "START!";
    }
}
