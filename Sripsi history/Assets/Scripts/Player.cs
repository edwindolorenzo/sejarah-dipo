using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : Humanoid
{
    private int maxLife = 3;
    private int life = 3;

    public int MaxLife
    {
        get
        {
            return maxLife;
        }
        set
        {
            maxLife = value;
        }
    }

    public int Life
    {
        get
        {
            return life;
        }
        set
        {
            if (value <= MaxLife)
            life = value;
        }
    }

    public Player()
    {
        Life = MaxLife;
        Health = MaxHealth;
    }

    public Player(int life)
    {
        Life = life;
        Health = MaxHealth;
    }
}
