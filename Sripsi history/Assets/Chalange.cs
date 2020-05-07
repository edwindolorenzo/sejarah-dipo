using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chalange
{
    private int idChalange;
    private bool clear;
    private string nameChalange;

    public int IdChalange
    {
        get
        {
            return idChalange;
        }

        set
        {
            idChalange = value;
        }
    }

    public bool Clear
    {
        get
        {
            return clear;
        }

        set
        {
            clear = value;
        }
    }

    public string NameChalange
    {
        get
        {
            return nameChalange;
        }
        set
        {
            nameChalange = value;
        }
    }

    public Chalange(int idChalange, string nameChalange, bool clear= false)
    {
        IdChalange = idChalange;
        NameChalange = nameChalange;
        Clear = clear;
    }
}
