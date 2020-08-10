using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHuir : SoldierState
{
    bool canCheckPath = true;

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
            soldier.Invoke("ResetCanCheckPath", 0.1f);

            Location bestPlace = FindSaveWay();

            if (bestPlace == null)
            {
                Debug.Log("Change State: No encuentro sitio seguro");
                ChangeState();
            }
            else
            {
                soldier.target = bestPlace;
            }

        }
    }

    public void ResetCanCheckPath()
    {
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

        for (int i = 1; i < 19; i++)
        {
            for (int j = 1; j < 19; j++)
            {
                if (soldier.map[i][j] != 'X' && !CheckPeligro(enemies, new List<Location> { new Location { X = i, Y = j } }))
                {
                    Debug.Log("primer if");
                    sumDist = 0;



                    foreach (GameObject enemy in enemies)
                    {
                        Location initial = new Location
                        {
                            X = i,
                            Y = j,
                        };

                        Location target = new Location
                        {
                            X = Mathf.Clamp(Mathf.RoundToInt(enemy.transform.position.x * 10), 1, 19) + soldier.border,
                            Y = Mathf.Clamp(Mathf.RoundToInt(enemy.transform.position.y * 10), 1, 19) + soldier.border,
                        };

                        bestPath = soldier.A_estrella_Coste(initial, target);

                        sumDist += bestPath.Count;
                        Debug.Log(sumDist);
                    }

                    if (CheckPeligro(enemies, bestPath))
                    {
                        Debug.Log("Hay peligro");
                        if (sumDist > maxDistSiHayPeligro)
                        {
                            Debug.Log("Asigna bestPathSiHayPeligro");
                            maxDistSiHayPeligro = sumDist;
                            bestPlaceSiHayPeligro = new Location();
                            bestPlaceSiHayPeligro.X = i;
                            bestPlaceSiHayPeligro.Y = j;
                        }
                        
                    }
                    else
                    {
                        Debug.Log("No hay peligro");
                        if (sumDist > maxDist)
                        {

                            maxDist = sumDist;
                            bestPlace = new Location();
                            bestPlace.X = i;
                            bestPlace.Y = j;
                        }
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
                    if (sold.stats.AttackDistance < Vector2.Distance(enemy.transform.position, new Vector2(casilla.X, casilla.Y)))
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
