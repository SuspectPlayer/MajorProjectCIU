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

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.master.SetSceneMusic(music);
        AudioManager.master.SetAmbientSounds(ambientSounds);
        AudioManager.master.SetPlayOnceSound(playOnceOnStart);
    }
}
