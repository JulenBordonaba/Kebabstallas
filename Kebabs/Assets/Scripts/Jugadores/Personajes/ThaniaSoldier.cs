using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThaniaSoldier : Soldier
{
    public override void Attack()
    {
        bool atacar = false;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < stats.AttackDistance)
            {
                atacar = true;
            }
        }
        if (atacar)
        {
            float angulo = -0.4f;
            for (int i = 0; i < 3; i += 1)
            {
                GameObject pollo = Instantiate(arma, transform.position, transform.rotation);
                Pollo miPollo = pollo.GetComponent<Pollo>();
                miPollo.targetTag = opositeTag;
                miPollo.direccion = (lastDirection + Vector2.Perpendicular(lastDirection) * (angulo + angulo * (-i)));
                miPollo.daño = stats.AttackDamage;

            }
        }
    }
}
