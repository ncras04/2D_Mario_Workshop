using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum ESounds
{
    JUMP,
    COIN,
    BLOCK,
    KILLENEMY,
    DEATH,
}

[System.Serializable]
public class soundsAndAudioFiles
{
    public ESounds sound;
    public AudioClip audioClip;
    [Range(0, 100)]
    public float volume = 100;
}

public class Audio : MonoBehaviour
{
    [SerializeField]
    private soundsAndAudioFiles[] soundfileReferences;
    private Transform parent;

    private Dictionary<ESounds, soundsAndAudioFiles> soundDictionary;

    [SerializeField]
    private AudioClip bgmClip;
    [SerializeField]
    [Range(0, 100)]
    private float bgmVolume;

    private AudioSource bgmSource;
    private AudioSource soundSource;

    [SerializeField]
    private AudioMixerGroup mixer;


    public static Audio Manager { get; private set; }

    private void Awake()
    {
        if (Manager != null)
        {
            Destroy(gameObject);
            return;
        }
        Manager = this;

        Init();
    }

    public void Init()
    {
        soundDictionary = new Dictionary<ESounds, soundsAndAudioFiles>();

        foreach (var Sound in soundfileReferences)
        {
            soundDictionary.Add(Sound.sound, Sound);
        }

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = mixer;
        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.outputAudioMixerGroup = mixer;

        StartBGM();

    }

    public void StartBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.spatialBlend = 0f;
        bgmSource.volume = bgmVolume * 0.01f;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySound(ESounds sound)
    {
        soundSource.volume = soundDictionary[sound].volume * 0.01f;
        soundSource.PlayOneShot(soundDictionary[sound].audioClip);
    }

}
