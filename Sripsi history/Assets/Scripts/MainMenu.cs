using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject sceneLoader;
    LevelLoader levelLoader;
    AudioManager audioManager = AudioManager.instance;

    private void Start()
    {
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("MainMenu", true);
    }
    public void PlayGame(string name)
    {
        levelLoader.LoadSceneName(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
