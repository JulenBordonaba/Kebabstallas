using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscarSoldier : Soldier
{
    public override void Attack()
    {
        if (GameObject.FindGameObjectsWithTag(opositeTag).Length > 0)
        {
            GameObject pato = Instantiate(arma, transform.position, Quaternion.identity);
            Pato miPato = pato.GetComponent<Pato>();
            miPato.opositeTag = opositeTag;
            miPato.daño = stats.AttackDamage;
            pato.tag = this.tag;
        }
    }
}
