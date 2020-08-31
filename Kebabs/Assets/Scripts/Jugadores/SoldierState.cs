using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class SoldierState 
{
    protected Soldier soldier;

    protected bool hasBeenFreezed = false;

    public SoldierState(Soldier _soldier)
    {
        soldier = _soldier;
    }

    public virtual IEnumerator Start()
    {
        yield return null;

        soldier.followTarget = null;
    }

    public virtual void Update()
    {

    }


    public virtual IEnumerator Exit()
    {
        yield break;
    }

    public virtual void ChangeState()
    {
        UnsubscribeFromEvents();
    }

    public virtual void UnsubscribeFromEvents()
    {

    }
}
