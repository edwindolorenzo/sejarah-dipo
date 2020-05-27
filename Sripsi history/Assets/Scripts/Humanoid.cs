using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : PhysicsObject
{
    private float health;
    private float maxHealth = 3f;
    private float attackStreght = 1f;

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

    public float AttackStreght
    {
        get
        {
            return attackStreght;
        }
        set
        {
            attackStreght = value;
        }
    }

    //public Humanoid()
    //{
    //    health = maxHealth;
    //}

    //public Humanoid(float max)
    //{

    //}
}
