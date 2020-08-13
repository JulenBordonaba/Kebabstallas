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

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("GoForConsumable", 0.1f, 1f);
    }

    protected override void Update()
    {
        base.Update();

        if (state == null && CompareTag("Enemy"))
        {
            StateMachineLogic();
        }
    }

    public void GoForConsumable()
    {
        if (!CompareTag("Enemy")) return;
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
