using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private Sound[] sounds = null;

    private static Sound currentMusic = null;
    private static Sound targetMusic = null;
    private static Coroutine crossfade = null;

    [SerializeField] private float transitionTime = 2f;

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

    public void PlayMusic(string sound)
    {
        if (currentMusic == null || !currentMusic.source.isPlaying)
        {
            currentMusic = FindSound(sound);
            Play(currentMusic);
        }

        else
        {
            if (targetMusic != null)
            {
                StopCoroutine(crossfade);
                targetMusic.source.Stop();
            }

            targetMusic = FindSound(sound);
            crossfade = StartCoroutine(CrossfadeSound());
        }
    }

    private IEnumerator CrossfadeSound()
    {
        float timer  = 0f;
        float currentVolume = currentMusic.source.volume;

        Play(targetMusic);
        float targetVolume = targetMusic.source.volume;
        targetMusic.source.volume = 0;

        while (timer <= transitionTime)
        {
            float step = (timer += Time.deltaTime) / transitionTime;
            currentMusic.source.volume = Mathf.Lerp(currentVolume, 0, step);
            targetMusic.source.volume = Mathf.Lerp(0, targetVolume, step);
            yield return null;
        }

        currentMusic = targetMusic;
        targetMusic = null;
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
}
