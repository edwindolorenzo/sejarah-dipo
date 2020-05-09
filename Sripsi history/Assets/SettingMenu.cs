using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider slider;
    float value;

    private void Start()
    {
        float sound = PlayerPrefs.GetFloat("SoundMixer");
        audioMixer.SetFloat("volume", sound);
        slider.value = sound;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("SoundMixer", volume);
    }
}
