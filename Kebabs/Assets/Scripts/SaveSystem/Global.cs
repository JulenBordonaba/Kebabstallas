using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Global
{

    #region cosas de otro proyecto que no hacen falta


    public static string[] SeparateNumbersFromString(string _string)
    {
        string numbers = "1234567890";

        if (_string.Length <= 0)
        {
            return new string[1];
        }


        List<string> strings = new List<string>();

        string substring = _string[0].ToString();

        for (int i = 1; i < _string.Length; i++)
        {
            if (numbers.Contains(_string[i].ToString()))
            {
                if (numbers.Contains(_string[i - 1].ToString()))
                {
                    substring += _string[i];
                }
                else
                {
                    string newString = substring;
                    strings.Add(newString);
                    substring = _string[i].ToString();

                }
            }
            else if (!numbers.Contains(_string[i].ToString()))
            {
                if (!numbers.Contains(_string[i - 1].ToString()))
                {
                    substring += _string[i];
                }
                else
                {
                    string newString = substring;
                    strings.Add(newString);
                    substring = _string[i].ToString();


                }
            }
            if (i >= _string.Length - 1)
            {
                strings.Add(substring);
            }
        }

        return strings.ToArray();

    }

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("something bad happened");
    }

    public static T[] InvertArray<T>(T[] array)
    {
        T[] aux = new T[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            aux[i] = array[array.Length - 1 - i];
        }
        return aux;
    }

    public static string LoadResourceTextfile(string path)
    {

        string filePath = "SetupData/" + path.Replace(".json", "");

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
    }

    public static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddSeconds(timestamp);
    }

    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }

    #endregion

    #region SaveLoadData



    public static void SaveData<T>(this T _data, string path)
    {
        string completePath = Path.Combine(Application.persistentDataPath, path);
        string jsonData = JsonUtility.ToJson(_data, true);
        

        File.WriteAllText(completePath, jsonData);
    }

    public static T LoadData<T>(this string path)
    {
        string completePath = Path.Combine(Application.persistentDataPath, path);

        if(!File.Exists(completePath))
        {
            return default(T);
        }

        string jsonData = File.ReadAllText(completePath);

        T data = JsonUtility.FromJson<T>(jsonData);

        return data;

    }

    public static void SaveDataPlayerPrefs<T>(this T _data, string dataKey)
    {
        string jsonData = JsonUtility.ToJson(_data, true);

        PlayerPrefs.SetString(dataKey, jsonData);
        
    }

    public static T LoadDataPlayerPrefs<T>(this string dataKey)
    {
        if (!PlayerPrefs.HasKey(dataKey)) return default(T);

        string _jsonData = PlayerPrefs.GetString(dataKey);
        T _data = JsonUtility.FromJson<T>(_jsonData);
        return _data;

    }


    #endregion
}
