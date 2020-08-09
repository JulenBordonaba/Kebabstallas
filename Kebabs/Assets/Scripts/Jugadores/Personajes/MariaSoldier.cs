using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MariaSoldier : Soldier
{
    public override void Attack()
    {
        targetEnemy = null;
        float distance = stats.AttackDistance;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
        {

            if (Vector2.Distance(enemy.transform.position, transform.position) < distance && enemy.GetComponent<Soldier>() != null)
            {

                distance = Vector2.Distance(enemy.transform.position, transform.position);
                targetEnemy = enemy;
            }
        }
        if (targetEnemy != null)
        {
            GameObject bomba = Instantiate(arma, transform.position, Quaternion.identity);
            BombaCorazon miBomba = bomba.GetComponent<BombaCorazon>();
            miBomba.targetPos = targetEnemy.transform.position + targetEnemy.GetComponent<Soldier>().direction * 0.3f;
            miBomba.targetTag = opositeTag;
            miBomba.daño = stats.AttackDamage;
        }
    }
}
