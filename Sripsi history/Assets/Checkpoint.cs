﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject respawn;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            respawn.transform.position = transform.position;
            animator.SetTrigger("Raise");
            GetComponent<Checkpoint>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
