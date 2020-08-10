using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStay : SoldierState
{
    public SoldierStay(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        yield return null;
    }

    public override void Update()
    {
        base.Update();
    }
}
