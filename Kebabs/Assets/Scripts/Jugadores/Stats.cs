using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float vida=100;
    public float maxVida;
    public float baseSpeed;
    public float baseAttackSpeed;
    public float baseAttackDistance;
    public float speedMultiplier = 1;
    public float baseAttackDamage = 10;



    public float Speed
    {
        get
        {
            return baseSpeed*speedMultiplier;
        }
    }
    public float AttackSpeed
    {
        get
        {
            return baseAttackSpeed;
        }
    }
    public float AttackDistance
    {
        get
        {
            return baseAttackDistance;
        }
    }
    public float AttackDamage
    {
        get
        {
            return baseAttackDamage;
        }
    }
}
