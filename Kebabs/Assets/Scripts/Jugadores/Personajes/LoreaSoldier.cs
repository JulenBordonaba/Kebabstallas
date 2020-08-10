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
        followTarget = null;
        if (stats.HealthPercentaje < 25f)
        {
            SetState(new SoldierHuir(this));
        }
        else if (stats.HealthPercentaje < 40f)
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


                        float healDistance = Vector2.Distance(transform.position, collectable.transform.position);
                        if (healDistance < 0.8f)
                        {
                            SetState(new SoldierFindHealConsumable(this));
                        }
                        else
                        {
                            foreach (GameObject ally in allies)
                            {
                                float allyDistance = Vector2.Distance(transform.position, collectable.transform.position);
                                if (allyDistance < 0.8f)
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
            //if (AllAlliesHealed)
            //{

            //}
            //else
            //{
            //    SetState(new SoldierFollowWeakestTeammate(this));
            //}
        }

    }

    public bool AllAlliesHealed
    {
        get
        {
            bool allHealed = true;
            foreach (GameObject ally in GameObject.FindGameObjectsWithTag(tag))
            {
                if (ally.GetComponent<Soldier>())
                {
                    if (ally.GetComponent<Soldier>().stats.HealthPercentaje < 85)
                    {
                        allHealed = false;
                        break;
                    }
                }
            }
            return allHealed;
        }
    }
}
