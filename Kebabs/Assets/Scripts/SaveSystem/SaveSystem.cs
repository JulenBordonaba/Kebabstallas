
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

	public static void SaveLevels (int[] levels)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levels.progress";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(levels);
        Debug.Log("Saved ");
        formatter.Serialize(stream, data);
        stream.Close();
    }

    

    public static LevelData LoadLevels()
    {
        string path = Application.persistentDataPath + "/levels.progress";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
             FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveRecords(Dictionary<int, int> records)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/records.progress";
        FileStream stream = new FileStream(path, FileMode.Create);

        RecordsData data = new RecordsData(records);
        Debug.Log("Saved ");
        formatter.Serialize(stream, data);
        stream.Close();
    }



    public static RecordsData LoadRecords()
    {
        string path = Application.persistentDataPath + "/records.progress";
        if (File.Exists(path))
        {
            Debug.Log("Loaded ");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            RecordsData data = formatter.Deserialize(stream) as RecordsData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
