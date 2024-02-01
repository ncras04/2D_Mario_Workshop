using System;
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
    private soundsAndAudioFiles[] m_soundfileReferences;
    private Transform m_parent;

    private Dictionary<ESounds, soundsAndAudioFiles> m_soundDictionary;

    [SerializeField]
    private AudioClip m_bgmClip;
    [SerializeField]
    [Range(0, 100)]
    private float m_bgmVolume;

    private AudioSource m_bgmSource;
    private AudioSource m_soundSource;

    [SerializeField]
    private AudioMixerGroup m_mixer;


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
        m_soundDictionary = new Dictionary<ESounds, soundsAndAudioFiles>();

        foreach (var Sound in m_soundfileReferences)
        {
            m_soundDictionary.Add(Sound.sound, Sound);
        }

        m_bgmSource = gameObject.AddComponent<AudioSource>();
        m_bgmSource.outputAudioMixerGroup = m_mixer;
        m_soundSource = gameObject.AddComponent<AudioSource>();
        m_soundSource.outputAudioMixerGroup = m_mixer;

        StartBGM();

    }

    public void StartBGM()
    {
        m_bgmSource.clip = m_bgmClip;
        m_bgmSource.spatialBlend = 0f;
        m_bgmSource.volume = m_bgmVolume * 0.01f;
        m_bgmSource.loop = true;
        m_bgmSource.Play();
    }
    public void StopBGM()
    {
        m_bgmSource.Stop();
    }

    public void PlaySound(ESounds _sound)
    {
        m_soundSource.volume = m_soundDictionary[_sound].volume * 0.01f;
        m_soundSource.PlayOneShot(m_soundDictionary[_sound].audioClip);
    }

}