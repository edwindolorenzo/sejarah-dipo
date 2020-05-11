﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownGame : MonoBehaviour
{
    public float timeCountdown = 180f;
    public Text timerText;
    bool playing = false;
    bool ended = false;
    bool respawn = false;
    public GameObject[] respawnObjects;
    public GameObject dialogue, gamePlayUI;
    public Camera cam;
    CameraFollow cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = timeCountdown.ToString("F2");
        cameraFollow = cam.GetComponent<CameraFollow>();
        cameraFollow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {        
        if (gamePlayUI.active)
        {
            playing = true;
            respawn = true;
        }
        if (playing && !ended)
        {
            timeCountdown -= Time.deltaTime;
            timerText.text = timeCountdown.ToString("F2");
            //if(timeCountdown%60 == 0.01f )
            //{
            //    foreach(GameObject respawnObject in respawnObjects)
            //    {
            //        respawnObject.GetComponent<Spawner>().AddSpawn();
            //    }
            //}
        }
        if (respawn && !ended)
        {            
            CallRespawnObjects();
        }
        if(timeCountdown < 0)
        {
            ended = true;
            dialogue.SetActive(true);
            for(int i = 0; i < respawnObjects.Length; i++)
            {
                respawnObjects[i].GetComponent<Spawner>().StopGame();
            }
        }
    }

    void CallRespawnObjects()
    {
        for(int i = 0; i< respawnObjects.Length; i++)
        {
            respawnObjects[i].SetActive(true);
        }
        respawn = false;
    }

}
