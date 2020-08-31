using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFollowAtDistanceEuclidea : SoldierState
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

    public SoldierFollowAtDistanceEuclidea(Soldier soldier) : base(soldier)
    {

    }

    public override IEnumerator Start()
    {
        base.Start();
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
        Soldier.OnDamageDealed.AddListener(ChangeState);
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

            Soldier nearestEnemy = null;

            float distanceToEnemy = DistanceToEnemy(out nearestEnemy);

            Location loc = null;

            if (distanceToEnemy < soldier.stats.AttackDistance*0.5f)
            {

                Location bestPlace = FindSaveWay();

                if (bestPlace == null && nearestEnemy != null)
                {
                    loc = RunToEnemy(nearestEnemy);
                }
                else
                {
                    loc = RunAway(bestPlace, distanceToEnemy, nearestEnemy);
                }
            }
            else if (distanceToEnemy > soldier.stats.AttackDistance && nearestEnemy != null)
            {
                loc = RunToEnemy(nearestEnemy);
            }

            if (loc != null)
            {
                soldier.target = loc;
            }






        }

    }

    public Location RunToEnemy(Soldier nearestEnemy)
    {
        Location initial = new Location
        {
            X = Mathf.RoundToInt(soldier.transform.position.x * 10) + soldier.border,
            Y = Mathf.RoundToInt(soldier.transform.position.y * 10) + soldier.border,
        };

        Location enemyLocation = new Location
        {
            X = Mathf.RoundToInt(nearestEnemy.transform.position.x * 10) + nearestEnemy.border,
            Y = Mathf.RoundToInt(nearestEnemy.transform.position.y * 10) + nearestEnemy.border,
        };


        List<Location> bestPath = new List<Location>();

        bestPath = soldier.A_estrella_Coste(initial, enemyLocation);



        

        return MostCenteredLocation(bestPath,nearestEnemy);
    }

    public Location MostCenteredLocation(List<Location> path, Soldier nearestEnemy)
    {
        float closestDistance = float.MaxValue;
        Location closestLocation = null;

        foreach (Location location in path)
        {
            float enemyDistance = Vector2.Distance(nearestEnemy.transform.position, new Vector2(location.X / 10f, location.Y / 10f));
            if (enemyDistance < closestDistance)
            {
                closestDistance = enemyDistance;
                closestLocation = location;
            }
        }

        return closestLocation;
    }

    public Location RunAway(Location location, float distanceToEnemy, Soldier nearestEnemy)
    {

        Location initial = new Location
        {
            X = Mathf.RoundToInt(soldier.transform.position.x * 10) + soldier.border,
            Y = Mathf.RoundToInt(soldier.transform.position.y * 10) + soldier.border,
        };

        List<Location> bestPath = new List<Location>();

        bestPath = soldier.A_estrella_Coste(initial, location);

        return MostCenteredLocation(bestPath, nearestEnemy);
    }

    public float DistanceToEnemy(out Soldier nearestEnemy)
    {
        Location initial = new Location
        {
            X = Mathf.RoundToInt(soldier.transform.position.x * 10) + soldier.border,
            Y = Mathf.RoundToInt(soldier.transform.position.y * 10) + soldier.border,
        };

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(soldier.opositeTag);


        Soldier nearestSoldier = null;
        float nearestEnemyDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Soldier>())
            {
                Soldier sold = enemy.GetComponent<Soldier>();



                float enemyDistance = Vector2.Distance(enemy.transform.position, soldier.transform.position);

                if (enemyDistance < nearestEnemyDistance)
                {
                    nearestEnemyDistance = enemyDistance;
                    nearestSoldier = sold;
                }
            }
        }

        nearestEnemy = nearestSoldier;

        return nearestEnemyDistance;

    }

    public IEnumerator ResetCanCheckPath()
    {
        yield return new WaitForSeconds(0.5f);
        canCheckPath = true;

    }

    public Location FindSaveWay()
    {
        float maxDist = 0;
        Location bestPlace = null;
        Location bestPlaceSiHayPeligro = null;
        float maxDistSiHayPeligro = 0;
        List<Location> bestPath = new List<Location>();
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(soldier.opositeTag));
        List<GameObject> minions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Miniom"));


        foreach (GameObject minion in minions)
        {
            if (minion.GetComponent<Oso>())
            {
                if (minion.CompareTag(soldier.opositeTag))
                {
                    enemies.Add(minion);
                }
            }
            else if (minion.GetComponent<Pato>())
            {
                if (minion.CompareTag(soldier.opositeTag))
                {
                    enemies.Add(minion);
                }
            }
        }

        if (enemies.Count <= 0) return null;

        foreach (Location location in proposedEscapePoints)
        {
            if (!CheckPeligro(enemies, location))
            {

                //comprobar la distancia del enemigo más cercano
                float nearestEnemyDistance = float.MaxValue;

                foreach (GameObject enemy in enemies)
                {
                    float enemyDistance = Vector2.Distance(enemy.transform.position, new Vector2(location.X / 10f, location.Y / 10f));

                    if (enemyDistance < nearestEnemyDistance)
                    {
                        nearestEnemyDistance = enemyDistance;
                    }

                    //sumDist += bestPath.Count;



                }

                //calcular la ruta más rápida a esa location
                Location initial = new Location
                {
                    X = Mathf.RoundToInt(soldier.transform.position.x * 10) + soldier.border,
                    Y = Mathf.RoundToInt(soldier.transform.position.y * 10) + soldier.border,
                };

                bestPath = soldier.A_estrella_Coste(initial, location);


                //comprobar si hay peligro en el camino
                if (CheckPeligro(enemies, bestPath))
                {
                    //si hay peligro
                    //comprobar si entre las location con camino peligroso es la que tiene el enemigo más alejado
                    if (nearestEnemyDistance > maxDistSiHayPeligro)
                    {
                        maxDistSiHayPeligro = nearestEnemyDistance;
                        bestPlaceSiHayPeligro = location;
                    }

                }
                else
                {
                    //comprobar si entre las location sin camino peligroso es la que tiene el enemigo más alejado
                    if (nearestEnemyDistance > maxDist)
                    {
                        maxDist = nearestEnemyDistance;
                        bestPlace = location;
                    }
                }
            }
        }

        //si hay una location sin peligro y sin camino peligroso
        if (bestPlace != null)
        {
            //Debug.Log("Hay lugares seguros con caminos seguros");
            return bestPlace;
        }
        else
        {
            //si no hay una zona sin camino peligroso
            //comprobar si hay alguna zona con el final alejado del peligro
            if (bestPlaceSiHayPeligro != null)
            {

                //Debug.Log("Hay lugares seguros pero con caminos peligrosos");
                return bestPlaceSiHayPeligro;

            }
            else
            {
                //si no hay devolver null
                //Debug.Log("Devuelve Null, no hay lugares seguros");
                return null;
            }
        }
    }

    public bool CheckPeligro(List<GameObject> enemies, List<Location> bestPath)
    {
        bool hayPeligro = false;
        //foreach (Location casilla in bestPath)
        for (int i = 0; i < bestPath.Count; i += 1)
        {

            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Soldier>())
                {
                    Soldier sold = enemy.GetComponent<Soldier>();
                    if (sold.stats.AttackDistance > Vector2.Distance(enemy.transform.position, new Vector2(bestPath[i].X / 10f, bestPath[i].Y / 10f)))
                    {
                        hayPeligro = true;
                    }
                }
                else if (enemy.GetComponent<Oso>())
                {
                    if (0.4f > Vector2.Distance(enemy.transform.position, new Vector2(bestPath[i].X / 10f, bestPath[i].Y / 10f)))
                    {
                        hayPeligro = true;
                    }
                }
                else if (enemy.GetComponent<Pato>())
                {
                    if (0.2f > Vector2.Distance(enemy.transform.position, new Vector2(bestPath[i].X / 10f, bestPath[i].Y / 10f)))
                    {
                        hayPeligro = true;
                    }
                }
            }
        }


        return hayPeligro;
    }

    public bool CheckPeligro(List<GameObject> enemies, Location location)
    {
        bool hayPeligro = false;


        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Soldier>())
            {
                Soldier sold = enemy.GetComponent<Soldier>();
                if (sold.stats.AttackDistance > Vector2.Distance(enemy.transform.position, new Vector2(location.X / 10f, location.Y / 10f)))
                {
                    //Debug.Log("Hay peligro (Soldados)");
                    hayPeligro = true;
                }
            }
            else if (enemy.GetComponent<Oso>())
            {
                if (0.4f > Vector2.Distance(enemy.transform.position, new Vector2(location.X / 10f, location.Y / 10f)))
                {
                    hayPeligro = true;
                }
            }
            else if (enemy.GetComponent<Pato>())
            {
                if (0.2f > Vector2.Distance(enemy.transform.position, new Vector2(location.X / 10f, location.Y / 10f)))
                {
                    hayPeligro = true;
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
        Soldier.OnDamageDealed.RemoveListener(ChangeState);
    }
}
