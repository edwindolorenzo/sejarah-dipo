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

    private void Start()
    {
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
