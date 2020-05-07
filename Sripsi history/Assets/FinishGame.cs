using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour
{
    public bool finished = false;
    public bool gameOver = false;
    public GameObject [] finishedObjects;
    public GameObject [] gameOverObjects;
    bool damaged = false;
    [SerializeField] Image[] objectiveStars;
    [SerializeField] Sprite fullStars, emptyStars;
    [SerializeField] Text[] objectiveTexts;
    [SerializeField] GameObject playerObject, backGroundUI;
    PlayerController playerController;
    GameManager gameManager = GameManager.instance;
    Player player;
    Stage stage;
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        stage = gameManager.SelectStage(SceneManager.GetActiveScene().buildIndex);
        Debug.Log(playerObject);
        playerController = playerObject.GetComponent<PlayerController>();
    }

    public void GameFinished()
    {
        player = playerController.givePlayerStatus();
        Debug.Log(stage.Level);
        backGroundUI.SetActive(true);
        if (stage != null)
        {
            stage.Clear = true;
            int i = 0;
            int clearing = 0;
            foreach (Chalange chalange in stage.Chalanges)
            {
                chalange.Clear = ClearObjective(chalange.IdChalange);
                Debug.Log(chalange.Clear);
                if (chalange.Clear)
                {
                    objectiveStars[i].sprite = fullStars;
                    clearing += 1;
                }
                else
                    objectiveStars[i].sprite = emptyStars;
                objectiveTexts[i].text = chalange.NameChalange;
                Debug.Log(chalange.NameChalange);
                i += 1;
                if (clearing == 3)
                    stage.ChalangeClear = true;
            }
            gameManager.UpdateStage(stage, stage.Level);
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

    public void PlayerDamaged()
    {
        damaged = true;
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

    public void Finish()
    {
        SceneManager.LoadScene("SelectLevel");
    }
}
