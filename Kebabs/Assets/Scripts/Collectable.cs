using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private string TargetTag = null;
    private bool exploting = false;
    public enum Type
    { EXPLOSION, VIDA, VELOCIDAD, RALENTIZADOR, ESCUDO, MINIOM, PETRIFICACION, ATAQUE };

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

    private IEnumerator PolvoVida()
    {
        exploting = true;
        this.GetComponent<Animator>().SetBool("Recolectada", true);
        yield return new WaitForSeconds(0.45f);
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
                            StartCoroutine("PolvoVida");
                            other.GetComponent<Soldier>().GetHeal(50f);
                            break;
                        }
                    case Type.VELOCIDAD:
                        {
                            StartCoroutine(other.GetComponent<Soldier>().SpeedChange(0.6f, Color.yellow));
                            Destroy(this.gameObject);
                            break;
                        }
                    case Type.RALENTIZADOR:
                        {
                            TargetTag = other.GetComponent<Soldier>().opositeTag;
                            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(TargetTag))
                            {
                                StartCoroutine(enemy.GetComponent<Soldier>().SpeedChange(0.05f, Color.cyan));
                            }
                            Destroy(this.gameObject);
                            break;
                        }
                    case Type.ESCUDO:
                        {
                            other.GetComponent<Soldier>().StartCoroutine("ActivateShield");
                            Destroy(this.gameObject);
                            break;
                        }
                    case Type.MINIOM:
                        {
                            var TargetTag = other.GetComponent<Soldier>().opositeTag;
                            break;
                        }
                    case Type.PETRIFICACION:
                        {
                            Soldier mySoldier = other.GetComponent<Soldier>();
                            StartCoroutine(mySoldier.SpeedChange(1f, Color.gray));
                            //Destroy(this.gameObject);
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
