using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulsSoldier : Soldier
{
    


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


    protected override void Start()
    {
        base.Start();
        InvokeRepeating("GoForConsumable", 0.1f, 1f);
    }

    protected override void Update()
    {
        base.Update();

    }

    public void GoForConsumable()
    {
        if (!IA) return;
        if (CheckNearConsumables(0.8f))
        {
            SetState(new SoldierFindConsumables(this));
        }
    }

    public override void StateMachineLogic()
    {
        if (stats.HealthPercentaje < 35)
        {
            SetState(new SoldierHuir(this));
        }
        else if (stats.HealthPercentaje < 50)
        {
            if (CheckNearConsumables(1.2f))
            {
                SetState(new SoldierFindConsumables(this));
            }
            else
            {
                SetState(new SoldierFollowAtDistanceEuclidea(this));
            }
        }
        else
        {
            SetState(new SoldierFollowAtDistanceEuclidea(this));
        }
    }


    private void OnDestroy()
    {
        if (state != null)
            state.UnsubscribeFromEvents();
    }
}
