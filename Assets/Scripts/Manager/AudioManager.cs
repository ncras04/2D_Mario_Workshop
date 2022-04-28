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


    public static Audio Manager { get; private set; }

    private void Awake()
    {
        if (Manager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Manager = this;

        Init();
    }

    public void Init()
    {

        foreach (var Sound in soundfileReferences)
        {
            soundDictionary.Add(Sound.sound, Sound);
        }

        parent = this.gameObject.transform;

        bgmSource = new AudioSource();

        StartBGM();

    }

    public void StartBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.spatialBlend = 0f;
        bgmSource.volume = bgmVolume * 0.01f;
        bgmSource.loop = true;
    }

    public void PlaySound(ESounds sound)
    {
        GameObject soundObject = new GameObject($"{sound}-Sound");
        soundObject.transform.parent = parent;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = soundDictionary[sound].audioClip;
        audioSource.volume = soundDictionary[sound].volume * 0.01f;
        audioSource.spatialBlend = 0f;
        audioSource.Play();

        Destroy(soundObject, audioSource.clip.length);
    }


}
