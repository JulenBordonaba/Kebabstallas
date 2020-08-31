using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreaSoldier : Soldier
{


    protected override void Start()
    {
        base.Start();
        InvokeRepeating("GoForConsumable", 0.1f, 1f);
    }

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


        print(state);
        try
        {
            Debug.Log(target.X + " " + target.Y);
        }
        catch
        {
            print("No hay target");
        }
        

    }

    public void GoForConsumable()
    {
        if (!IA) return;
        if (CheckNearConsumables(1.5f))
        {
            SetState(new SoldierFindConsumables(this));
        }
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
        if (!IA) return;
        //print("StateMachineLogic");
        followTarget = null;
        GameObject[] allies = null;
        try
        {
            allies=GameObject.FindGameObjectsWithTag(tag);

        }
        catch
        {
            if (CheckNearConsumables(1.5f))
            {
                SetState(new SoldierFindConsumables(this));
            }
            else 
            {
                SetState(new SoldierHuir(this));
            }
            return;
        }
        if (allies.Length > 0)
        {
            List<GameObject> _allies = new List<GameObject>();

            foreach (GameObject ally in allies)
            {
                if (ally.activeInHierarchy)
                {
                    if (ally.GetComponent<Soldier>())
                    {
                        if (ally.GetComponent<Soldier>() != this)
                        {
                            print(ally.name);
                            _allies.Add(ally);
                        }
                    }
                }
            }

            allies = _allies.ToArray();
        }

        print("Allies: " + allies.Length);

        if (CheckNearConsumables(1.5f))
        {
            SetState(new SoldierFindConsumables(this));
        }
        else if(allies==null)
        {
            SetState(new SoldierHuir(this));
        }
        else if (allies.Length<=1)
        {
            SetState(new SoldierHuir(this));
        }
        else
        {



            if (stats.HealthPercentaje < 25f)
            {
                SetState(new SoldierHuir(this));
            }
            else if (stats.HealthPercentaje < 40f)
            {
                GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
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
                                    if (allyDistance < 1.8f)
                                    {
                                        print("1");
                                        SetState(new SoldierFollowWeakestTeammate(this));
                                    }
                                    else
                                    {
                                        SetState(new SoldierHuir(this));
                                    }
                                }
                            }

                        }
                        else
                        {
                            if(allies.Length>0)
                            {
                                foreach (GameObject ally in allies)
                                {
                                    try
                                    {
                                        float allyDistance = Vector2.Distance(transform.position, collectable.transform.position);
                                        if (allyDistance < 1.8f)
                                        {
                                            print("2");
                                            SetState(new SoldierFollowWeakestTeammate(this));
                                        }
                                        else
                                        {
                                            SetState(new SoldierHuir(this));
                                        }
                                    }
                                    catch
                                    {
                                        SetState(new SoldierHuir(this));
                                    }

                                }
                            }
                            else
                            {
                                SetState(new SoldierHuir(this));
                            }
                            
                        }
                    }
                }
            }
            else
            {
                //SetState(new SoldierFollowWeakestTeammate(this));
                if (AllAlliesHealed)
                {
                    SetState(new SoldierHuir(this));
                    //if (GameObject.FindGameObjectsWithTag("Consumable").Length > 0)
                    //{
                    //    //SetState(new SoldierFindConsumables(this));
                    //}
                    //else
                    //{

                    //}
                }
                else
                {
                    print("3    Allies: " + allies.Length);
                    SetState(new SoldierFollowWeakestTeammate(this));
                }
            }
        }

    }

    

    public bool AllAlliesHealed
    {
        get
        {
            bool allHealed = true;
            GameObject[] allies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject ally in allies)
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

    private void OnDestroy()
    {
        if (state != null)
            state.UnsubscribeFromEvents();
    }
}
