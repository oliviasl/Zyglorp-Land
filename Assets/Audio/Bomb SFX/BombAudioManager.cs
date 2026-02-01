using UnityEngine;

public class BombAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource[] audios;
    public void NextPhaseSFX()
    {
        if (!audios[0].isPlaying)
        {
            audios[0].Play();
        }
    }
    public void CutWireSFX()
    {
        if (!audios[1].isPlaying)
        {
            audios[1].Play();
        }
    }
    public void FailureSFX()
    {
        if (!audios[2].isPlaying)
        { 
            audios[2].Play();
        }
    }
    public void DingSFX()
    {
        if (!audios[3].isPlaying)
        {
            audios[3].Play();
        }
    }
    public void SuccessSFX()
    {
        if (!audios[4].isPlaying)
        {
            audios[4].Play();
        }
    }
    public void ButtonClickSFX()
    {
        if (!audios[5].isPlaying)
        {
            audios[5].Play();
        }
    }
    public void BeepSFX()
    {
        if (!audios[6].isPlaying)
        {
            audios[6].Play();
        }
    }
    public void AirHornSFX()
    {
        if (!audios[7].isPlaying)
        {
            audios[7].Play();
        }
    }
    public void AlienFleeSFX()
    {
        if (!audios[8].isPlaying)
        {
            audios[8].Play();
        }
    }
    public void GlorpSFX()
    {
        if (!audios[9].isPlaying)
        {
            audios[9].Play();
        }
    }
    public void FlorpSFX()
    {
        if (!audios[10].isPlaying)
        {
            audios[10].Play();
        }
    }
    public void ZorpSFX()
    {
        if (!audios[11].isPlaying)
        {
            audios[11].Play();
        }
    }
    public void GlorpItSFX()
    {
        if (!audios[12].isPlaying)
        {
            audios[12].Play();
        }
    }
    public void FlorpItSFX()
    {
        if (!audios[13].isPlaying)
        {
            audios[13].Play();
        }
    }
    public void ZorpItSFX()
    {
        if (!audios[14].isPlaying)
        {
            audios[14].Play();
        }
    }
}
