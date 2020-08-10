using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHuir : SoldierState
{
    bool canCheckPath = true;

    List<Location> proposedEscapePoints = new List<Location>()
        {
            new Location { X = 3, Y = 2 },
            new Location { X = 17, Y = 2 },
            new Location { X = 10, Y = 2 },
            new Location { X = 3, Y = 10 },
            new Location { X = 17, Y = 10 },
            new Location { X = 10, Y = 10 },
            new Location { X = 3, Y = 17 },
            new Location { X = 17, Y = 17 },
            new Location { X = 10, Y = 17}
        };

    public SoldierHuir(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        yield return null;

        //Location bestPlace = FindSaveWay();

        //if (bestPlace == null)
        //{
        //    ChangeState();
        //}
        //else
        //{
        //    soldier.target = bestPlace;
        //}

    }

    public override void Update()
    {
        base.Update();
        //redondear a float con un decimal
        float posRoundX = soldier.RoundWithDecimals(soldier.transform.position.x, 1);
        float posRoundY = soldier.RoundWithDecimals(soldier.transform.position.y, 1);

        //
        if (Mathf.Abs(soldier.transform.position.x - posRoundX) < 0.005f && Mathf.Abs(soldier.transform.position.y - posRoundY) < 0.005f && canCheckPath)
        {
            canCheckPath = false;
            soldier.StartCoroutine(ResetCanCheckPath());

            Location bestPlace = FindSaveWay();

            if (bestPlace == null)
            {
                ChangeState();
            }
            else
            {
                soldier.target = bestPlace;
            }

        }
    }

    
    public IEnumerator ResetCanCheckPath()
    {
        yield return new WaitForSeconds(0.1f);
        canCheckPath = true;
        
    }

    public Location FindSaveWay()
    {
        float sumDist = 0;
        float maxDist = 0;
        Location bestPlace = null;
        Location bestPlaceSiHayPeligro = null;
        float maxDistSiHayPeligro = 0;
        List<Location> bestPath = new List<Location>();
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(soldier.opositeTag));
        if (enemies.Count <= 0) return null;

        foreach (Location location in proposedEscapePoints)
        {
            if (!CheckPeligro(enemies, new List<Location> { location}))
            {
                sumDist = 0;


                foreach (GameObject enemy in enemies)
                {
                    Location target = new Location
                    {
                        X = Mathf.Clamp(Mathf.RoundToInt(enemy.transform.position.x * 10), 1, 19) + soldier.border,
                        Y = Mathf.Clamp(Mathf.RoundToInt(enemy.transform.position.y * 10), 1, 19) + soldier.border,
                    };

                    bestPath = soldier.A_estrella_Coste(location, target);

                    sumDist += bestPath.Count;
                        
                }
                //Debug.Log(sumDist);

                if (CheckPeligro(enemies, bestPath))
                {
                    Debug.Log("Hay peligro");
                    if (sumDist > maxDistSiHayPeligro)
                    {
                        Debug.Log("Asigna bestPathSiHayPeligro");
                        maxDistSiHayPeligro = sumDist;
                        bestPlaceSiHayPeligro = location;
                    }
                        
                }
                else
                {
                    Debug.Log("No hay peligro");
                    if (sumDist > maxDist)
                    {
                        maxDist = sumDist;
                        bestPlace = location;
                    }
                }
            }
        }
        if (bestPlace == null)
        {
            if (bestPlaceSiHayPeligro == null)
            {
                Debug.Log("No hay sitio seguro");
                return null;
            }
            else
            {
                Debug.Log("Hay sitio con peligro en el camino");
                return bestPlaceSiHayPeligro;
            }
        }
        else
        {
            Debug.Log("Hay sitio con camino seguro");
            return bestPlace;
        }
    }

    public bool CheckPeligro(List<GameObject> enemies, List<Location> bestPath)
    {
        bool hayPeligro = false;
        foreach (Location casilla in bestPath)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Soldier>())
                {
                    Soldier sold = enemy.GetComponent<Soldier>();
                    if (sold.stats.AttackDistance > Vector2.Distance(enemy.transform.position, new Vector2(casilla.X, casilla.Y)))
                    {
                        hayPeligro = true;
                    }
                }
            }
        }


        return hayPeligro;
    }

    public override void ChangeState()
    {
        
        soldier.StateMachineLogic();
    }

}
