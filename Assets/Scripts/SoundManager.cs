using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public  SoundAudioClip[] gameSounds;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //PlayerPrefs.SetInt("SoundMuted",0);
    }

    public  void PlaySound(Sounds sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.Play();
        Destroy(gameObject, audioSource.clip.length);
    }


    public  GameObject PlaySoundLoop(Sounds sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.Play();
        audioSource.loop = true;
        audioSource.volume = 0.1f;
        return gameObject;
    }

    private  AudioClip GetAudioClip(Sounds sound)
    {
        foreach (SoundAudioClip soundAudioClip in gameSounds)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }


    public void Mute()
    {
        AudioListener.volume = 0;
    }

    public void Unmute()
    {
        AudioListener.volume = 1;
    }

    public void MuteOrUnmute()
    {
        int muted = PlayerPrefs.GetInt("SoundMuted");
        if (muted == 1)
        {
            Mute();
        }
        else
        {
            Unmute();
        }
    }

    public  enum Sounds
    {
        Bounce,
        BlockBreak,
        BGM,
        PurchaseSuccess,
        PurchaseFail,
        EndGame
    }

    [Serializable]
    public class SoundAudioClip
    {
        public Sounds sound;
        public AudioClip audioClip;
    }
}




