using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreaSoldier : Soldier
{

    protected override void Update()
    {
        base.Update();
        print("X: " + target.X + " / Y: " + target.Y);
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
        SetState(new SoldierHuir(this));
    }
}
