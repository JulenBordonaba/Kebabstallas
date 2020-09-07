using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "LevelsInfo", menuName = "ScriptableObjects/LevelsInfo", order = 1)]
public class LevelsInfo : ScriptableObject
{
    public AudioClip Music;
    public GameController.MapType Map;
    public List<SoldierInfo> perosnas;
    public int nPildoras;
}
