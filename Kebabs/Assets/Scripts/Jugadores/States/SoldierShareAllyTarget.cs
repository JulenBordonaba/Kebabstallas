using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierShareAllyTarget : SoldierState
{
    private bool canCheckPath = false;

    public SoldierShareAllyTarget(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        base.Start();
        yield return null;

        GameObject[] allies = GameObject.FindGameObjectsWithTag(soldier.tag);
        if (allies.Length <= 0)
        {
            Debug.Log("Sin aliados");
            ChangeState();
            yield break;
        }
        GameObject nearestAlly = null;
        float nearestAllyDist = float.MaxValue;
        foreach (GameObject ally in allies)
        {


            if (ally.GetComponent<Soldier>() != null)
            {
                Soldier s = ally.GetComponent<Soldier>();
                if (s.followTarget != null)
                {
                    if (s == soldier || s.followTarget.tag != soldier.opositeTag)
                    {
                        continue;
                    }
                    else
                    {

                        float allyDist = (Vector2.Distance(s.followTarget.transform.position, soldier.transform.position) * 30);

                        if (allyDist < nearestAllyDist)
                        {
                            nearestAllyDist = allyDist;
                            nearestAlly = s.gameObject;
                        }
                    }

                }

            }

        }

        soldier.StartCoroutine(ResetCanCheckPath());

        soldier.followTarget = nearestAlly;

        GameController.OnCollectablePlaced.AddListener(ChangeState);
        Collectable.OnCollectableCollected.AddListener(ChangeState);
        Soldier.OnDamageDealed.AddListener(ChangeState);
    }

    public override void Update()
    {
        base.Update();
        if (soldier.followTarget != null)
        {
            if ((soldier.followTarget.GetComponent<Soldier>().stats.HealthPercentaje > 90 || soldier.stats.HealthPercentaje < 30) && canCheckPath)
            {
                canCheckPath = false;
                soldier.StartCoroutine(ResetCanCheckPath());
                ChangeState();
            }
        }
        else
        {
            if (canCheckPath)
                soldier.SetState(new SoldierFollowWeakestEnemy(soldier));
        }
    }

    public IEnumerator ResetCanCheckPath()
    {
        yield return new WaitForSeconds(0.5f);
        canCheckPath = true;

    }

    public override void ChangeState()
    {
        base.ChangeState();
        soldier.StateMachineLogic();
    }

    public override void UnsubscribeFromEvents()
    {
        GameController.OnCollectablePlaced.RemoveListener(ChangeState);
        Collectable.OnCollectableCollected.RemoveListener(ChangeState);
        Soldier.OnDamageDealed.RemoveListener(ChangeState);
    }
}
