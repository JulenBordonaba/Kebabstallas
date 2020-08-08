using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulsSoldier : Soldier
{
    
        
    

    public override void Attack()
    {
        print("Attack");
        targetEnemy = null;
        float distance = stats.AttackDistance;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
        {

            if (Vector2.Distance(enemy.transform.position, transform.position) < distance)
            {

                distance = Vector2.Distance(enemy.transform.position, transform.position);
                targetEnemy = enemy;
            }
        }
        if (targetEnemy != null)
        {
            GameObject arrojadiza = Instantiate(arma, transform.position, Quaternion.identity);
            arrojadiza.GetComponent<arrojadiza>().targetTag = opositeTag;
            arrojadiza.GetComponent<arrojadiza>().target = targetEnemy;
        }
    }
}
