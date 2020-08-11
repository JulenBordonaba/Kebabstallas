using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFindConsumables : SoldierState
{
    public SoldierFindConsumables(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        yield return null;

        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
        if(consumables.Length<=0)
        {
            yield break;
        }

        GameObject nearestCollectable = null;
        float nearestDistace = float.MaxValue;

        foreach (GameObject collectable in consumables)
        {
            float dist = Vector2.Distance(soldier.transform.position, collectable.transform.position);
            if (dist < nearestDistace)
            {
                if(collectable.GetComponent<Collectable>())
                {
                    Collectable miCollectable = collectable.GetComponent<Collectable>();
                    if(miCollectable.myType != Collectable.Type.PETRIFICACION)
                    {
                        nearestDistace = dist;
                        nearestCollectable = collectable;
                    }

                }
                
            }
        }

        GameController.OnCollectablePlaced.AddListener(ChangeState);
        Collectable.OnCollectableCollected.AddListener(ChangeState);
        Soldier.OnDamageDealed.AddListener(ChangeState);
        
        soldier.followTarget = nearestCollectable;

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
