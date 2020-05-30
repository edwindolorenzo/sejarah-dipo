using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunnerMiniGamePlay : MonoBehaviour
{
    public Text scoreText;

    public Transform playerRidingHorse;

    private float scoreCount;

    public Text getReadyText;

    public GameObject gamePlayUI;
    public bool scoreIncreasing;

    AudioManager audioManager = AudioManager.instance;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = playerRidingHorse.position;
        scoreCount = 0;
        StartCoroutine(startGame());
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("GameMusic", true);
        gamePlayUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreIncreasing)
        {
            scoreCount = Vector3.Distance(playerRidingHorse.position, transform.position);
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
