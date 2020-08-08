using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulaSoldier : Soldier
{
    public override void Attack()
    {
        StartCoroutine(Llorar());
    }
}
