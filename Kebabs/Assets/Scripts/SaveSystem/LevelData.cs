using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {

    public int[] levels = new int[50];

    public LevelData(int[] levs)
    {
        for (int i = 0;  i < levs.Length; i++)
        {
            levels[i] = levs[i];
        }
    }

    
}

