using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mainMixer;

    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip buttonClick;
    public AudioClip drawCard;
    public AudioClip dropCard;
    public AudioClip shuffleDeck;
    public AudioClip applause;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void SetMasterVolume(float inVolume)
    {
        mainMixer.SetFloat("MasterVolume",inVolume);
    }

    public void SetMusicVolume(float inVolume)
    {
        mainMixer.SetFloat("MusicVolume", inVolume);
    }

    public void SetSFXVolume(float inVolume)
    {
        mainMixer.SetFloat("SFXVolume", inVolume);
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip inClip)
    {
        SFXSource.PlayOneShot(inClip);
    }


}
