using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private DialogManager DM;
    private GameController GC;
    private Soldier huls;

    public GameObject Esqueleto;
    public GameObject Pildora;

    private int DialogIndex = 0;
    private int SecuenceIndex = 0;
    private bool nextAction = true;

    private List<string> Dialogos = new List<string>()
    {
        "Hola, maldito Hedonista.",
        "Bienvenido a Kebatallas, un juego con bastante escasez de sentido, cuyo único fin es la pérdida de tu tiempo en batallas absurdas que no llevan a ninguna parte.",
        "A quien ves en pantalla es a Huls.",
        "Será tu primer personaje en este caótico viaje sin pies ni cabeza.",
        "Trátalo con mucho Hamor y Cariñoh.",
        "Bueno, ¿Quieres jugar ya?",
        "Espero que sí,",
        "porque el juego...",
        "empieza...",
        "aquí.",

        "Empecemos aprendiendo a mover a tu personaje:",
        "Para ello, primero tienes que seleccionarlo pulsándolo una vez.",

        "¡Muy bien! Ahora mismo eres igual de inteligente que una marmota.",
        "Con el personaje seleccionado, pulsa cualquier parte del mapa para que Huls vaya hasta él.",

        "Parece que ya lo tienes dominado más o menos.",
        "Si el tutorial te está suponiendo demasiado esfuerzo, descansa un par de horas y si eso luego ya sigues (o no).",

        "¡Oh! Un enemigo salvaje ha aparecido.",
        "Para atacarle, ponte cerca de él y Huls empezará a lanzar botellas de alcohol contra él.",

        "¿Que por qué lanza botellas de alcohol preguntas?",
        "Eh, pues... porque... porque es una buena medida de desinfección del entorno, sí, eso es.",

        "Ten cuidado, si estás cerca del Esqueleto, él también empezará a atacarte.",
        "Alguna dificultad tenía que tener este juego.",
        "Esquiva sus ataques y acércate con cuidado para atacar y vencerle.",

        "De vez en cuando, aparecerán cristales de poder en el mapa. Lleva a Huls hasta ellos para obtener una habilidad especial temporal.",
        "Este cristal en concreto aumenta la velocidad por un tiempo.",


        "Hay varios tipos de cristales con diferentes poderes que irás viendo más adelante.",
        "Los cristales pueden recogerlos también los enemigos, obteniendo así ellos el poder.",

        "Para perseguir a un enemigo, pulsa encima de él y Huls le perseguirá sin descanso.",

        "Y aquí termina el tutorial, puedes volver a realizarlo si te ha quedado alguna duda.",
        "¡Adiós!",
        ""


    };
    // Start is called before the first frame update
    void Start()
    {
        GC = gameObject.GetComponent<GameController>();
        DM = gameObject.GetComponent<DialogManager>();
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));
        Camera.main.GetComponent<PanZoom>().CanZoom = false;
        GameController.onGame = false;
        huls = FindObjectOfType<Soldier>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (SecuenceIndex)
        {
            case 0:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 11)
                    {
                        SecuenceIndex++;
                    }
                    break;
                }
            case 1:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            Camera.main.GetComponent<PanZoom>().CanZoom = true;
                        }
                    }
                    if (GC.Selected != null && DM.endConversation)
                    {
                        DialogIndex++;
                        SecuenceIndex++;
                        StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                    }

                    break;
                }
            case 2:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 13)
                    {
                        SecuenceIndex++;
                    }
                    break;

                }
            case 3:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            huls.stats.baseSpeed = 0.3f;
                        }
                        if (nextAction)
                        {
                            StartCoroutine(Moves());
                            nextAction = false;
                        }
                    }


                    break;
                }
            case 4:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex == 16)
                    {
                        Esqueleto.SetActive(true);
                    }
                    else if (DialogIndex >= 17)
                    {
                        SecuenceIndex++;
                        nextAction = true;
                    }
                    break;

                }
            case 5:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            huls.stats.baseSpeed = 0.3f;
                            huls.stats.baseAttackDistance = 0.5f;
                        }
                        if (nextAction)
                        {
                            StartCoroutine(Attacks());
                            nextAction = false;
                        }
                    }


                    break;
                }
            case 6:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 19)
                    {
                        SecuenceIndex++;
                        nextAction = true;
                    }
                    break;

                }
            case 7:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            huls.stats.baseSpeed = 0.3f;
                            huls.stats.baseAttackDistance = 0.3f;
                            Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = 0.4f;
                        }
                        if (nextAction)
                        {
                            StartCoroutine(EnemyAttacks());
                            nextAction = false;
                        }
                    }


                    break;
                }
            case 8:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 22)
                    {
                        SecuenceIndex++;
                        nextAction = true;
                    }
                    break;

                }
            case 9:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            huls.stats.baseSpeed = 0.3f;
                            huls.stats.baseAttackDistance = 0.3f;
                            Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = 0.4f;
                            Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0.15f;
                        }
                        if (nextAction)
                        {
                            StartCoroutine(CrystalAppears());
                            nextAction = false;
                        }
                    }


                    break;
                }
            case 10:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 23)
                    {
                        SecuenceIndex++;
                        nextAction = true;
                        Time.timeScale = 1;
                    }
                    break;

                }
            case 11:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            huls.stats.baseSpeed = 0.3f;
                            //huls.stats.baseAttackDistance = 0.3f;
                            //Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = 0.4f;
                            Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0.15f;
                        }
                        if (nextAction)
                        {
                            StartCoroutine(CrystalDissappears());
                            nextAction = false;
                        }
                    }


                    break;
                }
            case 12:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 24)
                    {
                        SecuenceIndex++;
                        nextAction = true;
                    }
                    break;

                }
            case 13:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            huls.stats.baseSpeed = 0.3f;
                            huls.stats.baseAttackDistance = 0.3f;
                            Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = 0.4f;
                            Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0.15f;
                        }
                        if (nextAction)
                        {
                            StartCoroutine(FollowEnemy());
                            nextAction = false;
                        }
                    }

                    break;
                }
            case 14:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 27)
                    {
                        SecuenceIndex++;
                        nextAction = true;
                    }
                    break;

                }
            case 15:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            huls.stats.baseSpeed = 0.3f;
                            huls.stats.baseAttackDistance = 0.3f;
                            Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = 0.4f;
                            Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0.15f;
                        }

                    }
                    if (nextAction)
                    {
                        StartCoroutine(KillEnemy());
                        nextAction = false;
                    }

                    break;
                }
            case 16:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DM.hasEnded)
                        {
                            DM.endConversation = true;
                            DialogIndex++;
                            StartCoroutine(DM.Type(Dialogos[DialogIndex]));
                        }
                    }
                    if (DialogIndex >= 30)
                    {
                        GameManager.LoadScene("MainTitle");
                    }
                    break;
                }
        }
    }

    private IEnumerator Moves()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitUntil(() => huls.target != null);
            yield return new WaitUntil(() => huls.target == null);
        }
        huls.stats.baseSpeed = 0;
        SecuenceIndex++;
        DialogIndex++;
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));

    }

    private IEnumerator Attacks()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitUntil(() => GameObject.FindObjectOfType<arrojadiza>() != null );
            yield return new WaitUntil(() => GameObject.FindObjectOfType<arrojadiza>() == null);
        }
        huls.stats.baseSpeed = 0;
        huls.stats.baseAttackDistance = 0;
        SecuenceIndex++;
        DialogIndex++;
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));

    }

    private IEnumerator EnemyAttacks()
    {
        yield return new WaitUntil(() => GameObject.FindObjectOfType<Baston>() != null);
        yield return new WaitUntil(() => GameObject.FindObjectOfType<Baston>() == null);

        huls.stats.baseSpeed = 0;
        huls.stats.baseAttackDistance = 0;
        Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = 0f;
        SecuenceIndex++;
        DialogIndex++;
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));

    }

    private IEnumerator CrystalAppears()
    {
        yield return new WaitForSeconds(5);
        Pildora.SetActive(true);
        Pildora.GetComponent<Collectable>().timeToDissapear = 3000;
        huls.stats.baseSpeed = huls.stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0;
        SecuenceIndex++;
        DialogIndex++;
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));

    }

    private IEnumerator CrystalDissappears()
    {
        yield return new WaitUntil(() => GameObject.FindObjectOfType<Collectable>() == null);
        
        huls.stats.baseSpeed = huls.stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0;
        SecuenceIndex++;
        DialogIndex++;
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));

    }

    private IEnumerator FollowEnemy()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => huls.gameObject.GetComponent<EffectManager>().activeEffects.Count == 0);

        huls.stats.baseSpeed = huls.stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0;
        SecuenceIndex++;
        DialogIndex++;
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));

    }

    private IEnumerator KillEnemy()
    {
        yield return new WaitUntil(() => Esqueleto == null);

        //huls.stats.baseSpeed = huls.stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseAttackDistance = Esqueleto.GetComponent<Soldier>().stats.baseSpeed = 0;
        SecuenceIndex++;
        DialogIndex++;
        StartCoroutine(DM.Type(Dialogos[DialogIndex]));

    }
}
