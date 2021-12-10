using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Location 
{
   public int RIndex { get; set; }
   public int CIndex { get; set; }

    public Location(int r, int c) :this()
    {
        this.RIndex = r;
        this.CIndex = c;
    }
}
