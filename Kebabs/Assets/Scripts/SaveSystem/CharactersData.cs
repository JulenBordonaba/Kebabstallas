using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharactersData{

    public int[] characters = new int[16];

    public CharactersData(int[] chars)
    {
        for (int i = 0; i < chars.Length; i++)
        {
            characters[i] = chars[i];
        }
    }
}

