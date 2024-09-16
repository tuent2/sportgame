using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    [SerializeField] AudioSource audioSource;

    [SerializeField] [Range(0f, 1f)] float CommonValue = 1f;

    [SerializeField] AudioClip[] AudientClip;
    [SerializeField] AudioClip[] KickClip;
    [SerializeField] AudioClip[] GoalClip;
    [SerializeField] AudioClip CatchClip;
    private void Awake()
    {
        Debug.Log("!23");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayAudiemtClip()
    {
        if (Globals.isMusic && AudientClip !=null)
        {
            audioSource.Stop();
            //Random.Range()
            audioSource.clip = AudientClip[Random.Range(0, AudientClip.Length)];
            audioSource.volume = 0.3f;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayKickClip()
    {   
        playClip(KickClip[Random.Range(0, KickClip.Length)]);
    }

    public void PlayGoalClip()
    {
        playClip(GoalClip[Random.Range(0, GoalClip.Length)]);
    }

    public void PlayCatchClip()
    {
        playClip(CatchClip);
    }

    public void playClip(AudioClip clip)
    {   
        if (clip != null  && Globals.isSound)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, CommonValue);
        }
    }


}
