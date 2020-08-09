using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulaSoldier : Soldier
{
    private bool atacando = false;
    public GameObject CampoLlanto;

    public GameObject lagrimas;

    protected override void Update()
    {
        base.Update();

        if (myType == Type.PAULA && atacando)
        {
            if (lastDirection == Vector2.up)
            {
                lagrimas.GetComponent<Animator>().SetInteger("Llorar", 0);
                lagrimas.GetComponent<SpriteRenderer>().sortingOrder = 50;
            }
            else
            {
                lagrimas.GetComponent<SpriteRenderer>().sortingOrder = 100;
                if (lastDirection == Vector2.right)
                {
                    lagrimas.GetComponent<Animator>().SetInteger("Llorar", 1);
                }
                else if (lastDirection == Vector2.down)
                {
                    lagrimas.GetComponent<Animator>().SetInteger("Llorar", 0);
                }
                else if (lastDirection == Vector2.left)
                {
                    lagrimas.GetComponent<Animator>().SetInteger("Llorar", 2);
                }
            }

        }
    }

    public override void Attack()
    {
        StartCoroutine(Llorar());
    }

    protected IEnumerator Llorar()
    {
        CampoLlanto.SetActive(true);
        CampoLlanto.GetComponent<CampoDebilitador>().targetTag = opositeTag;
        lagrimas.SetActive(true);
        atacando = true;
        yield return new WaitForSeconds(40);
        CampoLlanto.SetActive(false);
        lagrimas.SetActive(false);
        atacando = false;
    }

}
