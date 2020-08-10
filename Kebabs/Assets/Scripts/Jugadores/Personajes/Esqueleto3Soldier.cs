using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esqueleto3Soldier : Soldier
{
    public override void Attack()
    {
        GameObject veneno = Instantiate(arma, transform.position, Quaternion.identity);
        veneno.GetComponent<Veneno>().targetTag = opositeTag;

    }
}
