using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZorroSoldier : Soldier
{
    private bool atacando = false;
    public GameObject misLlamas;

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

    public override void Attack()
    {
        StartCoroutine(Llamas());
    }

    protected IEnumerator Llamas()
    {
        misLlamas.SetActive(true);
        misLlamas.transform.Find("Llama").GetComponent<Llama>().targetTag = opositeTag;
        misLlamas.transform.Find("Llama1").GetComponent<Llama>().targetTag = opositeTag;
        misLlamas.transform.Find("Llama2").GetComponent<Llama>().targetTag = opositeTag;
        atacando = true;
        StateMachineLogic();
        yield return new WaitForSeconds(35);
        misLlamas.SetActive(false);
        atacando = false;
    }

    public override void StateMachineLogic()
    {
        if (CheckNearConsumables(0.8f))
        {
            SetState(new SoldierFindConsumables(this));
            return;
        }
        if (atacando)
        {
            if(stats.HealthPercentaje<35)
            {
                SetState(new SoldierHuir(this));
            }
            else if(stats.HealthPercentaje<50)
            {
                if(CheckNearConsumables(1.2f))
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
        else
        {
            SetState(new SoldierHuir(this));
        }
    }

    private void OnDestroy()
    {
        if (state != null)
            state.UnsubscribeFromEvents();
    }

}
