using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioSource oneShot1;
    [SerializeField] private AudioSource oneShot2;
    [SerializeField] private AudioSource oneShot3;
    [SerializeField] private AudioSource oneShot4;
    [SerializeField] private AudioSource oneShot5;

    [SerializeField] private AudioSource bossChaseMusic;
    [SerializeField] private AudioSource bombMusic;
    private void Awake()
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

    public void PlayBossMusic()
    {
        if(bossChaseMusic != null)
        {
            if(bombMusic != null)
            {
                if(bombMusic.isPlaying)
                {
                    bombMusic.Stop();
                    bossChaseMusic.Play();
                }
            }
        }
    }
    
    public void PlayOneShot(AudioClip clipToPlay)
    {
        if(!oneShot1.isPlaying)
        {
            oneShot1.PlayOneShot(clipToPlay);
        }
        else if (!oneShot2.isPlaying)
        {
            oneShot2.PlayOneShot(clipToPlay);
        }
        else if (!oneShot3.isPlaying)
        {
            oneShot3.PlayOneShot(clipToPlay);
        }
        else if (!oneShot4.isPlaying)
        {
            oneShot4.PlayOneShot(clipToPlay);
        }
        else if (!oneShot5.isPlaying)
        {
            oneShot5.PlayOneShot(clipToPlay);
        }


    }
}
