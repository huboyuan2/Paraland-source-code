using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    private static SoundMgr _instance;
    public AudioSource source;
    public AudioSource StepSource;
    public AudioSource BGMSource;
    public AudioSource DialogSource;
    public float StepFrequest;
    public bool StepSoundLooping;
    public static SoundMgr Instance
    {
        get
        {
            if (_instance == null)
                _instance = (SoundMgr)FindObjectOfType(typeof(SoundMgr));
            return _instance;
        }
    }
    // 偷懒的写法,最好是从AB加载
    public List<AudioClip> AudioList;
    //private void Start()
    //{
    //    source = GetComponent<AudioSource>();
    //}
    public void PlayAudio(int audioid)
    {
        var clip = AudioList[audioid];
        if (clip != null)
        {
            source.clip = clip;

            source.Play();
        }
    }
    public void PlayAudioLoop()
    {

    }
    public void SetBGMVolume(float vol)
    {
        BGMSource.volume = vol;
    }
    public void SetSFXVolume(float vol)
    {
        source.volume = vol;
        StepSource.volume = vol;

    }
    public void PlayStepSound()
    {
        StepSoundLooping = CharacterPathFind.Instance.IsNavigating;
        if (StepSoundLooping)
        //StepAudio.Pause();
        {
            StepSource.Play();
            Invoke("PlayStepSound", StepFrequest);
        }
    }
    public void PlayDialogSound()
    {
        DialogSource.Play();
    }
    public void StopStepSound()
    {
        StepSource.Stop();
    }
}
