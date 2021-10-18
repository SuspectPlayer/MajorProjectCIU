using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScene : MonoBehaviour
{
    [Tooltip("A list of music to play in this scene. Will get a random clip if more and one audioclipis added. Will play continously. Looping if one clip is added, get's a new random clip (but not repeat the previous one) if multiple clips are added.")]
    [SerializeField] AudioClip[] music;
    [Tooltip("Ambient sounds to loop in the scene.")]
    [SerializeField] AudioClip[] ambientSounds;
    [Tooltip("A clip that will play once when the scene is loaded")]
    [SerializeField] AudioClip playOnceOnStart;

    [Header("List of audioclips to be accessed from scene animations etc. Will be called by its name (string) from the method PlaySFXOneShot.")]
    [SerializeField] List<AudioClip> sceneAudioClips = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.master.SetSceneMusic(music);
        AudioManager.master.SetAmbientSounds(ambientSounds);
        AudioManager.master.SetPlayOnceSound(playOnceOnStart);
    }
    /// <summary>
    /// Call an audioclip from the list sceneAudioClips by its name
    /// </summary>
    /// <param name="name">The name of the clip you want to play</param>
    public void PlaySFXOneShot(string name)
    {
        int index = sceneAudioClips.FindIndex(item => item.name == name);
        if(index != -1)
        {
            // Play the 
            AudioClip clipToPlay = sceneAudioClips[index];
            AudioManager.master.PlaySFXOneShot(clipToPlay);
        }
        else Debug.LogError($"Couldn't find any audioclip named '{name}' in the list called 'sceneAudioClips'.");
    }
}
