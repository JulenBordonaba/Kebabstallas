using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulsSoldier : Soldier
{

    public EffectData efectoPrueba;

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.L))
        {
            effectManager.StartEffect(efectoPrueba.id);
            StartAttacking();
        }
    }


    public override void Attack()
    {
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
            GameObject arrojadizaObj = Instantiate(arma, transform.position, Quaternion.identity);
            arrojadiza arrojadiza = arrojadizaObj.GetComponent<arrojadiza>();
            arrojadiza.targetTag = opositeTag;
            arrojadiza.target = targetEnemy;
            arrojadiza.daño = stats.AttackDamage;
        }
    }
}
