using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour
{
    public GameObject sceneLoader;
    LevelLoader levelLoader;

    public GameObject [] finishedObjects;
    public GameObject [] gameOverObjects;
    bool damaged = false;
    [SerializeField] bool haveMiniGame = false;


    [SerializeField] Image[] objectiveStars;
    [SerializeField] Sprite fullStars, emptyStars;
    [SerializeField] Text[] objectiveTexts;

    [SerializeField] GameObject playerObject, playerRideObject, backGroundUI;
    PlayerController playerController;
    PlayerRideController playerRideController;
    GameManager gameManager = GameManager.instance;
    AudioManager audioManager = AudioManager.instance;

    Player player;
    Stage stage;
    List<MiniGame> listMiniGames = new List<MiniGame>();

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        stage = gameManager.SelectStage(SceneManager.GetActiveScene().buildIndex);
        listMiniGames.AddRange(gameManager.AllMiniGame());
        if (playerObject != null)
            playerController = playerObject.GetComponent<PlayerController>();
        if (playerRideObject != null)
            playerRideController = playerRideObject.GetComponent<PlayerRideController>();
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
    }

    void Update()
    {
        if(playerController != null)
            player = playerController.givePlayerStatus();
        else if (playerRideController != null)
            player = playerRideController.givePlayerStatus();
        
        if (player.Health < 3)
            damaged = true;
    }

    public void GameFinished()
    {
        audioManager.Stop("GameMusic");
        audioManager.Play("WinGame");
        backGroundUI.SetActive(true);
        if (stage != null)
        {
            if (haveMiniGame && !stage.Clear)
            {
                int x = 0;
                foreach(MiniGame miniGame in listMiniGames)
                {
                    if (!miniGame.Opened)
                    {
                        listMiniGames[x].Opened = true;
                        break;
                    }
                    x += 1;
                }
            }

            stage.Clear = true;
            CheckObjective(true);
            gameManager.UpdateStage(stage, stage.Level, listMiniGames);
        }
        else
            Debug.Log("Stage Tidak Ditemukan");
        for (int i = 0; i < finishedObjects.Length; i++)
        {
            finishedObjects[i].SetActive(true);
        }
        for (int i = 0; i < gameOverObjects.Length; i++)
        {
            gameOverObjects[i].SetActive(false);
        }
    }

    public void GameOver()
    {
        audioManager.Stop("GameMusic");
        audioManager.Play("GameOver");
        CheckObjective(false);
        backGroundUI.SetActive(true);
        for (int i = 0; i < finishedObjects.Length; i++)
        {
            finishedObjects[i].SetActive(false);
        }
        for (int i = 0; i < gameOverObjects.Length; i++)
        {
            gameOverObjects[i].SetActive(true);
        }
    }

    bool ClearObjective(int idChalange)
    {
        bool cleared = false;
        switch (idChalange)
        {
            case 1:
                cleared = true;
                break;
            case 2:
                cleared = player.Life == 3 ? true : false;
                break;
            case 3:
                cleared = player.Life >= 2 ? true : false;
                break;
            case 4:
                cleared = damaged ? false : true;
                break;
            case 5:
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
                cleared = enemies.Length == 0 ? true : false;
                break;
            default:
                cleared = false;
                break;
        }
        return cleared;
    }

    private void CheckObjective(bool finish = false)
    {
        int i = 0;
        foreach (Chalange chalange in stage.Chalanges)
        {
            if (!chalange.Clear && finish)
                chalange.Clear = ClearObjective(chalange.IdChalange);
            if (chalange.Clear)
            {
                objectiveStars[i].sprite = fullStars;
            }
            else
                objectiveStars[i].sprite = emptyStars;
            objectiveTexts[i].text = chalange.NameChalange;
            i += 1;
        }
    }

    public void Finish()
    {
        audioManager.Stop("GameMusic",true);
        levelLoader.LoadSceneName("SelectLevel");
    }
}
