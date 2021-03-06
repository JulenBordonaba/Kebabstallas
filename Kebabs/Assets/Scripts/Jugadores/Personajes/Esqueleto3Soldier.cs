﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esqueleto3Soldier : Soldier
{
    public override void Attack()
    {
        GameObject veneno = Instantiate(arma, transform.position, Quaternion.identity);
        veneno.GetComponent<Veneno>().targetTag = opositeTag;

    }

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("GoForConsumable", 0.05f, 1f);
    }

    protected override void Update()
    {
        base.Update();
        
    }

    public void GoForConsumable()
    {
        if (!IA) return;
        if (CheckNearConsumables(3f))
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
            if (CheckNearConsumables(3f))
            {
                SetState(new SoldierFindConsumables(this));
            }
            else
            {
                SetState(new SoldierRandomPosition(this));
            }
        }
        else
        {
            SetState(new SoldierRandomPosition(this));
        }
    }


    private void OnDestroy()
    {
        if (state != null)
            state.UnsubscribeFromEvents();
    }
}
