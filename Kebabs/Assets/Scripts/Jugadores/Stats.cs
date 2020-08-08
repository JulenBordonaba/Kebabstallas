using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public EffectManager effectManager;

    public float vida=100;
    public float maxVida;
    public float baseSpeed;
    public float baseAttackSpeed;
    public float baseAttackDistance;
    public float slow = 0;
    public float baseAttackDamage = 10;
    public float baseDamageReduction = 0;

    public float speedReplace = -1;



    public float Speed
    {
        get
        {
            if (speedReplace < 0)
            {
                return Mathf.Max((baseSpeed + effectManager.Velocity) * ((100f-slow)/100f),0);
            }
            else
            {
                return speedReplace;
            }
        }
    }
    public float AttackSpeed
    {
        get
        {
            return Mathf.Max(baseAttackSpeed-effectManager.AttackSpeed,0.1f);
        }
    }
    public float AttackDistance
    {
        get
        {
            return Mathf.Max(baseAttackDistance + effectManager.AttackDistance,0);
        }
    }
    public float AttackDamage
    {
        get
        {
            return Mathf.Max(baseAttackDamage + effectManager.AttackDamage,0);
        }
    }

    public float DamageReduction
    {
        get
        {
            return Mathf.Clamp(baseDamageReduction + effectManager.DamageReduction,0f,100f);
        }
    }

}
