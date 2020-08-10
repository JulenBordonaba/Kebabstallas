using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFollowWeakestTeammate : SoldierState
{



    public SoldierFollowWeakestTeammate(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        yield return null;

        GameObject[] allies = GameObject.FindGameObjectsWithTag(soldier.tag);
        if(allies.Length<=0)
        {
            //entrar en otro estado
            yield break;
        }
        GameObject currentHealAlly = allies[0];
        float currentHealHeuristic = float.MinValue;
        foreach (GameObject ally in allies)
        {
            

            if (ally.GetComponent<Soldier>() != null)
            {
                Soldier s = ally.GetComponent<Soldier>();

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

                float allyHealHeuristic = ((100-s.stats.HealthPercentaje)) - (soldier.A_estrella_Coste(initial,target).Count*5);
                if (allyHealHeuristic > currentHealHeuristic)
                {
                    currentHealHeuristic = allyHealHeuristic;
                    currentHealAlly = s.gameObject;
                }
            }
               
        }

        soldier.followTarget = currentHealAlly;

        GameController.OnCollectablePlaced.AddListener(ChangeState);
        Collectable.OnCollectableCollected.AddListener(ChangeState);
    }

    public override void Update()
    {
        base.Update();
        
    }

    public override void ChangeState()
    {
        GameController.OnCollectablePlaced.RemoveListener(ChangeState);
        Collectable.OnCollectableCollected.AddListener(ChangeState);
        soldier.StateMachineLogic();
    }



}

