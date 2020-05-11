using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{

    public GameObject sceneLoader;
    LevelLoader levelLoader;

    public Button[] buttons;
    GameObject playerMenu;
    int levelPassed = 0;
    int pickLevel = 1;
    [SerializeField]Image[] objectiveStars;
    [SerializeField] Sprite fullStars, emptyStars;
    [SerializeField] Text[] objectiveTexts;
    [SerializeField] Text headerLevelText;
    GameManager gameManager = GameManager.instance;
    private List<Stage> stages = new List<Stage>();
    // Start is called before the first frame update
    void Start()
    {
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        stages = gameManager.AllStage();
        playerMenu = GameObject.Find("MainMenuPlayer");
        foreach(Stage stage in stages)
        {
            if (stage.Clear)
                levelPassed = stage.Level;
        }
        for (int i = 0; i <= levelPassed; i++)
        {
            if(buttons.Length > i)
                buttons[i].interactable = true;
        }
        for (int i = levelPassed+1; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    public void PickLevel(int level)
    {
        Stage stage = stages[level-1];
        pickLevel = level;
        if(stage != null)
        {
            headerLevelText.text = "Level " + level;
            int i = 0;
            foreach(Chalange chalange in stage.Chalanges)
            {
                if (chalange.Clear)
                    objectiveStars[i].sprite = fullStars;
                else
                    objectiveStars[i].sprite = emptyStars;
                objectiveTexts[i].text = chalange.NameChalange;
                i += 1;
            }
        }
        else
        {
            Debug.Log("stage tidak ada isinya");
        }
    }

    public void MainMenuLoad()
    {
        levelLoader.LoadSceneName("MainMenu");
    }

    public void levelToLoad ()
    {
        Destroy(playerMenu);
        levelLoader.LoadSceneNumber(pickLevel);
    }
 
}
