using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFollowBoredEnemy : SoldierState
{

    private bool canCheckPath = false;

    public SoldierFollowBoredEnemy(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        base.Start();
        yield return null;

        GameObject[] anemies = GameObject.FindGameObjectsWithTag(soldier.opositeTag);
        if (anemies.Length <= 0)
        {
            ChangeState();
            yield break;
        }
        GameObject currentEnemy = null;
        float currentFollowValue = float.MinValue;
        foreach (GameObject enemy in anemies)
        {


            if (enemy.GetComponent<Soldier>() != null)
            {
                Soldier s = enemy.GetComponent<Soldier>();

                if(!s.riendo)
                {
                    float lostHealth = ((100 - s.stats.HealthPercentaje));

                    float enemyFollowValue = lostHealth - (Vector2.Distance(s.transform.position, soldier.transform.position) * 30);

                    if (enemyFollowValue > currentFollowValue)
                    {
                        currentFollowValue = enemyFollowValue;
                        currentEnemy = s.gameObject;
                    }
                }


                
            }

        }

        soldier.StartCoroutine(ResetCanCheckPath());

        soldier.followTarget = currentEnemy;

        GameController.OnCollectablePlaced.AddListener(ChangeState);
        Collectable.OnCollectableCollected.AddListener(ChangeState);
        Soldier.OnDamageDealed.AddListener(ChangeState);
        

    }

    public override void Update()
    {
        base.Update();
        if (soldier.followTarget == null && canCheckPath)
        {
            ChangeState();
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
