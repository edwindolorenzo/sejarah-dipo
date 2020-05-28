﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfJourney : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerRideController>().MakePlayerNotMoving();
        }
    }
}
