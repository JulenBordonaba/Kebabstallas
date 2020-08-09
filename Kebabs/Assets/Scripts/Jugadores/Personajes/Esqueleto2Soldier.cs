using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esqueleto2Soldier : Soldier
{
    public override void Attack()
    {
        StartCoroutine(Huesos());
    }

    private IEnumerator Huesos()
    {
        for (int i = 0; i < 10; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject Hueso = Instantiate(arma, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Hueso miHueso = Hueso.GetComponent<Hueso>();
            miHueso.targetTag = opositeTag;
            miHueso.daño = stats.AttackDamage;
        }
        yield return null;
    }
}
