using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    private int level;
    private List<Chalange> chalanges;
    private bool clear;
    private bool chalangesClear;
    //private float miniGame1;
    //private float miniGame2;
    public int Level { get; set; }
    public List<Chalange> Chalanges { get; set; }
    public bool Clear { get; set; }
    public bool ChalangeClear { get; set; }

    public Stage(int level, List<Chalange> chalanges, bool clear = false, bool chalangeClear = false)
    {
        Level = level;
        Chalanges = chalanges;
        Clear = clear;
        ChalangeClear = chalangeClear;
    }
}
