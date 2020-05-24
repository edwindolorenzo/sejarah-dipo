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

    public StateEnemy State { get; set; }


    public Enemy()
    {
        Health = MaxHealth;
        State = StateEnemy.Patrol;
    }

    public Enemy(float health)
    {
        Health = health;
        State = StateEnemy.Patrol;
    }

}
