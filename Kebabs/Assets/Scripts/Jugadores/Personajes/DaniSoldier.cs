using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaniSoldier : Soldier
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
            GameObject baston = Instantiate(arma, transform.position, Quaternion.Euler(new Vector3(0, 0, 90 + Vector2.SignedAngle(Vector2.up, lastDirection))));
            //GameObject baston01 = baston.transform.Find("baston_0").gameObject;
            Baston miBaston = baston.GetComponent<Baston>();
            Impacto miImpacto = miBaston.imageObject.GetComponent<Impacto>();
            miImpacto.targetTag = opositeTag;
            miImpacto.daño = stats.AttackDamage;
            miBaston.owner = gameObject;


        }
    }
}
