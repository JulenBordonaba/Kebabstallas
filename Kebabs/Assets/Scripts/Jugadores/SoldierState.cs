using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        yield break;
    }

    public virtual void Update()
    {

    }


    public virtual IEnumerator Exit()
    {
        yield break;
    }
}
