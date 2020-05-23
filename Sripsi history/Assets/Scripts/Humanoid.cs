using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid
{
    private float health;
    private float maxHealth = 3f;
    private float attack = 1f;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value <= MaxHealth)
                health = value;
            else
                health = MaxHealth;
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public float Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
        }
    }

    public Humanoid()
    {
        health = maxHealth;
    }

    //public Humanoid(float max)
    //{

    //}
}
