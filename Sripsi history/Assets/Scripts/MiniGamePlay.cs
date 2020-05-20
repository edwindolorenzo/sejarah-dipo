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
    public GameObject[] spawnPonts;

    void Start()
    {
        StartCoroutine(startPlay());
        gamePlayUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(incraseCounter > 0)
        {
            incraseCounter -= pointToScore * Time.deltaTime;
        }
        if (start)
        {
            score += pointToScore * Time.deltaTime;
            if(score%dificultIncrase < 0.01f)
            {
                incraseCounter = dificultIncrase;
                foreach (GameObject respawnObject in spawnPonts)
                {
                    respawnObject.GetComponent<Spawner>().AddSpawn();
                }
            }
            scoreText.text = score.ToString("0");

        }
    }

    public float GetScore()
    {
        start = false;
        foreach (GameObject respawnObject in spawnPonts)
        {
            respawnObject.GetComponent<Spawner>().StopGame();
        }
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
        for(int i = 0; i< spawnPonts.Length; i++)
        {
            spawnPonts[i].SetActive(true);
        }
        gamePlayUI.SetActive(true);
        start = true;
        yield return null;
    }
}
