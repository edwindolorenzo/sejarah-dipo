using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    AudioManager audioManager = AudioManager.instance;

    void Start()
    {
        if(audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("MainMenu", true);
    }

}
