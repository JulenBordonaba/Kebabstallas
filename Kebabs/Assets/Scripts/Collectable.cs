using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private string TargetTag = null;
    private bool exploting = false;
    public enum Type
    { EXPLOSION, VIDA, VELOCIDAD, RALENTIZADOR, ESCUDO, MINIOM, CONGELACION, ATAQUE };

    public Type myType;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Explode()
    {
        exploting = true;
        this.GetComponent<Animator>().SetBool("Explota", true);
        yield return new WaitForSeconds(0.34f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TargetTag == null)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                switch (myType)
                {
                    case Type.EXPLOSION:
                        {
                            TargetTag = other.GetComponent<Soldier>().opositeTag;
                            StartCoroutine("Explode");
                            break;
                        }
                    case Type.VIDA:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;
                        }
                    case Type.VELOCIDAD:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;
                        }
                    case Type.RALENTIZADOR:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;
                        }
                    case Type.ESCUDO:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;
                        }
                    case Type.MINIOM:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;
                        }
                    case Type.CONGELACION:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;

                        }
                    case Type.ATAQUE:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;
                        }
                }
            }
        }
        else
        {
            if (other.tag == TargetTag && exploting)
            {
                if (other != null)
                    other.GetComponent<Soldier>().GetDamage(40);
            }
        }
    }
}
