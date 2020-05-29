using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamePlay : MonoBehaviour
{
    public Text scoreText;
    public float pointToScore = 5f;
    float score = 0;
    bool start = false;

    public float dificultIncrase = 10f;
    float incraseCounter = 0;

    public Text countDownPlay;

    public GameObject gamePlayUI;
    public GameObject spawnManager;
    CountDownGame countDownGame;

    AudioManager audioManager = AudioManager.instance;

    void Start()
    {
        StartCoroutine(startPlay());
        gamePlayUI.SetActive(false);
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        countDownGame = spawnManager.GetComponent<CountDownGame>();
        audioManager.Play("GameMusic", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(incraseCounter > 0)
        {
            incraseCounter -= Time.deltaTime;
        }
        if (start)
        {
            score += pointToScore * Time.deltaTime;
            if(score%dificultIncrase < 0.1f && incraseCounter <=0)
            {
                incraseCounter = 10;
                countDownGame.AddSpawnerNumber();
            }
            scoreText.text = score.ToString("0");

        }
    }

    public float GetScore()
    {
        start = false;
        countDownGame.SpawnInactive();
        return score;
    }

    IEnumerator startPlay()
    {
        countDownPlay.gameObject.SetActive(true);
        countDownPlay.text = "Bersiap - siap";
        yield return new WaitForSeconds(2f);
        countDownPlay.text = "Mulai";
        yield return new WaitForSeconds(1f);
        countDownPlay.gameObject.SetActive(false);
        spawnManager.SetActive(true);
        gamePlayUI.SetActive(true);
        start = true;
        yield return null;
    }
}
