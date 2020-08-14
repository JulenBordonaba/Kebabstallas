using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esqueleto2Soldier : Soldier
{



    public override void Attack()
    {
        StartCoroutine(Huesos());
    }

    private IEnumerator Huesos()
    {
        for (int i = 0; i < 10; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject Hueso = Instantiate(arma, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Hueso miHueso = Hueso.GetComponent<Hueso>();
            miHueso.targetTag = opositeTag;
            miHueso.daño = stats.AttackDamage;
        }
        yield return null;
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
