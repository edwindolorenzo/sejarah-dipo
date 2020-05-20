using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunnerMiniGamePlay : MonoBehaviour
{
    public Text scoreText;
    public float pointPerSecond;

    //public Transform startPosition;
    //public Transform playerPosition;

    private float scoreCount;

    public Text getReadyText;

    public GameObject gamePlayUI;
    public bool scoreIncreasing;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startGame());
        gamePlayUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreIncreasing)
        {
            scoreCount += pointPerSecond * Time.deltaTime;
            scoreText.text = scoreCount.ToString("0");
        }
    }

    public float GetScore()
    {
        scoreIncreasing = false;
        return Mathf.Round(scoreCount);
    }

    public IEnumerator startGame()
    {
        getReadyText.gameObject.SetActive(true);
        getReadyText.text = "Bersiap - siap";
        yield return new WaitForSeconds(2f);
        getReadyText.text = "Mulai";
        yield return new WaitForSeconds(1f);
        getReadyText.gameObject.SetActive(false);
        gamePlayUI.SetActive(true);
        GameObject.FindObjectOfType<PlayerRideController>().MakePlayerMove();
        scoreIncreasing = true;
        yield return null;
    }
}
