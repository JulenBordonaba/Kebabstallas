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

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("GoForConsumable", 0.1f, 1f);
    }

    protected override void Update()
    {
        base.Update();

        if (state == null && IA)
        {
            StateMachineLogic();
        }
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
                SetState(new SoldierFollowWeakestEnemy(this));
            }
        }
        else
        {
            SetState(new SoldierFollowWeakestEnemy(this));
        }
    }


    private void OnDestroy()
    {
        if (state != null)
            state.UnsubscribeFromEvents();
    }
}
