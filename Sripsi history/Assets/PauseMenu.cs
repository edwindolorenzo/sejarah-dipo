using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;

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
}
