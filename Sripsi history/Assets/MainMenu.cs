using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject sceneLoader;
    LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
    }
    public void PlayGame()
    {
        levelLoader.LoadSceneName("SelectLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
