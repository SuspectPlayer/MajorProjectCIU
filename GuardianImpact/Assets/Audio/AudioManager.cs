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
    [SerializeField] AudioSource SFXAudioSource;


    [Header("Music Sounds")]
    [SerializeField] string musicMixerVolume = "MusicVolume";
    [SerializeField] Slider musicSlider;


    AudioClip[] currentMusicArray;
    int lastMusicIndex = -1;
    int attempts = 0;
    bool playMusic = true;
    bool playAmbient = false;

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
            SetSceneMusic(currentMusicArray);
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
    /// <summary>
    /// This needs to be public because it's called from UI buttons etc.
    /// </summary>
    /// <param name="clip">The clip to play</param>
    public void PlaySoundUI(AudioClip clip)
    {
        UIAudioSource.PlayOneShot(clip);
    }

    internal void SetAmbientSounds(AudioClip[] ambientSounds)
    {
        if(ambientSounds != null && ambientSounds.Length > 0)
        {
            GameObject sceneAmbient = new GameObject("SceneAmbientSounds");

            for (int i = 0; i < ambientSounds.Length; i++)
            {
                AudioSource newAudioManager = sceneAmbient.AddComponent(typeof(AudioSource)) as AudioSource;
                newAudioManager.outputAudioMixerGroup = UIAudioSource.outputAudioMixerGroup;
                newAudioManager.playOnAwake = false;
                newAudioManager.loop = true;
                newAudioManager.clip = ambientSounds[i];
                newAudioManager.Play();
            }
        }
    }
    internal void SetPlayOnceSound(AudioClip playOnceOnStart)
    {
        if (playOnceOnStart != null) PlaySoundUI(playOnceOnStart);
    }
    internal void SetSceneMusic(AudioClip[] music)
    {
        currentMusicArray = music;

        if (currentMusicArray != null && currentMusicArray.Length > 0) SetMusic();
        else
        {
            playMusic = false;
            MusicAudioSource.Stop();
        }
    }
    void SetMusic()
    {
        playMusic = true;
        if (MusicAudioSource.isPlaying) MusicAudioSource.Stop();
        int clipIndex = 0;
        if (currentMusicArray.Length > 1)
        {
            clipIndex = Random.Range(0, currentMusicArray.Length);
            // Get a random song to play that is different from the last song played. Try get a new song 50 times.
            attempts++;
            if (currentMusicArray.Length > 1 && (clipIndex == lastMusicIndex && attempts < 50))
            {
                SetMusic();
            }
            else
            {
                attempts = 0;
                lastMusicIndex = clipIndex;

            }
        }
        MusicAudioSource.clip = currentMusicArray[clipIndex];
        MusicAudioSource.Play();
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
    public void PlaySFXOneShot(AudioClip clipToPlay)
    {
        SFXAudioSource.PlayOneShot(clipToPlay);
    }
    #endregion Custom functions
}
