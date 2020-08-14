using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulaSoldier : Soldier
{
    private bool atacando = false;
    public GameObject CampoLlanto;

    public GameObject lagrimas;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("GoForConsumable", 0.1f, 1f);
    }

    protected override void Update()
    {
        base.Update();


        //if (lastDirection == Vector2.up)
        //{
        //    lagrimas.GetComponent<Animator>().SetInteger("Llorar", 0);
        //    lagrimas.GetComponent<SpriteRenderer>().sortingOrder = 50;
        //}
        //else
        //{
        //    lagrimas.GetComponent<SpriteRenderer>().sortingOrder = 100;
        //    if (lastDirection == Vector2.right)
        //    {
        //        lagrimas.GetComponent<Animator>().SetInteger("Llorar", 1);
        //    }
        //    else if (lastDirection == Vector2.down)
        //    {
        //        lagrimas.GetComponent<Animator>().SetInteger("Llorar", 0);
        //    }
        //    else if (lastDirection == Vector2.left)
        //    {
        //        lagrimas.GetComponent<Animator>().SetInteger("Llorar", 2);
        //    }
        //}



        
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
        if (CheckNearConsumables(0.8f))
        {
            SetState(new SoldierFindConsumables(this));
            return;
        }
        if (atacando)
        {
            if (stats.HealthPercentaje < 25)
            {
                SetState(new SoldierHuir(this));
            }
            else if (stats.HealthPercentaje < 40)
            {
                if (CheckNearConsumables(1.2f))
                {
                    SetState(new SoldierFindConsumables(this));
                }
                else
                {
                    SetState(new SoldierShareAllyTarget(this));
                }
            }
            else
            {
                SetState(new SoldierShareAllyTarget(this));
            }
        }
        else
        {
            SetState(new SoldierHuir(this));
        }
    }

    public override void Attack()
    {
        StartCoroutine(Llorar());
    }

    protected IEnumerator Llorar()
    {
        
        CampoLlanto.SetActive(true);
        CampoLlanto.GetComponent<CampoDebilitador>().targetTag = opositeTag;
        lagrimas.SetActive(true);
        atacando = true;
        if(IA)
        {
            StateMachineLogic();
        }
        yield return new WaitForSeconds(40);
        CampoLlanto.SetActive(false);
        lagrimas.SetActive(false);
        atacando = false;
    }

}
