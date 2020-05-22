using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishRunnerMiniGame : MonoBehaviour
{
    public GameObject sceneLoader;
    LevelLoader levelLoader;

    MiniGame miniGame;
    int idMiniGame;

    public GameObject playerObject;
    public GameObject backGroundUI;
    public Text scoreText;
    public Text highscoreText;
    public Text newRecordText;

    GameManager gameManager = GameManager.instance;
    AudioManager audioManager = AudioManager.instance;
    RunnerMiniGamePlay runnerMiniGamePlay;

    // Start is called before the first frame update
    void Start()
    {
        levelLoader = sceneLoader.GetComponent<LevelLoader>();
        idMiniGame = int.Parse(new String(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray()));
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        runnerMiniGamePlay = FindObjectOfType<RunnerMiniGamePlay>();
        miniGame = gameManager.SelectMiniGame(idMiniGame);
    }

    public void GameOver()
    {
        float score = runnerMiniGamePlay.GetScore();
        string _score = score.ToString("0");
        audioManager.Play("WinGame");
        backGroundUI.SetActive(true);
        if (miniGame != null)
        {
            if (miniGame.Highscore > score)
            {
                highscoreText.text = miniGame.Highscore.ToString("0");
            }
            else
            {
                highscoreText.text = _score;
                newRecordText.gameObject.SetActive(true);
                miniGame.Highscore = score;
                gameManager.UpdateMiniGame(miniGame);
            }
            scoreText.text = _score;
        }
        else
            Debug.LogError("Nomor Mini Game tidak ditemukan");
    }

    public void FinishScene()
    {
        levelLoader.LoadSceneName("SelectMiniGame");
    }
}
