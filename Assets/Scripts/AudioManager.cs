using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private Sound[] sounds = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }
    }

    private Sound FindSound(string sound) {
        return Array.Find(sounds, item => item.name == sound);
    }

    public void PlayRandom(params string[] sounds)
    {
        Play(sounds[UnityEngine.Random.Range(0, sounds.Length)]);
    }

    public void Play(string sound)
    {
        Play(FindSound(sound));
    }

    private void Play(Sound s)
    {
        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void PlayLoop(string sound)
    {
        Sound s = FindSound(sound);
        if (s.source.isPlaying)
        {
            return;
        }

        s.loop = true;
        Play(s);
    }

    public void StopLoop(string sound)
    {
        Sound s = FindSound(sound);
        if (s.source.isPlaying)
        {
            s.source.Pause();
        }
    }

    public void PlayMusic(string sound)
    {

    }
}
