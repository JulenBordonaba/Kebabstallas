using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecordsData{

    public Dictionary<int, int> records = new Dictionary<int, int>();

    public RecordsData(Dictionary<int, int> recs)
    {
        foreach (int key in recs.Keys)
        {
            records.Add(key, recs[key]);
        }
    }
}

