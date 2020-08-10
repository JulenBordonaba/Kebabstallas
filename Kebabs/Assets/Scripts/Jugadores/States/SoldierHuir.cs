using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHuir : SoldierState
{
    bool canCheckPath = true;

    List<Location> proposedEscapePoints = new List<Location>()
        {
            new Location { X = 5, Y = 2 },
            new Location { X = 15, Y = 2 },
            new Location { X = 10, Y = 2 },
            new Location { X = 3, Y = 10 },
            new Location { X = 17, Y = 10 },
            new Location { X = 10, Y = 10 },
            new Location { X = 5, Y = 17 },
            new Location { X = 15, Y = 17 },
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
        GameController.OnCollectablePlaced.AddListener(ChangeState);
        Collectable.OnCollectableCollected.AddListener(ChangeState);
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
            if (VidaCerca())
            {
                soldier.SetState(new SoldierFindHealConsumable(soldier));
                return;
            }

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

    public bool VidaCerca()
    {
        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");

        
        foreach (GameObject consumable in consumables)
        {
            if (consumable.GetComponent<Collectable>())
            {
                Collectable collectable = consumable.GetComponent<Collectable>();
                if (collectable.myType == Collectable.Type.VIDA)
                {


                    if(Vector2.Distance(consumable.transform.position,soldier.transform.position)<0.8f)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public IEnumerator ResetCanCheckPath()
    {
        yield return new WaitForSeconds(0.5f);
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
        List<GameObject> minions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Miniom"));
        

        foreach(GameObject miniom in minions)
        {
            if(miniom.GetComponent<Oso>())
            {
                if(miniom.GetComponent<Oso>().opositeTag==soldier.tag)
                {
                    enemies.Add(miniom);
                }
            }
        }

        if (enemies.Count <= 0) return null;

        foreach (Location location in proposedEscapePoints)
        {
            if (!CheckPeligro(enemies, new List<Location> { location }))
            {
                sumDist = 0;

                float nearestEnemyDistance = float.MaxValue;
                foreach (GameObject enemy in enemies)
                {
                    Location target = new Location
                    {
                        X = Mathf.Clamp(Mathf.RoundToInt(enemy.transform.position.x * 10), 1, 19) + soldier.border,
                        Y = Mathf.Clamp(Mathf.RoundToInt(enemy.transform.position.y * 10), 1, 19) + soldier.border,
                    };

                    
                    bestPath = soldier.A_estrella_Coste(location, target);

                    if(bestPath.Count<nearestEnemyDistance)
                    {
                        nearestEnemyDistance = bestPath.Count;
                    }

                    //sumDist += bestPath.Count;

                    

                }
                //Debug.Log(sumDist);

                if (CheckPeligro(enemies, bestPath))
                {
                    Debug.Log("Hay peligro");
                    if (nearestEnemyDistance > maxDistSiHayPeligro)
                    {
                        Debug.Log("Asigna bestPathSiHayPeligro");
                        maxDistSiHayPeligro = nearestEnemyDistance;
                        bestPlaceSiHayPeligro = location;
                    }

                }
                else
                {
                    Debug.Log("No hay peligro");
                    if (nearestEnemyDistance > maxDist)
                    {
                        maxDist = nearestEnemyDistance;
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
        //foreach (Location casilla in bestPath)
        for (int i = 0; i < bestPath.Count; i += 3)
        {

            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Soldier>())
                {
                    Soldier sold = enemy.GetComponent<Soldier>();
                    if (sold.stats.AttackDistance > Vector2.Distance(enemy.transform.position, new Vector2(bestPath[i].X, bestPath[i].Y)))
                    {
                        hayPeligro = true;
                    }
                }
                else if (enemy.GetComponent<Oso>())
                {
                    if(0.4f > Vector2.Distance(enemy.transform.position, new Vector2(bestPath[i].X, bestPath[i].Y)))
                    {
                        hayPeligro = true;
                    }
                }
                else if (enemy.GetComponent<Pato>())
                {
                    if (0.2f > Vector2.Distance(enemy.transform.position, new Vector2(bestPath[i].X, bestPath[i].Y)))
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
        base.ChangeState();
        soldier.StateMachineLogic();
    }

    public override void UnsubscribeFromEvents()
    {
        GameController.OnCollectablePlaced.RemoveListener(ChangeState);
        Collectable.OnCollectableCollected.RemoveListener(ChangeState);
    }

}
