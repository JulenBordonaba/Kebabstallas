using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{

    private string TargetTag = null;
    private bool exploting = false;
    public EffectData myEffect;
    public GameObject Oso;
    private float timer = 0;
    private bool CanSound = true;
    public enum Type
    { EXPLOSION, VIDA, VELOCIDAD, RALENTIZADOR, ESCUDO, MINIOM, PETRIFICACION, ATAQUE };

    public Type myType;

    public static UnityEvent OnCollectableCollected = new UnityEvent();

    // Use this for initialization
    void Start()
    {

    }
    
    void Awake()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 + 1);
        timer += Time.deltaTime;
        
        if (timer > 25f)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Explode()
    {
        if (CanSound)
        {
            CanSound = false;
            AudioManager.PlaySound(AudioManager.Sound.EXPLOSIONAZUL);
        }
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
        if (CanSound)
        {
            CanSound = false;
            AudioManager.PlaySound(AudioManager.Sound.ATAQUEUP);
        }
        this.GetComponent<Animator>().SetBool("Recolectada", true);
        yield return new WaitForSeconds(0.95f);
        Destroy(this.gameObject);
    }

    private IEnumerator Velocidad()
    {
        if (CanSound)
        {
            CanSound = false;
            AudioManager.PlaySound(AudioManager.Sound.VELOCIDAD);
        }
        this.GetComponent<Animator>().SetBool("Recolectada", true);
        yield return new WaitForSeconds(1f);
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
                            transform.parent = other.transform;
                            transform.position = transform.parent.position;
                            StartCoroutine("Velocidad");
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
                            if (other.GetComponent<Soldier>().opositeTag == "Player")
                            {
                                Osito.GetComponent<SpriteRenderer>().color = new Color32(255, 83, 83, 255);
                            }
                            else if(other.GetComponent<Soldier>().opositeTag == "Enemy")
                            {
                                Osito.GetComponent<SpriteRenderer>().color = new Color32(129, 255, 133, 255);
                            }
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

    public Soldier NearestSoldier
    {
        get
        {
            Soldier[] soldiers = GameObject.FindObjectsOfType<Soldier>();

            Soldier nearestSoldier = null;
            float minDist = float.MaxValue;

            foreach (Soldier soldier in soldiers)
            {
                float dist = Vector2.Distance(transform.position, soldier.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestSoldier = soldier;
                }
            }

            return nearestSoldier;
        }
    }
}
