using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFollowWeakestTeammate : SoldierState
{

    private bool canCheckPath = false;

    public SoldierFollowWeakestTeammate(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        yield return null;

        GameObject[] allies = GameObject.FindGameObjectsWithTag(soldier.tag);
        if (allies.Length <= 0)
        {
            Debug.Log("Sin aliados");
            ChangeState();
            yield break;
        }
        GameObject currentHealAlly = allies[0];
        float currentHealHeuristic = float.MinValue;
        foreach (GameObject ally in allies)
        {


            if (ally.GetComponent<Soldier>() != null)
            {
                Soldier s = ally.GetComponent<Soldier>();

                if (s == soldier)
                {
                    continue;
                }

                Location initial = new Location
                {
                    X = Mathf.RoundToInt(soldier.transform.position.x * 10) + soldier.border,
                    Y = Mathf.RoundToInt(soldier.transform.position.y * 10) + soldier.border,
                };

                Location target = new Location
                {
                    X = Mathf.Clamp(Mathf.RoundToInt(s.transform.position.x * 10), 1, 19) + soldier.border,
                    Y = Mathf.Clamp(Mathf.RoundToInt(s.transform.position.y * 10), 1, 19) + soldier.border,
                };

                float lostHealth = ((100 - s.stats.HealthPercentaje));

                float allyHealHeuristic = s.riendo ? -1000 : (lostHealth == 0 ? -100 : (lostHealth - (soldier.A_estrella_Coste(initial, target).Count * 3)));

                if (allyHealHeuristic > currentHealHeuristic)
                {
                    currentHealHeuristic = allyHealHeuristic;
                    currentHealAlly = s.gameObject;
                }
            }

        }

        soldier.StartCoroutine(ResetCanCheckPath());

        soldier.followTarget = currentHealAlly;

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

