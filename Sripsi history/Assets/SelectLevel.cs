using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{

    public Button[] buttons;
    int levelPassed;
    // Start is called before the first frame update
    void Start()
    {

        PlayerPrefs.SetInt("LevelPassed",0);
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
        for (int i = 0; i <= levelPassed; i++)
        {
            buttons[i].interactable = true;
        }
        for (int i = levelPassed+1; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    public void levelToLoad (int level)
    {
        SceneManager.LoadScene(level);
    }
 
}
