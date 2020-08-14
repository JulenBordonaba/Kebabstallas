using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlosSoldier : Soldier
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
            StartCoroutine(Rayos());
    }

    private IEnumerator Rayos()
    {
        for (int i = 0; i < 6; i += 1)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject rayo = Instantiate(arma, transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f)), Quaternion.identity);
            Rayo miRayo = rayo.GetComponent<Rayo>();
            miRayo.targetTag = opositeTag;
            miRayo.daño = stats.AttackDamage;
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
