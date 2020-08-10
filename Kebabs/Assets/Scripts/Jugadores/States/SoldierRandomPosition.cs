using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierRandomPosition : SoldierState
{



    public SoldierRandomPosition(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        yield return null;
    }

    public override void Update()
    {
        base.Update();

        //redondear a float con un decimal
        float posRoundX = soldier.RoundWithDecimals(soldier.transform.position.x, 1);
        float posRoundY = soldier.RoundWithDecimals(soldier.transform.position.y, 1);

        if (soldier.target != null && Mathf.Abs(soldier.transform.position.x - posRoundX) < 0.005f
                           && Mathf.Abs(soldier.transform.position.y - posRoundY) < 0.005f)
        {
            if (Mathf.Abs(soldier.target.X / 10f - posRoundX) < 0.05f && Mathf.Abs(soldier.target.Y / 10f - posRoundY) < 0.05f)
            {
                soldier.direction = Vector2.zero;
                soldier.target = null;
                soldier.StateMachineLogic();
            }
        }


    }
}

