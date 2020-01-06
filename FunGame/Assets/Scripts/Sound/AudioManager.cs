using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    //[SerializeField] string menuTheme;
    [Space]
    [SerializeField] SoundClip[] sounds = new SoundClip[0];
    SoundClip currentTrack = null;

    public void Start()
    {
        currentTrack = sounds[0];
        foreach (SoundClip s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string clipToPlay)
    {
        print(" I'm now playing - " + clipToPlay);

        if (currentTrack.isMusic)
        {
            currentTrack.source.Stop();
        }

        SoundClip s = Array.Find(sounds, sounds => sounds.name == clipToPlay);
        currentTrack = s;
        s.source.Play();
    }
}

[Serializable]
public class SoundClip
{
    public string name;
    public bool isMusic;

    public AudioClip clip;

    [HideInInspector] public AudioSource source;

    [Range(0, 1)] public float volume = 1;

    [Range(0, 3)] public float pitch = 1;

}