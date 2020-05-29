using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMiniGame : MonoBehaviour
{
    public GameObject sceneLoader;
    LevelLoader levelLoader;

    // all minigames
    public Button[] buttons;
    private List<MiniGame> listMiniGames = new List<MiniGame>();

    // mini game selected
    public Sprite[] miniGameImages;
    public Text descriptionText, highscore;
    public Image descImage;
    int gameId;

    // player behind sence
    GameObject playerMenu;

    GameManager gameManager = GameManager.instance;
    AudioManager audioManager = AudioManager.instance;

    // Start is called before the first frame update
    void Start()
    {
        playerMenu = GameObject.Find("MainMenuPlayer");
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("MainMenu", true);
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
            highscore.text = "Nilai Tertinggi : " + miniGame.Highscore.ToString("0");
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
        audioManager.Stop("MainMenu", true);
        levelLoader.LoadSceneName("MiniGame"+gameId);
    }

}
