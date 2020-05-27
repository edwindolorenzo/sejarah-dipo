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

    public Chalange(int idChalange, bool clear = false)
    {
        IdChalange = idChalange;
        if(idChalange-1 < orderName.Length)
        {
            NameChalange = orderName[idChalange - 1];
        }
        Clear = clear;

    }

    string[] orderName = new string[] {
        "Menyelesaikan permainan",
        "Menyelesaikan permainan tanpa menggunakan kesempatan hidup",
        "Menggunakan kesempatan hidup hanya satu kali",
        "Mengumpulkan semua keris yang ada di dalam permainan",
        "Mengalahkan semua prajurit"
    };
}
