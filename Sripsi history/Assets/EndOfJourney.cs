using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfJourney : MonoBehaviour
{
    public GameObject[] gameobjects;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerRideController>().MakePlayerNotMoving();
            for (int i = 0; i < gameobjects.Length; i++)
            {
                gameobjects[i].SetActive(true);
            }
        }
    }
}
