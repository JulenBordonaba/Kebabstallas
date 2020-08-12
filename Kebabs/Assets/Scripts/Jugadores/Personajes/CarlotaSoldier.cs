using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlotaSoldier : Soldier
{
    public override void Attack()
    {
        StartCoroutine(Notas());
    }

    private IEnumerator Notas()
    {
        AudioManager.PlaySound(AudioManager.Sound.RISA);
        for (int i = 0; i < 10; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject nota = Instantiate(arma, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Nota miNota = nota.GetComponent<Nota>();
            miNota.targetTag = opositeTag;
            miNota.daño = stats.AttackDamage;
        }
        yield return null;
    }
}
