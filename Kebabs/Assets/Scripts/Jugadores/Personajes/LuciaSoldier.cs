using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuciaSoldier : Soldier
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
            GameObject cartel = Instantiate(arma, transform.position, Quaternion.identity);
            Señal miSeñal = cartel.GetComponent<Señal>();
            miSeñal.targetTag = opositeTag;
            miSeñal.daño = stats.AttackDamage;
            cartel.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, lastDirection));

            cartel.transform.parent = this.transform;
        }
    }
}
