using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.output;
        }
    }

    public void Play (string name, bool animated = false)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + "not found");
            return;
        }
        if (animated)
        {
            StartCoroutine(FadeIn(s, 10f));
        }
        else
            s.source.Play();
    }

    IEnumerator FadeIn(Sound s , float fadeIn)
    {
        float maxSound = s.source.volume;
        s.source.volume = 0;
        s.source.Play();
        while (s.source.volume < 1)
        {
            s.source.volume += Time.deltaTime / 1000f;
            Debug.Log(s.source.volume);
        }
        yield return null;
    }

}
