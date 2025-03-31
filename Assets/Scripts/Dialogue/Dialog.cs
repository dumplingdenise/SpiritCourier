using System;
using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.Rendering;*/
using UnityEngine;
[System.Serializable]


public class Dialog
{
    [SerializeField] List<string> lines;

    public List<string> Lines
    {
        get { return lines; }
        set { lines = value; }
    }

    /*[SerializeField] string[][] lines;

    public string[][] Lines
    {
        get { return lines; }
        set { lines = value; }
    }*/
}