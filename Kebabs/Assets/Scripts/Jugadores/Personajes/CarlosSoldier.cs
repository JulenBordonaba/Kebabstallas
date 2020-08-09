using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlosSoldier : Soldier
{
    public override void Attack()
    {
        bool atacar = false;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < stats.AttackDistance)
            {
                atacar = true;
            }
        }
        if (atacar)
            StartCoroutine(Rayos());
    }

    private IEnumerator Rayos()
    {
        for (int i = 0; i < 6; i += 1)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject rayo = Instantiate(arma, transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f)), Quaternion.identity);
            Rayo miRayo = rayo.GetComponent<Rayo>();
            miRayo.targetTag = opositeTag;
            miRayo.daño = stats.AttackDamage;
        }
        yield return null;
    }
}
