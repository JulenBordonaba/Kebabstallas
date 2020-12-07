using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Pato : MonoBehaviour
{

    string[] map;

    //private bool selected = false;

    private Animator anim;

    private SpriteRenderer sr;

    private GameObject targetEnemy;

    public float maxVida = 20f;

    public float vida = 20f;

    public string opositeTag;

    public float daño = 10;

    public float defensa = 1;

    public float speed = 0.15f;

    private float atackDistance;

    private bool exploting = false;

    private bool CanSound = true;

    private float timer;

    public Vector2 direction = Vector2.zero;//Direccion del fantasma

    GameController GC;

    public float TiempoVida;

    bool blinking = false;

    public GameObject sangre;


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
    void Awake()
    {
        GC = FindObjectOfType<GameController>();
        anim = this.GetComponent<Animator>();
        sr = transform.GetComponent<SpriteRenderer>();
        bar = transform.Find("Bar");
        //opositeTag = "Untagged";
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x*10)*0.1f,Mathf.RoundToInt(transform.position.y * 10) * 0.1f);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        map = GC.GetMap();
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20);

        targetEnemy = null;
        float distance = Mathf.Infinity;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(opositeTag))
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < distance)
            {
                distance = Vector2.Distance(enemy.transform.position, transform.position);
                targetEnemy = enemy;
            }
        }
        timer += Time.deltaTime;
        if (timer > TiempoVida)
        {
            StartCoroutine("Explode");
            timer = 0;
        }else if(timer > TiempoVida - 1.5f && !blinking)
        {
            blinking = true;
            StartCoroutine("Blink");
        }
        if (targetEnemy != null)
        {
            
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) < 0.13f)
            {
                
                StartCoroutine("Explode");
            }

            target = new Location
            {
                X = Mathf.Clamp(Mathf.RoundToInt(targetEnemy.transform.position.x * 10), 1, 19),
                Y = Mathf.Clamp(Mathf.RoundToInt(targetEnemy.transform.position.y * 10), 1, 19)
            };
        }
        

        if (vida <= 0)
        {
            Die();
        }

        float posRoundX = Mathf.RoundToInt(transform.position.x * 10f) / 10f;
        float posRoundY = Mathf.RoundToInt(transform.position.y * 10f) / 10f;

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

        //Movemos el fantasma en esa dirección
        if (direction != Vector2.zero && !exploting)
        {
            this.transform.Translate(direction * speed * Time.deltaTime);
        }

        if (direction == Vector2.up)
        {
            anim.SetInteger("Walk", 1);
        }
        else if (direction == Vector2.right)
        {
            anim.SetInteger("Walk", 0);
        }
        else if (direction == Vector2.down)
        {
            anim.SetInteger("Walk", 2);
        }
        else if (direction == Vector2.left)
        {
            anim.SetInteger("Walk", 3);
        }
        else
        {
            anim.SetInteger("Walk", 4);
        }
    }

    private IEnumerator Blink()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.3f);
    }

    private IEnumerator Explode()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child != transform)
                child.gameObject.SetActive(false);
        }
        exploting = true;
        if (CanSound)
        {
            AudioManager.PlaySound(AudioManager.Sound.EXPLOSIONPATO);
            CanSound = false;
        }
        anim.SetBool("Explode", true);
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }

    private void Die()
    {
        Instantiate(sangre, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void SetHealthBarSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void RecibirDaño(float daño)
    {
        vida -= daño*defensa;
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
            if (l.X < 0 || l.X > 20 || l.Y < 0 || l.Y > 20)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == opositeTag && exploting)
        {
            if (other != null)
            {
                if (other.GetComponent<Soldier>())
                    other.GetComponent<Soldier>().GetDamage(daño);
                else if (other.GetComponent<Pato>())
                    other.GetComponent<Pato>().RecibirDaño(daño);
            }
                
        }
    }

}
