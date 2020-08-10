using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZorroSoldier : Soldier
{
    private bool atacando = false;
    public GameObject misLlamas;


    protected override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        StartCoroutine(Llamas());
    }

    protected IEnumerator Llamas()
    {
        misLlamas.SetActive(true);
        misLlamas.transform.Find("Llama").GetComponent<Llama>().targetTag = opositeTag;
        misLlamas.transform.Find("Llama1").GetComponent<Llama>().targetTag = opositeTag;
        misLlamas.transform.Find("Llama2").GetComponent<Llama>().targetTag = opositeTag;
        atacando = true;
        yield return new WaitForSeconds(35);
        misLlamas.SetActive(false);
        atacando = false;
    }

}
