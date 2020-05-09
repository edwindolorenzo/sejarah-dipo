using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame
{
    private float highscore;
    private int id;
    private bool opened;

    public float Highscore { get; set; }
    public int Id { get; set; }
    public bool Opened { get; set; }

    public MiniGame(int id, float highscore = 0, bool opened = false)
    {
        Id = id;
        Highscore = highscore;
        Opened = opened;
    }

}
