using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreaSoldier : Soldier
{

    protected override void Update()
    {
        base.Update();
        //if (target != null)
        //    print("X: " + target.X + " / Y: " + target.Y);
        //else
        //    print("No tengo target");
        //if (followTarget != null)
        //    print(followTarget);
        //else
        //    print("No tengo followTarget");
    }

    public override void Attack()
    {
        bool atacar = false;
        foreach (GameObject friend in GameObject.FindGameObjectsWithTag(tag))
        {
            if (friend != this.gameObject)
                if (friend.GetComponent<Pato>() == null && Vector2.Distance(friend.transform.position, transform.position) < stats.AttackDistance)
                {
                    friend.GetComponent<Soldier>().GetHeal(stats.AttackDamage);
                    atacar = true;
                }
        }
        if (atacar)
            StartCoroutine(Vida());

    }

    public IEnumerator Vida()
    {
        Transform campoReg = transform.Find("CampoReg");
        campoReg.GetComponent<Animator>().SetBool("activo", true);
        yield return new WaitForSeconds(1);
        campoReg.GetComponent<Animator>().SetBool("activo", false);
    }

    public override void StateMachineLogic()
    {
        print("StateMachineLogic");
        if(stats.HealthPercentaje<40f)
        {
            GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
            GameObject[] allies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject consumable in consumables)
            {
                if (consumable.GetComponent<Collectable>())
                {
                    Collectable collectable = consumable.GetComponent<Collectable>();
                    if (collectable.myType == Collectable.Type.VIDA)
                    {
                        Location initial = new Location
                        {
                            X = Mathf.RoundToInt(transform.position.x * 10) + border,
                            Y = Mathf.RoundToInt(transform.position.y * 10) + border,
                        };

                        Location target = new Location
                        {
                            X = Mathf.Clamp(Mathf.RoundToInt(collectable.transform.position.x * 10), 1, 19) + border,
                            Y = Mathf.Clamp(Mathf.RoundToInt(collectable.transform.position.y * 10), 1, 19) + border,
                        };

                        float healDistance = A_estrella_Coste(initial, target).Count;
                        if (healDistance<10)
                        {
                            SetState(new SoldierFindHealConsumable(this));
                        }
                        else
                        {
                            foreach (GameObject ally in allies)
                            {
                                target = new Location
                                {
                                    X = Mathf.Clamp(Mathf.RoundToInt(collectable.transform.position.x * 10), 1, 19) + border,
                                    Y = Mathf.Clamp(Mathf.RoundToInt(collectable.transform.position.y * 10), 1, 19) + border,
                                };

                                float allyDistance = A_estrella_Coste(initial, target).Count;
                                if (allyDistance < 10)
                                {
                                    SetState(new SoldierFollowWeakestTeammate(this));
                                }
                                else
                                {
                                    SetState(new SoldierHuir(this));
                                }
                            }
                        }

                    }
                }
            }
        }
        else
        {
            SetState(new SoldierFollowWeakestTeammate(this));
        }
        
    }
}
