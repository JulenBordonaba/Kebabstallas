using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCalavera : MonoBehaviour
{
    public string[] map;

    private enum Ataque { METEORITO, MAGIA, OJOS, TELETRANSPORTE, NONE };
    private Ataque miAtaque;

    public GameObject Meteorito;
    public GameObject Magia;
    public GameObject Ojo;

    //public float maxVida = 2000f;

    //public float vida = 2000f;

    //public float defensa = 1;

    //private SpriteRenderer sr;

    private Animator anim;

    public string opositeTag = "Player";

    private bool puedeLanzarOjos = false;

    Camera camara;

    GameController GC;

    private float timer;

    private float MagiaTimer;

    public bool HazMagia = false;

    //private Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponent<Animator>();
    }

    private void Awake()
    {
        InvokeRepeating("LanzarOjo", 1f, 4f);
        GC = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (HazMagia)
        {
            MagiaAnim();
        }

        map = FindObjectOfType<GameController>().GetMap();

        switch (miAtaque)
        {
            case Ataque.METEORITO:
                {
                    if (timer > 6f)
                    {
                        StartCoroutine("Meteoritos");
                        timer = 0f;
                        miAtaque = Ataque.NONE;
                        StartCoroutine("ChooseAttack");
                    }
                    break;
                }
            case Ataque.OJOS:
                {
                    if (timer > 20f)
                    {
                        puedeLanzarOjos = false;
                        timer = 0f;
                        miAtaque = Ataque.NONE;
                        StartCoroutine("ChooseAttack");
                    }
                    break;
                }
            case Ataque.MAGIA:
                {
                    if (timer > 5f)
                    {
                        Instantiate(Magia, transform.position, Quaternion.identity);
                        HazMagia = true;
                        AudioManager.PlaySound(AudioManager.Sound.MAGIAAZUL);
                        timer = 0f;
                        miAtaque = Ataque.NONE;
                        StartCoroutine("ChooseAttack");
                    }
                    break;
                }
            case Ataque.TELETRANSPORTE:
                {
                    if (timer > 3f)
                    {
                        int X = 0;
                        int Y = 0;
                        while (map[X][Y] == 'X')
                        {
                            X = Random.Range(1, 19);
                            Y = Random.Range(1, 19);
                        }
                        transform.position = new Vector2(X / 10f, Y / 10f);
                        timer = 0f;
                        miAtaque = Ataque.NONE;
                        StartCoroutine("ChooseAttack");
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void MagiaAnim()
    {
        MagiaTimer += Time.deltaTime;
        if (MagiaTimer < 0.5f)
        {
            float t = MagiaTimer / 0.5f;
            GC.MainCamera.transform.parent.position = Vector3.Lerp(Vector3.one, transform.position + Vector3.up * 0.1f, t);
            GC.MainCamera.GetComponent<Camera>().orthographicSize = 1f - t / 2;
        }
        else if (MagiaTimer <= 2 && MagiaTimer > 1.5f)
        {
            float t = (MagiaTimer - 1.5f) / 0.5f;
            GC.MainCamera.transform.parent.position = Vector3.Lerp(transform.position + Vector3.up * 0.1f, Vector3.one, t);
            GC.MainCamera.GetComponent<Camera>().orthographicSize = 0.5f + t / 2;
        }
        else if (MagiaTimer > 2)
        {
            GC.MainCamera.transform.parent.position = Vector3.one;
            GC.MainCamera.GetComponent<Camera>().orthographicSize = 1;
            MagiaTimer = 0;
            HazMagia = false;
        }
    }

    private IEnumerator ChooseAttack()
    {
        yield return new WaitForSeconds(3f);
        timer = 0;
        float Rand = Random.Range(0, 1f);
        if (Rand < 0.25f)
        {
            miAtaque = Ataque.METEORITO;
            anim.SetInteger("Ataque", 1);
        }
        else if (Rand < 0.5f)
        {
            miAtaque = Ataque.MAGIA;
            
            anim.SetInteger("Ataque", 0);
        }
        else if (Rand < 0.8f)
        {
            miAtaque = Ataque.OJOS;
            anim.SetInteger("Ataque", 2);
            puedeLanzarOjos = true;
        }
        else
        {
            miAtaque = Ataque.TELETRANSPORTE;
            anim.SetInteger("Ataque", 3);
        }
        
    }

    private void LanzarOjo()
    {
        if (puedeLanzarOjos)
            Instantiate(Ojo, transform.position, Quaternion.identity);
    }

    private IEnumerator Meteoritos()
    {
        for (int i = 0; i < 8; i += 1)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject meteorito = Instantiate(Meteorito, new Vector2(Random.Range(3, 17) / 10f, Random.Range(3, 17) / 10f), Quaternion.identity);
            meteorito.GetComponent<Meteorito>().targetTag = opositeTag;
        }
        GC.MainCamera.GetComponent<Animator>().SetTrigger("Meteoritos");

        yield return null;
    }



    /*private void Die()
    {
        Destroy(this.gameObject);
    }


    public void SetHealthBarSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void RecibirDaño(float daño)
    {
        vida -= daño * defensa;
        if (vida < 0)
        {
            vida = 0;
        }
        sr.color = new Color32(255, 83, 83, 255);
        SetHealthBarSize(vida / maxVida);
        StartCoroutine(RecuperarColor(0.2f));
    }
    private IEnumerator RecuperarColor(float segs)
    {
        yield return new WaitForSeconds(segs);
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }*/

}
