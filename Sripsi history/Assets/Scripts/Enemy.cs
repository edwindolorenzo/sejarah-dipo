using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Humanoid
{
    public enum State
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public State state;

    public Enemy()
    {
        Health = MaxHealth;
        state = State.Patrol;
    }

    public Enemy(float health)
    {
        Health = health;
        state = State.Patrol;
    }

}
