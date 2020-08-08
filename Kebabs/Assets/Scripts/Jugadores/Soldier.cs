using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Soldier : SoldierStateMachine , IDamagable, IHealeable
{

    string[] map;

    private bool selected = false;

    private Animator anim;

    private SpriteRenderer sr;

    protected GameObject targetEnemy;

    public GameObject arma;

    public GameObject sangre;

    public GameObject nota;

    private bool riendo = false;
    

    public string opositeTag;

    public float defensa = 1;


    public Stats stats;

    private Vector3 direction = Vector3.zero;//Direccion del fantasma

    private Vector2 lastDirection = Vector2.down;

    GameController GC;

    public enum Type { HULS, CARLOS, OSKAR, DANI, PAULA, LOREA, LEYRE, MARIA, LUCIA, CARLOTA, THANIA, ESQUELETO2 };

    public Type myType;

    private bool atacando = false;

    public GameObject CampoLlanto;

    public GameObject lagrimas;

    protected bool canAttack = false;

    protected Coroutine attackCoroutine;

    public EffectManager effectManager;

    public GameObject followEnemy;

    class Location  //Nodo para el algoritmo de búsqueda
    {
        public int X;
        public int Y;
        public float F;
        public int G;
        public float H;
        public Location Parent;
    }
    Location target;
    Location provisionalTarget = null;

    private Transform bar;

    // Use this for initialization
    protected virtual void Start()
    {
        stats.maxVida = stats.vida;
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x * 10) * 0.1f, Mathf.RoundToInt(transform.position.y * 10) * 0.1f);
        GC = FindObjectOfType<GameController>();
        anim = this.GetComponent<Animator>();
        sr = transform.GetComponent<SpriteRenderer>();
        //InvokeRepeating("Attack", Random.Range(1, 3.5f), stats.AttackSpeed);
        Invoke("StartAttacking", Random.Range(1, 3.5f));
        attackCoroutine = StartCoroutine(AttackCoroutine());

        print("start");
        
        bar = transform.Find("Bar");
        if (this.tag == "Enemy")
        {
            InvokeRepeating("Move", 1, 5f);
            opositeTag = "Player";
            bar.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        else
        {
            opositeTag = "Enemy";
            bar.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {

        //Asignar mapa actual
        map = GC.GetMap();

        //asignar sorting layer
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20);

        //si se queda sin vida muere
        if (stats.vida == 0)
        {
            Die();
        }

        //lógica al pulsar click izquierdo con el ratón 
        OnMouseClick();

        if (followEnemy != null)
        {
            target = new Location
            {
                X = Mathf.Clamp(Mathf.RoundToInt(followEnemy.transform.position.x * 10), 1, 19),
                Y = Mathf.Clamp(Mathf.RoundToInt(followEnemy.transform.position.y * 10), 1, 19),
            };
        }

        //redondear a float con un decimal
        float posRoundX = RoundWithDecimals(transform.position.x, 1);
        float posRoundY = RoundWithDecimals(transform.position.y, 1);

        //
        if (target != null && Mathf.Abs(transform.position.x - posRoundX) < 0.005f
                           && Mathf.Abs(transform.position.y - posRoundY) < 0.005f)
        {
            if (Mathf.Abs(target.X / 10f - posRoundX) < 0.05f && Mathf.Abs(target.Y / 10f - posRoundY) < 0.05f)
            {
                direction = Vector2.zero;
                target = null;
            }
            else
            {
                //Posición actual para el primer nodo del A*
                Location initial = new Location
                {
                    X = Mathf.RoundToInt(transform.position.x * 10),
                    Y = Mathf.RoundToInt(transform.position.y * 10),
                };

                //Calculamos el siguiente movimiento
                Location next = A_estrella(initial, target);


                //Si lo encuentra, obtenemos la dirección que toma el fantasma
                if (next != null)
                {
                    if (provisionalTarget != null && Mathf.Abs(provisionalTarget.X / 10f - posRoundX) < 0.05f && Mathf.Abs(provisionalTarget.Y / 10f - posRoundY) < 0.05f)
                    {
                        direction = Vector2.zero;
                    }
                    else
                    {
                        direction = new Vector2(next.X - initial.X, next.Y - initial.Y).normalized;
                    }
                }
                else
                {
                    if (provisionalTarget != null && Mathf.Abs(provisionalTarget.X / 10f - posRoundX) < 0.05f && Mathf.Abs(provisionalTarget.Y / 10f - posRoundY) < 0.05f)
                    {
                        direction = Vector2.zero;
                    }
                }


            }


        }

        //Movemos el fantasma(personaje XD) en esa dirección
        if (direction != Vector3.zero)
        {
            lastDirection = direction;
            this.transform.Translate(direction * stats.Speed * Time.deltaTime);
        }

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

        SetWalkAnimator();

    }


    public void SetWalkAnimator()
    {
        if (direction == Vector3.up)
        {
            anim.SetInteger("Walk", 1);
        }
        else if (direction == Vector3.right)
        {
            anim.SetInteger("Walk", 0);
        }
        else if (direction == Vector3.down)
        {
            anim.SetInteger("Walk", 2);
        }
        else if (direction == Vector3.left)
        {
            anim.SetInteger("Walk", 3);
        }
        else
        {
            anim.SetInteger("Walk", 4);
        }
    }

    public float RoundWithDecimals(float num, int numDecimals)
    {
        float pow = Mathf.Pow(10, numDecimals);

        return Mathf.RoundToInt(num * pow) / pow;
    }

    public void OnMouseClick()
    {
        if (Input.GetMouseButton(0) && this.tag == "Player")
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();
            int layerMask = 1 << 8;
            if (Physics.Raycast(ray, out hitInfo, 100, layerMask))
            {
                if (hitInfo.transform == transform)
                {
                    selected = true;
                }
                else
                {
                    selected = false;
                }
            }
            else
            {
                if (selected)
                {
                    int layerMask2 = 1 << 9;
                    if (Physics.Raycast(ray, out hitInfo, 100, layerMask2))
                    {
                        followEnemy = hitInfo.collider.gameObject;
                    }
                    else
                    {
                        var v3 = Input.mousePosition;
                        v3.z = 10.0f;
                        v3 = Camera.main.ScreenToWorldPoint(v3);
                        int X = Mathf.Clamp(Mathf.RoundToInt(v3.x * 10), 1, 19);
                        int Y = Mathf.Clamp(Mathf.RoundToInt(v3.y * 10), 1, 19);
                        if (map[X][Y] != 'X')
                        {
                            followEnemy = null;
                            target = new Location
                            {
                                X = X,
                                Y = Y,
                            };
                        }
                    }
                }
            }
        }
    }

    protected void Die()
    {
        Instantiate(sangre, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    protected void Move()
    {
        int X = 0;
        int Y = 0;
        while (map[X][Y] == 'X')
        {
            X = Random.Range(1, 19);
            Y = Random.Range(1, 19);
        }
        target = new Location
        {
            X = X,
            Y = Y,
        };
    }

    
    public virtual void Attack()
    {

    }


    public void StartAttacking()
    {
        canAttack = true;
    }

    public IEnumerator AttackCoroutine()
    {
        while(true)
        {
            if(canAttack)
            {
                Attack();
            }

            yield return new WaitForSeconds(stats.AttackSpeed);
        }
    }


    private void Atack()
    {

        switch (myType)
        {
            case Type.HULS:
                {
                    targetEnemy = null;
                    float distance = stats.AttackDistance;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
                    {

                        if (Vector2.Distance(enemy.transform.position, transform.position) < distance)
                        {

                            distance = Vector2.Distance(enemy.transform.position, transform.position);
                            targetEnemy = enemy;
                        }
                    }
                    if (targetEnemy != null)
                    {
                        GameObject arrojadiza = Instantiate(arma, transform.position, Quaternion.identity);
                        arrojadiza.GetComponent<arrojadiza>().targetTag = opositeTag;
                        arrojadiza.GetComponent<arrojadiza>().target = targetEnemy;
                    }
                    break;
                }
            case Type.DANI:
                {
                    bool atacar = false;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
                    {
                        if (Vector2.Distance(enemy.transform.position, transform.position) < stats.AttackDistance)
                        {
                            atacar = true;
                        }
                    }
                    if (atacar)
                    {
                        GameObject baston = Instantiate(arma, transform.position, Quaternion.Euler(new Vector3(0, 0, 90 + Vector2.SignedAngle(Vector2.up, lastDirection))));
                        GameObject baston01 = GameObject.Find("baston_0");
                        baston01.GetComponent<Impacto>().targetTag = opositeTag;
                        baston.GetComponent<Baston>().owner = gameObject;


                    }
                    break;
                }
            case Type.THANIA:
                {
                    bool atacar = false;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
                    {
                        if (Vector2.Distance(enemy.transform.position, transform.position) < stats.AttackDistance)
                        {
                            atacar = true;
                        }
                    }
                    if (atacar)
                    {
                        float angulo = -0.4f;
                        for (int i = 0; i < 3; i += 1)
                        {
                            GameObject pollo = Instantiate(arma, transform.position, transform.rotation);
                            pollo.GetComponent<Pollo>().targetTag = opositeTag;
                            pollo.GetComponent<Pollo>().direccion = (lastDirection + Vector2.Perpendicular(lastDirection) * (angulo + angulo * (-i)));

                        }
                    }

                    break;
                }
            case Type.LUCIA:
                {
                    bool atacar = false;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
                    {
                        if (Vector2.Distance(enemy.transform.position, transform.position) < stats.AttackDistance)
                        {
                            atacar = true;
                        }
                    }
                    if (atacar)
                    {
                        GameObject cartel = Instantiate(arma, transform.position, Quaternion.identity);
                        cartel.GetComponent<Señal>().targetTag = opositeTag;
                        cartel.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, lastDirection));

                        cartel.transform.parent = this.transform;
                    }

                    break;
                }
            case Type.PAULA:
                {
                    
                    break;
                }
            case Type.LEYRE:
                {
                    GameObject nutella = Instantiate(arma, transform.position, Quaternion.identity);
                    nutella.GetComponent<Nutella>().targetTag = opositeTag;
                    break;
                }
            case Type.CARLOTA:
                {
                    StartCoroutine("notas");
                    break;
                }
            case Type.ESQUELETO2:
                {
                    StartCoroutine("huesos");
                    break;
                }
            case Type.MARIA:
                {
                    targetEnemy = null;
                    float distance = stats.AttackDistance;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
                    {

                        if (Vector2.Distance(enemy.transform.position, transform.position) < distance && enemy.GetComponent<Soldier>() != null)
                        {

                            distance = Vector2.Distance(enemy.transform.position, transform.position);
                            targetEnemy = enemy;
                        }
                    }
                    if (targetEnemy != null)
                    {
                        GameObject bomba = Instantiate(arma, transform.position, Quaternion.identity);
                        bomba.GetComponent<BombaCorazon>().targetPos = targetEnemy.transform.position + targetEnemy.GetComponent<Soldier>().direction * 0.3f;
                        bomba.GetComponent<BombaCorazon>().targetTag = opositeTag;
                    }
                    break;
                }
            case Type.CARLOS:
                {
                    bool atacar = false;
                    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
                    {
                        if (Vector2.Distance(enemy.transform.position, transform.position) < stats.AttackDistance)
                        {
                            atacar = true;
                        }
                    }
                    if (atacar)
                        StartCoroutine("rayos");
                    break;
                }
            case Type.OSKAR:
                {
                    if (GameObject.FindGameObjectsWithTag(opositeTag).Length > 0)
                    {
                        GameObject pato = Instantiate(arma, transform.position, Quaternion.identity);
                        pato.GetComponent<Pato>().opositeTag = opositeTag;
                        pato.tag = this.tag;
                    }

                    break;
                }
            case Type.LOREA:
                {
                    bool atacar = false;
                    foreach (GameObject friend in GameObject.FindGameObjectsWithTag(tag))
                    {
                        if (friend != this.gameObject)
                            if (friend.GetComponent<Pato>() == null && Vector2.Distance(friend.transform.position, transform.position) < stats.AttackDistance)
                            {
                                friend.GetComponent<Soldier>().GetHeal(10);
                                atacar = true;
                            }
                    }
                    if (atacar)
                        StartCoroutine(Vida());
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void EmpiezaContagio()
    {
        StartCoroutine(Contagiar());
    }

    private IEnumerator Contagiar()
    {
        if (!riendo)
        {
            riendo = true;
            InvokeRepeating("Reir", 0, 1.5f);
            yield return new WaitForSeconds(Random.Range(10, 15));
            CancelInvoke("Reir");
            riendo = false;
        }
        else
        {
            yield return null;
        }

    }

    private void Reir()
    {
        GameObject Notas = Instantiate(nota, transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), Quaternion.identity);
        Notas.transform.parent = transform;
        GetDamage(3);
        foreach (GameObject friend in GameObject.FindGameObjectsWithTag(tag))
        {
            if (friend != this.gameObject)
                if (friend.GetComponent<Pato>() == null && Vector2.Distance(friend.transform.position, transform.position) < 0.15)
                {
                    friend.GetComponent<Soldier>().EmpiezaContagio();
                }
        }
    }

    private IEnumerator Vida()
    {
        Transform campoReg = transform.Find("CampoReg");
        campoReg.GetComponent<Animator>().SetBool("activo", true);
        yield return new WaitForSeconds(1);
        campoReg.GetComponent<Animator>().SetBool("activo", false);
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

    private IEnumerator rayos()
    {
        for (int i = 0; i < 6; i += 1)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject rayo = Instantiate(arma, transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f)), Quaternion.identity);
            rayo.GetComponent<Rayo>().targetTag = opositeTag;
        }
        yield return null;
    }

    private IEnumerator notas()
    {
        for (int i = 0; i < 10; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject Nota = Instantiate(arma, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Nota.GetComponent<Nota>().targetTag = opositeTag;
        }
        yield return null;
    }

    private IEnumerator huesos()
    {
        for (int i = 0; i < 10; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject Hueso = Instantiate(arma, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Hueso.GetComponent<Hueso>().targetTag = opositeTag;
        }
        yield return null;
    }

    public void SetHealthBarSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void GetDamage(float daño)
    {

        stats.vida -= daño * ((100f-stats.DamageReduction)/100f);
        if (stats.vida < 0)
        {
            stats.vida = 0;
        }
        sr.color = new Color32(255, 83, 83, 255);
        SetHealthBarSize(stats.vida / stats.maxVida);
        StartCoroutine(RecuperarColor(0.2f));
    }

    public void GetHeal(float regeneracion)
    {
        stats.vida += regeneracion;
        if (stats.vida > stats.maxVida)
        {
            stats.vida = stats.maxVida;
        }
        sr.color = new Color32(129, 255, 133, 255);
        SetHealthBarSize(stats.vida / stats.maxVida);
        StartCoroutine(RecuperarColor(0.4f));
    }

    private IEnumerator RecuperarColor(float segs)
    {
        yield return new WaitForSeconds(segs);
        sr.color = Color.white;
    }

    //Usmaos la distancia euclídea para la heurística del A*
    static float ComputeHScore(float x, float y, float targetX, float targetY)
    {
        return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(targetX - x), 2) + Mathf.Pow(Mathf.Abs(targetY - y), 2));
    }

    //A* voy a prescindir de comentarlo que es lo de siempre XD
    private Location A_estrella(Location initial, Location target)
    {
        Location bestH = null;
        Location current = null;
        List<Location> openList = new List<Location>();
        List<Location> closedList = new List<Location>();

        initial.G = 0;
        initial.Parent = null;
        initial.H = ComputeHScore(initial.X, initial.Y, target.X, target.Y);
        initial.F = initial.H + initial.G;

        bestH = initial;
        openList.Add(initial);
        while (openList.Count > 0)
        {
            float minim = 9999;
            foreach (Location l in openList)
            {
                if (l.F < minim)
                {
                    current = l;
                    minim = l.F;
                }
            }
            //print(current.X +" " + current.Y + " " + current.F);
            openList.Remove(current);

            closedList.Add(current);

            if (current.H < bestH.H || (current.H == bestH.H && current.G < bestH.G))
            {
                bestH = current;
            }

            if (current.X == target.X && current.Y == target.Y)
            {
                break;
            }


            var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y, map);
            foreach (var adjacentSquare in adjacentSquares)
            {
                // if this adjacent square is already in the closed list, ignore it

                bool isIn = false;
                foreach (Location l in closedList)
                {
                    if (l.X == adjacentSquare.X && l.Y == adjacentSquare.Y)
                    {
                        isIn = true;
                    }
                }
                if (isIn)
                {
                    continue;
                }



                // if it's not in the open list...
                isIn = false;
                Location aux = null;
                foreach (Location l in openList)
                {
                    if (l.X == adjacentSquare.X && l.Y == adjacentSquare.Y)
                    {
                        isIn = true;
                        aux = l;
                    }
                }
                if (!isIn)
                {
                    // compute its score, set the parent
                    adjacentSquare.G = current.G + 1;
                    adjacentSquare.H = ComputeHScore(adjacentSquare.X,
                        adjacentSquare.Y, target.X, target.Y);
                    adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                    adjacentSquare.Parent = current;

                    // and add it to the open list
                    openList.Add(adjacentSquare);
                }
                else
                {
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (current.G + 1 + aux.H < aux.F)
                    {
                        aux.G = current.G + 1;
                        aux.F = aux.G + aux.H;
                        aux.Parent = current;
                    }
                }
            }
        }

        if (openList.Count == 0)
        {
            current = bestH;
            provisionalTarget = bestH;
        }
        else
        {
            provisionalTarget = null;
        }
        Location next = null;
        while (current.Parent != null)
        {
            next = current;
            current = current.Parent;
        }
        return next;
    }

    static List<Location> GetWalkableAdjacentSquares(int x, int y, string[] map)
    {
        var proposedLocations = new List<Location>()
        {
            new Location { X = x, Y = y - 1 },
            new Location { X = x, Y = y + 1 },
            new Location { X = x - 1, Y = y },
            new Location { X = x + 1, Y = y },
        };

        var aux = new List<Location>();

        foreach (Location l in proposedLocations)
        {
            if (map[l.X][l.Y] == 'X')
            {
                aux.Add(l);
            }
        }
        foreach (Location l in aux)
        {
            proposedLocations.Remove(l);
        }
        return proposedLocations;
    }

}
