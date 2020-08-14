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
        if (!IA) return;
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
