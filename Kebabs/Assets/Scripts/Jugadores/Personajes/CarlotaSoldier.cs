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

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("GoForConsumable", 0.1f, 1f);
    }

    protected override void Update()
    {
        base.Update();
    }
    

    public void GoForConsumable()
    {
        if (!IA) return;
        if (BoredEnemies)
        {
            SetState(new SoldierFollowBoredEnemy(this));
        }
        else if (CheckNearConsumables(0.8f))
        {
            SetState(new SoldierFindConsumables(this));
        }
    }

    public override void StateMachineLogic()
    {
        if (!IA) return;
        if (stats.HealthPercentaje < 35)
        {
            SetState(new SoldierHuir(this));
        }
        else if (stats.HealthPercentaje < 50)
        {
            if (CheckNearConsumables(1.2f))
            {
                SetState(new SoldierFindConsumables(this));
            }
            else if(BoredEnemies)
            {
                SetState(new SoldierFollowBoredEnemy(this));
            }
            else
            {
                SetState(new SoldierHuir(this));
            }
        }
        else
        {
            if (BoredEnemies)
            {
                SetState(new SoldierFollowBoredEnemy(this));
            }
            else
            {
                SetState(new SoldierHuir(this));
            }
        }
    }

    public bool BoredEnemies
    {
        get
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(opositeTag);
            if (enemies == null) return false;
            if (enemies.Length <= 0) return false;
            foreach(GameObject enemy in enemies)
            {
                if(enemy.GetComponent<Soldier>())
                {
                    Soldier sold = enemy.GetComponent<Soldier>();
                    if(!sold.riendo)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }


    private void OnDestroy()
    {
        if (state != null)
            state.UnsubscribeFromEvents();
    }
}
