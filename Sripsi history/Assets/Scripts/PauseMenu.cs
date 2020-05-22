using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameObject sceneLoader;
    LevelLoader levelLoader;

    AudioManager audioManager = AudioManager.instance;


    private void Start()
    {
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Resume"))
        {
            Resume();
        }
        if (CrossPlatformInputManager.GetButtonDown("Menu"))
        {
            Pause();
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        levelLoader.LoadSceneName(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        audioManager.Stop("GameMusic",true);
        levelLoader.LoadSceneName("MainMenu");
    }
}
