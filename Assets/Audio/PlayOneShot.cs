using UnityEngine;

public class PlayOneShot : MonoBehaviour
{
    [SerializeField] private AudioClip clipToPlay;
    

    public void PlayTheOneShot()
    {
        AudioManager.instance.PlayOneShot(clipToPlay);
    }
}
