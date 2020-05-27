using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Humanoid
{
    public enum StateEnemy
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    private StateEnemy state;
    private Transform player;

    public StateEnemy State { get; set; }
    public Transform Player { get; set; }

    public Enemy()
    {
        Health = MaxHealth;
        State = StateEnemy.Patrol;
    }

    public Enemy(float health)
    {
        Health = health;
        State = StateEnemy.Patrol;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

}
