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
        if (CheckNearConsumables(0.3f))
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
            if (CheckNearConsumables(0.5f))
            {
                SetState(new SoldierFindConsumables(this));
            }
            else
            {
                SetState(new SoldierFollowAtDistanceManhatan(this));
            }
        }
        else
        {
            SetState(new SoldierFollowAtDistanceManhatan(this));
        }
    }


    private void OnDestroy()
    {
        if (state != null)
            state.UnsubscribeFromEvents();
    }
}
