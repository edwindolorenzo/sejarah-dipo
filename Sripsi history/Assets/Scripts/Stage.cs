using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    private int level;
    private List<Chalange> chalanges;
    private bool clear;
    public int Level { get; set; }
    public List<Chalange> Chalanges { get; set; }
    public bool Clear { get; set; }

    public Stage(int level, List<Chalange> chalanges, bool clear = false)
    {
        Level = level;
        Chalanges = chalanges;
        Clear = clear;
    }
}
