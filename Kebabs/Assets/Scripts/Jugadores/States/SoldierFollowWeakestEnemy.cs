using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFollowWeakestEnemy : SoldierState
{

    private bool canCheckPath = false;

    public SoldierFollowWeakestEnemy(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        yield return null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(soldier.opositeTag);
        if (enemies.Length <= 0)
        {
            //Debug.Log("Sin aliados");
            ChangeState();
            yield break;
        }
        GameObject currentEnemy = enemies[0];
        float currentFollowValue = float.MinValue;
        foreach (GameObject enemy in enemies)
        {


            if (enemy.GetComponent<Soldier>() != null)
            {
                Soldier s = enemy.GetComponent<Soldier>();
                
                

                float lostHealth = ((100 - s.stats.HealthPercentaje));

                float enemyFollowValue = lostHealth - (Vector2.Distance(s.transform.position,soldier.transform.position) * 30);

                if (enemyFollowValue > currentFollowValue)
                {
                    currentFollowValue = enemyFollowValue;
                    currentEnemy = s.gameObject;
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
        if (soldier.followTarget != null)
        {
            if ((soldier.stats.HealthPercentaje < 30) && canCheckPath)
            {
                
                canCheckPath = false;
                soldier.StartCoroutine(ResetCanCheckPath());
                ChangeState();
            }
        }
        else
        {
            if (canCheckPath)
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
