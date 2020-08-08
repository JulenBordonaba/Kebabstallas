using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeyreSoldier : Soldier
{
    public override void Attack()
    {
        GameObject nutella = Instantiate(arma, transform.position, Quaternion.identity);
        nutella.GetComponent<Nutella>().targetTag = opositeTag;
        
    }
}
