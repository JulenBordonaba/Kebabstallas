using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Events;

public class Soldier : SoldierStateMachine, IDamagable, IHealeable
{

    public string[] map;

    private bool selected = false;

    private Animator anim;

    private SpriteRenderer sr;

    protected GameObject targetEnemy;

    public GameObject arma;

    public GameObject sangre;

    public GameObject nota;

    public bool riendo = false;

    private bool escudo = false;

    public int border = 0;

    public int nutellaCount = 0;

    private bool isNutella = false;

    
        
    public string opositeTag;

    public float debilidad = 1;


    public Stats stats;

    public Vector3 direction = Vector3.zero;//Direccion del fantasma

    protected Vector2 lastDirection = Vector2.down;

    public GameController GC;

    public enum Type { HULS, CARLOS, OSKAR, DANI, PAULA, LOREA, LEYRE, MARIA, LUCIA, CARLOTA, THANIA, ESQUELETO2 };

    public Type myType;

    private bool CanSound = true;

    private float notaDamage = 0;



    protected bool canAttack = false;

    protected Coroutine attackCoroutine;

    public EffectManager effectManager;

    public GameObject followTarget;

    private Color myColor = Color.white;

    public Dictionary<GameController.MapType, List<Location>> proposedEscapePoints = new Dictionary<GameController.MapType, List<Location>>();

    public Location target;
    public Location provisionalTarget = null;

    private Transform bar;

    public static UnityEvent OnDamageDealed = new UnityEvent();


    private Vector3 currentPoint;

    private Vector3 nextPoint;

    private float Timer = 0;

    


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

        currentPoint = nextPoint = transform.position;

        effectManager.OnEffectStart.AddListener(UpdateSpriteColor);
        effectManager.OnEffectEnd.AddListener(UpdateSpriteColor);

        bar = transform.Find("Bar");
        if (this.tag == "Enemy")
        {
            //InvokeRepeating("Move", 1, 5f);
            Invoke("StateMachineLogic",Random.Range(0.1f,2f));
            opositeTag = "Player";
            IA = true;
            bar.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        else
        {
            opositeTag = "Enemy";
            IA = false;
            bar.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }

        proposedEscapePoints.Add(GameController.MapType.TEMPLE, new List<Location>()
        {
            new Location { X = 5, Y = 2 },
            new Location { X = 15, Y = 2 },
            new Location { X = 10, Y = 2 },
            new Location { X = 5, Y = 10 },
            new Location { X = 15, Y = 10 },
            new Location { X = 10, Y = 10 },
            new Location { X = 5, Y = 17 },
            new Location { X = 15, Y = 17 },
            new Location { X = 10, Y = 17}
        });

        proposedEscapePoints.Add(GameController.MapType.HELL,

        new List<Location>()
        {
            new Location { X = 10, Y = 5 },
            new Location { X = 5, Y = 10 },
            new Location { X = 10, Y = 15 },
            new Location { X = 15, Y = 10 },
            new Location { X = 10, Y = 10 }
        });

        proposedEscapePoints.Add(GameController.MapType.PRISION,

        new List<Location>()
        {
            new Location { X = 10, Y = 10 },
            new Location { X = 8, Y = 1 },
            new Location { X = 12, Y = 19 },
            new Location { X = 15, Y = 8 },
            new Location { X = 15, Y = 1 },
            new Location { X = 5, Y = 19 },
            new Location { X = 5, Y = 12 },
        });

        proposedEscapePoints.Add(GameController.MapType.CLIFF,

        new List<Location>()
        {
            new Location { X = 10, Y = 10 },
            new Location { X = 14, Y = 14 },
            new Location { X = 6, Y = 14 },
            new Location { X = 14, Y = 4 },
            new Location { X = 6, Y = 4 },
        });


    }

    // Update is called once per frame
    protected override void Update()
    {
        //Asignar mapa actual
        map = GC.GetMap();

        base.Update();
        

        

        //asignar sorting layer
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20);

        //si se queda sin vida muere
        if (stats.vida == 0)
        {
            Die();
        }

        //lógica al pulsar click izquierdo con el ratón 
        OnMouseClick();

        if (followTarget != null)
        {
            target = new Location
            {
                X = Mathf.Clamp(Mathf.RoundToInt(followTarget.transform.position.x * 10), 1, 19) + border,
                Y = Mathf.Clamp(Mathf.RoundToInt(followTarget.transform.position.y * 10), 1, 19) + border,
            };
        }

        Timer += Time.deltaTime;

        if (target != null && Timer > 0.1f / stats.Speed)
        {
            Timer = 0;
            currentPoint = nextPoint;
            transform.position = currentPoint;

            float posRoundX = Mathf.RoundToInt(transform.position.x * 10f) / 10f;
            float posRoundY = Mathf.RoundToInt(transform.position.y * 10f) / 10f;

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
        /*
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
                    X = Mathf.RoundToInt(transform.position.x * 10) + border,
                    Y = Mathf.RoundToInt(transform.position.y * 10) + border,
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
                        direction = new Vector2 (Mathf.RoundToInt(direction[0]), Mathf.RoundToInt(direction[1]));
                    }
                }
                else
                {
                    if (provisionalTarget != null && Mathf.Abs(provisionalTarget.X / 10f - posRoundX) < 0.05f && Mathf.Abs(provisionalTarget.Y / 10f - posRoundY) < 0.05f)
                    {
                        direction = Vector2.zero;
                        
                    }
                }


            }       x/1     0.1/y


        }*/

        nextPoint = currentPoint + direction / 10f;

        //Movemos el fantasma(personaje XD) en esa dirección
        if (direction != Vector3.zero)
        {
            lastDirection = direction;
            float t = Timer * stats.Speed / 0.1f;
            transform.position = Vector3.Lerp(currentPoint, nextPoint, t);
            //this.transform.Translate(direction * stats.Speed * Time.deltaTime);
        }




        SetWalkAnimator(direction);

        if (state == null && IA)
        {
            StateMachineLogic();
        }

        try
        {
            if (followTarget != null && direction == Vector3.zero)
            {
                Vector3 provisionalDirection = -(transform.position - followTarget.transform.position).normalized;
                provisionalDirection = new Vector2(Mathf.RoundToInt(provisionalDirection[0]), Mathf.RoundToInt(provisionalDirection[1]));
                lastDirection = provisionalDirection;
                if (provisionalDirection == Vector3.up)
                {
                    anim.Play("IdleU");
                }
                else if (provisionalDirection == Vector3.right)
                {
                    anim.Play("IdleR");
                }
                else if (provisionalDirection == Vector3.down)
                {
                    anim.Play("IdleD");
                }
                else if (provisionalDirection == Vector3.left)
                {
                    anim.Play("IdleL");
                }
            }
        }
        catch
        {

        }
        

    }


    public void SetWalkAnimator(Vector3 _direction)
    {
        if (stats.Speed == 0)
        {
            anim.SetInteger("Walk", 4);
        }
        else
        {
            if (_direction == Vector3.up)
            {
                anim.SetInteger("Walk", 1);
            }
            else if (_direction == Vector3.right)
            {
                anim.SetInteger("Walk", 0);
            }
            else if (_direction == Vector3.down)
            {
                anim.SetInteger("Walk", 2);
            }
            else if (_direction == Vector3.left)
            {
                anim.SetInteger("Walk", 3);
            }
            else
            {
                anim.SetInteger("Walk", 4);
                
            }
        }

    }

    public float RoundWithDecimals(float num, int numDecimals)
    {
        float pow = Mathf.Pow(10, numDecimals);

        return Mathf.RoundToInt(num * pow) / pow;
    }

    public void OnMouseClick()
    {

        if (Input.GetMouseButtonDown(0) && this.tag == "Player")
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();
            int layerMask = 1 << 8;
            if (Physics.Raycast(ray, out hitInfo, 100, layerMask))
            {
                if (hitInfo.transform == transform)
                {
                    selected = true;
                    GC.ChangeSelected(this);
                    IA = false;
                    state = null;
                }
                else
                {
                    selected = false;
                }
            }
            else
            {
                float dist = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist <= 0.1f &&  dist <= GC.SelectedDistance)
                {
                    GC.SelectedDistance = dist;
                    selected = true;
                    GC.ChangeSelected(this);
                    IA = false;
                    state = null;
                }
                else
                {
                    selected = false;
                }
                
            }
        }



        if (Input.GetMouseButtonUp(0) && this.tag == "Player")
        {
            //if(Camera.main.GetComponent<PanZoom>().direction.magnitude * (1/Camera.main.orthographicSize)> 0.001f)
            //{
            //    Camera.main.GetComponent<PanZoom>().direction = Vector3.zero;
            //    return;
            //}

            
            if (selected)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo = new RaycastHit();
                int layerMask2 = 1 << 9;
                if (Physics.Raycast(ray, out hitInfo, 100, layerMask2))
                {
                    followTarget = hitInfo.collider.gameObject;
                    GC.FollowArrow(followTarget);
                }
                else
                {
                    var v3 = Input.mousePosition;
                    v3.z = 10.0f;
                    v3 = Camera.main.ScreenToWorldPoint(v3);
                    int X = Mathf.Clamp(Mathf.RoundToInt(v3.x * 10), 1, 19) + border;
                    int Y = Mathf.Clamp(Mathf.RoundToInt(v3.y * 10), 1, 19) + border;
                    followTarget = null;
                    target = new Location
                    {
                        X = X,
                        Y = Y,
                    };
                }

                GC.ChangeSelected(null);
            }
            GC.SelectedDistance = int.MaxValue;
        }
    }

    protected void Die()
    {
        if (transform.Find("DownArrow"))
        {
            transform.Find("DownArrow").parent = null;
            transform.Find("DownArrow").position = new Vector2(0, -10f);
        }
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
        attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    public IEnumerator AttackCoroutine()
    {
        Attack();
        while (true)
        {
            yield return new WaitForSeconds(stats.AttackSpeed);
            if (CanAttack)
            {
                Attack();
            }
        }
    }

    public void EmpiezaContagio(float damage)
    {
        StartCoroutine(Contagiar(damage));
    }

    private IEnumerator Contagiar(float damage)
    {
        if (!riendo)
        {
            riendo = true;
            notaDamage = damage;
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
        GetDamage(notaDamage);
        foreach (GameObject friend in GameObject.FindGameObjectsWithTag(tag))
        {
            if (friend != this.gameObject)
                if (friend.GetComponent<Pato>() == null && Vector2.Distance(friend.transform.position, transform.position) < 0.15)
                {
                    friend.GetComponent<Soldier>().EmpiezaContagio(notaDamage);
                }
        }
    }

    //protected IEnumerator Vida()
    //{
    //    Transform campoReg = transform.Find("CampoReg");
    //    campoReg.GetComponent<Animator>().SetBool("activo", true);
    //    yield return new WaitForSeconds(1);
    //    campoReg.GetComponent<Animator>().SetBool("activo", false);
    //}


    public void CheckNutella()
    {
        if (nutellaCount <= 0)
        {
            if (isNutella)
            {
                isNutella = false;
                stats.speedReplace = -1;
                Timer /= stats.Speed / 0.1f;
            }
        }
        else
        {
            if (!isNutella)
            {
                isNutella = true;
                Timer /= 0.1f / stats.Speed;
                stats.speedReplace = 0.1f;
                
            }
            
        }
    }

    public void SetHealthBarSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void GetDamage(float daño)
    {
        if (!escudo)
        {
            if (CanSound)
            {
                AudioManager.PlaySound(AudioManager.Sound.HIT);
                CanSound = false;
                StartCoroutine("WaitUntilCanSoundAgain");
            }
            stats.vida -= debilidad * daño * ((100f - stats.DamageReduction) / 100f);
            
            if (stats.vida < 0)
            {
                stats.vida = 0;
            }
            SetHealthBarSize(stats.vida / stats.maxVida);

            OnDamageDealed.Invoke();

            object[] parms = new object[2] { 0.2f, new Color32(255, 83, 83, 255) };
            StartCoroutine("OriginalColorChange", parms);
        }

    }

    private IEnumerator WaitUntilCanSoundAgain()
    {
        yield return new WaitForSeconds(0.5f);
        CanSound = true;
    }

    public void GetHeal(float regeneracion)
    {
        stats.vida += regeneracion;
        if (stats.vida > stats.maxVida)
        {
            stats.vida = stats.maxVida;
        }
        SetHealthBarSize(stats.vida / stats.maxVida);

        OnDamageDealed.Invoke();

        object[] parms = new object[2] { 0.4f, new Color32(129, 255, 133, 255) };
        StartCoroutine("OriginalColorChange", parms);
    }

    public IEnumerator ActivateShield()
    {
        escudo = true;
        transform.Find("escudito").GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(15f);
        escudo = false;
        transform.Find("escudito").GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator OriginalColorChange(object[] parms)
    {
        myColor = (Color32)parms[1];
        //sr.color = (Color)parms[1];
        UpdateSpriteColor();
        yield return new WaitForSeconds((float)parms[0]);
        myColor = Color.white;
        UpdateSpriteColor();
        //sr.color = Color.white;
    }

    private void UpdateSpriteColor()
    {
        sr.color = SpriteColor;
    }

    //private IEnumerator RecuperarColor(float segs)
    //{
    //    yield return new WaitForSeconds(segs);
    //    sr.color = myColor;
    //}

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

            if (adjacentSquares == null)
            {
                print("ATENCION, SI SALE ESTO ES POSIBLE QUE EL CORRECTOR DE BUG DE QUE NO SE SALGAN DEL MAPA NO VAYA BIEN");
                return new Location { X = 10, Y = 10 };
            }

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

    public List<Location> A_estrella_Coste(Location initial, Location target)
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

        List<Location> path = new List<Location>();

        Location next = null;
        while (current.Parent != null)
        {
            path.Add(current);
            next = current;
            current = current.Parent;
        }
        path.Add(current);

        return path;
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
            if (l.X < 0 || l.X >20 || l.Y < 0 || l.Y > 20)
            {
                return null;
            }
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

    public bool CanAttack
    {
        get { return canAttack && !effectManager.SilenceAbilities; }
    }

    public Color SpriteColor
    {
        get { return myColor * effectManager.EffectColor; }
    }


    public virtual void StateMachineLogic()
    {
        InvokeRepeating("Move", 1, 5f);
    }

    public float Danger
    {
        get
        {
            float danger = 0;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(opositeTag);
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Soldier>())
                {
                    Soldier sold = enemy.GetComponent<Soldier>();
                    if (Vector2.Distance(transform.position, enemy.transform.position) < sold.stats.AttackDistance)
                    {
                        danger += sold.stats.help;
                    }
                }

            }
            return danger;
        }
    }

    public float Help
    {
        get
        {
            float help = 0;
            GameObject[] allies = GameObject.FindGameObjectsWithTag(tag);
            foreach(GameObject ally in allies)
            {
                if(ally.GetComponent<Soldier>())
                {
                    Soldier sold = ally.GetComponent<Soldier>();
                    if(Vector2.Distance(transform.position,ally.transform.position)<(stats.AttackDistance+sold.stats.AttackDistance)*0.5f)
                    {
                        help += sold.stats.help;
                    }
                    
                }
                
            }
            return help;
        }
    }

    public float DangerBalance
    {
        get
        {
            return Help - Danger;
        }
    }

    public bool CheckNearConsumables(float dist)
    {
        foreach (GameObject consumable in GameObject.FindGameObjectsWithTag("Consumable"))
        {
            if (consumable != null)
            {
                Collectable collectable = consumable.GetComponent<Collectable>();
                if (collectable.myType != Collectable.Type.PETRIFICACION)
                {
                    if (!(collectable.myType == Collectable.Type.EXPLOSION && collectable.NearestSoldier.tag == opositeTag))
                    {
                        try
                        {
                            if (Vector2.Distance(transform.position, collectable.transform.position) < dist)
                            {
                                return true;
                            }
                        }
                        catch
                        {

                        }
                    }

                }

            }
        }

        return false;
    }

}

public class Location  //Nodo para el algoritmo de búsqueda
{
    public int X;
    public int Y;
    public float F;
    public int G;
    public float H;
    public Location Parent;
}