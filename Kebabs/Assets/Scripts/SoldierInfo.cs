using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SoldierInfo", menuName = "ScriptableObjects/SoldierInfo", order = 1)]

public class SoldierInfo : ScriptableObject
{
    public GameObject prefab;
    public Vector2 position;
    public string Tag;
    public float vida;
    public float baseSpeed;
    public float baseAttackSpeed;
    public float baseAttackDistance;
    public float baseAttackDamage;
}
