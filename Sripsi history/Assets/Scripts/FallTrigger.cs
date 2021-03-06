﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(collision.GetComponent<PlayerController>() != null)
                collision.GetComponent<PlayerController>().TakeDamage(10, true);
            if (collision.GetComponent<PlayerRideController>() != null)
                collision.GetComponent<PlayerRideController>().TakeDamage(10, true);
        }
        else
        {
            if(collision.GetComponent<EnemyScript>())
            collision.GetComponent<EnemyScript>().TakeDamage(10);
            if (collision.GetComponent<EnemyGunSoldier>())
            collision.GetComponent<EnemyGunSoldier>().TakeDamage(10);
        }
    }
}
