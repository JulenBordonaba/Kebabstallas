﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFindHealConsumable : SoldierState
{
    public SoldierFindHealConsumable(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        base.Start();
        yield return null;

        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");


        GameObject nearestHealth = null;
        float currentHealHeuristic = float.MinValue;
        if (consumables.Length > 0)
        {
            foreach (GameObject consumable in consumables)
            {
                if (consumable.GetComponent<Collectable>())
                {
                    Collectable collectable = consumable.GetComponent<Collectable>();
                    if (collectable.myType == Collectable.Type.VIDA)
                    {


                        Location initial = new Location
                        {
                            X = Mathf.RoundToInt(soldier.transform.position.x * 10) + soldier.border,
                            Y = Mathf.RoundToInt(soldier.transform.position.y * 10) + soldier.border,
                        };

                        Location target = new Location
                        {
                            X = Mathf.Clamp(Mathf.RoundToInt(collectable.transform.position.x * 10), 1, 19) + soldier.border,
                            Y = Mathf.Clamp(Mathf.RoundToInt(collectable.transform.position.y * 10), 1, 19) + soldier.border,
                        };

                        float allyHealHeuristic = soldier.A_estrella_Coste(initial, target).Count;
                        if (allyHealHeuristic > currentHealHeuristic)
                        {
                            currentHealHeuristic = allyHealHeuristic;
                            nearestHealth = collectable.gameObject;
                        }
                    }
                }
            }
        }


        GameController.OnCollectablePlaced.AddListener(ChangeState);
        Collectable.OnCollectableCollected.AddListener(ChangeState);
        Soldier.OnDamageDealed.AddListener(ChangeState);

        soldier.followTarget = nearestHealth;


    }

    public override void Update()
    {
        base.Update();
        if (soldier.followTarget == null)
        {
            ChangeState();
        }
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
