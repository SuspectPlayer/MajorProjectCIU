using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[System.Serializable]
public class SoundClass
{
    public string name;
    public AudioClip clip;
}
public class PlayerAudioEffects : MonoBehaviourPun
{
    BasicBehaviour basicBehavior;
    [SerializeField] SoundClass[] soundsToAdd;

    Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    AudioSource audioSourceBase;

    // Start is called before the first frame update
    void Start()
    {
        basicBehavior = GetComponent<BasicBehaviour>();
        audioSourceBase = GetComponent<AudioSource>();
        foreach(SoundClass SL in soundsToAdd)
        {
            sounds.Add(SL.name, SL.clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine && Input.GetKeyDown(KeyCode.P))
        {
            PlaySoundEffect("TestClip");
        }
    }
    public void PlaySwooshRegular()
    {
        if (!photonView.IsMine) return;
        string[] sounds = { "Swoosh1", "Swoosh2", "Swoosh3" };
        string sound = sounds[Random.Range(0, sounds.Length)];

        PlaySoundEffect(sound);
    }
    public void PlaySwooshPower()
    {
        if (!photonView.IsMine) return;
        string[] sounds = { "Swoosh4", "Swoosh5", "Swoosh6" };
        string sound = sounds[Random.Range(0, sounds.Length)];

        PlaySoundEffect(sound);
    }
    void PlaySoundEffect(string clipName)
    {
        if (!photonView.IsMine) return;
        Debug.Log($"Trying to play sound : {clipName}.");

        AudioClip clipToPlay;
        sounds.TryGetValue(clipName, out clipToPlay);
        if (clipToPlay == null)
        {
            Debug.Log($"Clip to play is null");
            return;
        }
        audioSourceBase.PlayOneShot(clipToPlay);
        photonView.RPC("PlaySoundEffectRPC", RpcTarget.Others, photonView.ViewID, clipName );
    }
    [PunRPC]
    void PlaySoundEffectRPC(int viewID , string clipName)
    {
        AudioClip clipToPlay = null;
        sounds.TryGetValue(clipName, out clipToPlay);
        if (clipToPlay == null) return;

        AudioSource source = null;

        GameObject targetObject = PhotonView.Find(viewID).gameObject;
        if (targetObject == null) return;

        targetObject.TryGetComponent<AudioSource>(out source);
        if (source == null) return;

        source.PlayOneShot(clipToPlay);
    }
}
