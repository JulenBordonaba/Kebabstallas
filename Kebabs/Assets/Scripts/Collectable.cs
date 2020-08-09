using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private string TargetTag = null;
    private bool exploting = false;
    public EffectData myEffect;
    public GameObject Oso;
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

    private IEnumerator SubeAtaque()
    {
        exploting = true;
        this.GetComponent<Animator>().SetBool("Recolectada", true);
        yield return new WaitForSeconds(0.95f);
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
                            //StartCoroutine(other.GetComponent<Soldier>().OriginalColorChange( Color.yellow));
                            other.GetComponent<EffectManager>().StartEffect(myEffect.id);
                            Destroy(this.gameObject);
                            break;
                        }
                    case Type.RALENTIZADOR:
                        {
                            TargetTag = other.GetComponent<Soldier>().opositeTag;
                            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(TargetTag))
                            {
                                if (enemy.GetComponent<EffectManager>() != null)
                                    enemy.GetComponent<EffectManager>().StartEffect(myEffect.id);
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
                            GameObject Osito = Instantiate(Oso, transform.position, Quaternion.identity);
                            Osito.GetComponent<Oso>().opositeTag = other.GetComponent<Soldier>().opositeTag;
                            Destroy(this.gameObject);
                            break;
                        }
                    case Type.PETRIFICACION:
                        {
                            other.GetComponent<EffectManager>().StartEffect(myEffect.id); 
                            //object[] parms = new object[2] { myEffect.duration, other.GetComponent<EffectManager>().EffectColor };
                            //other.GetComponent<Soldier>().StartCoroutine("OriginalColorChange", parms);
                            Destroy(this.gameObject);
                            break;

                        }
                    case Type.ATAQUE:
                        {
                            transform.parent = other.transform;
                            transform.position = transform.parent.position;
                            other.GetComponent<EffectManager>().StartEffect(myEffect.id);
                            StartCoroutine("SubeAtaque");
                            
                            break;
                        }
                }
            }
        }
        else
        {
            if (other.tag == TargetTag && exploting)
            {
                if (other != null && other.GetComponent<Soldier>() != null)
                    other.GetComponent<Soldier>().GetDamage(40);
            }
        }
    }
}
