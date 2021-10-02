using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager master;
    [SerializeField] AudioSource UIAudioSource;
    [SerializeField] AudioSource MusicAudioSource;

    [SerializeField] int mainMenuIndex = 3;
    [SerializeField] int waitingScene = 4;
    [SerializeField] int gameScene = 5;


    [Header("Music Sounds")]
    [SerializeField] string musicMixerVolume = "MusicVolume";
    [SerializeField] AudioClip[] menuMusic;
    [SerializeField] AudioClip[] waitingMusic;
    [SerializeField] AudioClip[] gameMusic;
    [SerializeField] Slider musicSlider;

    AudioClip[] currentMusicArray;
    int lastMusicIndex = -1;
    int attempts = 0;
    bool playMusic = true;

    [Header("UI Sounds")]
    [SerializeField] string UIMixerVolume = "UIVolume";
    [SerializeField] AudioClip clickStandard;
    [SerializeField] AudioClip message;
    [SerializeField] Slider UISlider;

    [Header("SFX Sounds")]
    [SerializeField] string SFXMixerVolume = "SFXVolume";
    [SerializeField] Slider SFXSlider;

    [Header("Master Sounds")]
    [SerializeField] string masterMixerVolume = "MasterVolume";
    [SerializeField] Slider masterSlider;

    [Header("Other")]
    [SerializeField] AudioMixer audioMixer;

    float messageTimer = 0;
    float cooldownTime = 3;

    #region Monobehavior methods
    private void Awake()
    {
        if (master != null) Destroy(this);
        master = this;

        masterSlider.onValueChanged.AddListener(HandleMasterVolumeSliderChanged);
        musicSlider.onValueChanged.AddListener(HandleMusicVolumeSliderChanged);
        SFXSlider.onValueChanged.AddListener(HandleSFXVolumeSliderChanged);
        UISlider.onValueChanged.AddListener(HandleUIVolumeSliderChanged);
    }
    private void OnDisable()
    {
        // Save the values from the sliders
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefs.SetFloat("UIVolume", UISlider.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSceneMusic();
        // Set the saved values if any exist
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", masterSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", musicSlider.value);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", SFXSlider.value);
        UISlider.value = PlayerPrefs.GetFloat("UIVolume", UISlider.value);
    }
    void FixedUpdate()
    {
        if (playMusic && !MusicAudioSource.isPlaying)
        {
            SetSceneMusic();
        }
        if(messageTimer > 0) messageTimer -= Time.fixedDeltaTime;
    }
    #endregion Monobehavior methods
    #region Custom functions

    public void HandleMasterVolumeSliderChanged(float rawValue)
    {
        audioMixer.SetFloat(masterMixerVolume, Mathf.Log10(rawValue) * 20);
    }
    public void HandleMusicVolumeSliderChanged(float rawValue)
    {
        audioMixer.SetFloat(musicMixerVolume, Mathf.Log10(rawValue) * 20);
    }
    public void HandleSFXVolumeSliderChanged(float rawValue)
    {
        audioMixer.SetFloat(SFXMixerVolume, Mathf.Log10(rawValue) * 20);
    }
    public void HandleUIVolumeSliderChanged(float rawValue)
    {
        audioMixer.SetFloat(UIMixerVolume, Mathf.Log10(rawValue) * 20);
    }

    public void PlaySoundUI(AudioClip clip)
    {
        UIAudioSource.PlayOneShot(clip);
    }
    void SetSceneMusic()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;

        currentMusicArray = null;
        if (activeScene == mainMenuIndex) currentMusicArray = menuMusic;
        else if (activeScene == waitingScene) currentMusicArray = waitingMusic;
        else if (activeScene == gameScene) currentMusicArray = gameMusic;

        if (currentMusicArray != null && currentMusicArray.Length > 0) SetMusic();
        else playMusic = false;
    }
    void SetMusic()
    {
        int clipIndex = Random.Range(0, currentMusicArray.Length);
        // Get a random song to play that is different from the last song played. Try get a new song 50 times.
        attempts++;
        Debug.Log($"Attempts : {attempts}");
        if (currentMusicArray.Length > 1 && (clipIndex == lastMusicIndex && attempts < 50))
        {
            SetMusic();
        }
        else
        {
            attempts = 0;
            lastMusicIndex = clipIndex;

            MusicAudioSource.clip = currentMusicArray[clipIndex];
            MusicAudioSource.Play();
        }

    }
    public void ClickButtonSound()
    {
        UIAudioSource.PlayOneShot(clickStandard);
    }
    public void MessageReceivedSound()
    {
        if (messageTimer > 0) return;
        messageTimer = cooldownTime;
        UIAudioSource.PlayOneShot(message);
    }
    #endregion Custom functions
}
