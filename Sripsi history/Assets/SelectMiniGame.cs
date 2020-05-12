using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMiniGame : MonoBehaviour
{
    public Button[] buttons;
    public Sprite[] miniGameImages;
    public Text descriptionText, highscore;
    public Image descImage;
    public GameObject sceneLoader;

    private List<MiniGame> listMiniGames = new List<MiniGame>();
    GameObject playerMenu;
    GameManager gameManager = GameManager.instance;
    LevelLoader levelLoader;
    int gameId;

    // Start is called before the first frame update
    void Start()
    {
        playerMenu = GameObject.Find("MainMenuPlayer");
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        listMiniGames = gameManager.AllMiniGame();
        int i = 0;
        foreach(MiniGame gameManager in listMiniGames)
        {
            if (gameManager.Opened)
                buttons[i].interactable = true;
            else
                buttons[i].interactable = false;
            i += 1;
        }
    }

    public void PickMiniGame(int number)
    {
        MiniGame miniGame = listMiniGames[number - 1];
        if(miniGame != null)
        {
            gameId = miniGame.Id;
            descriptionText.text = miniGame.Description;
            highscore.text = miniGame.Highscore.ToString();
            descImage.sprite = miniGameImages[miniGame.Id - 1];
        }
    }

    public void MainMenuLoad()
    {
        levelLoader.LoadSceneName("MainMenu");
    }

    public void levelToLoad()
    {
        Destroy(playerMenu);
        levelLoader.LoadSceneName("MiniGame"+gameId);
    }

}
