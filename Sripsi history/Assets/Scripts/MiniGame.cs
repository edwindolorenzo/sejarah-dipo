using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame
{
    private float highscore;
    private int id;
    private bool opened;
    private string description;

    public float Highscore { get; set; }
    public int Id { get; set; }
    public bool Opened { get; set; }
    public string Description { get; set; }

    public MiniGame(int id, float highscore = 0, bool opened = false)
    {
        Id = id;
        Highscore = highscore;
        Opened = opened;
        Description = desc[id-1];
    }

    public MiniGame(int id, string description, float highscore = 0, bool opened = false)
    {
        Id = id;
        Highscore = highscore;
        Opened = opened;
        Description = description;
    }

    string[] desc = new string[] {
        "Pangeran Diponegoro harus bertahan hidup dari serangan musuh. Semakin lama nilai semakin tinggi.",
        "Pangeran Diponegoro mengejar musuh - musuhnya menggunakan kuda. Semakin lama nilai semakin tinggi."
    };


}
