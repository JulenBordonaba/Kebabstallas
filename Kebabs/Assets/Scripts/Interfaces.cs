using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void GetDamage( float daño);
}

public interface IHealeable
{
    void GetHeal(float regeneracion);
}
