using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private string[] map;

    public enum MapType { TEMPLE, HELL, PRISION};
    public MapType myMap;
    public GameObject[] Soldiers;
    public GameObject[] Collectables;
    public static bool onGame = false;
    public static List<int> players = new List<int>();
    public static List<int> enemies = new List<int>();
    public bool GamePaused = false;
    public GameObject LoseImage;    //Pantalla de derrota
    public GameObject VictoryImage; //Pantalla de victoria
    public GameObject PauseImage; //Pantalla de pausa
    public static int[] levels = new int[20];
    public Camera MainCamera;
    private bool CanSound = true;
    private int GreenTeamCount = 0;
    private int RedTeamCount = 0;
    public Text GreenTeamCountText;
    public Text RedTeamCountText;
    public Soldier Selected = null;
    

    public static UnityEvent OnCollectablePlaced = new UnityEvent();

    private void Start()
    {
        Resume();
        if (onGame)
        {
            VictoryImage.SetActive(false);
            LoseImage.SetActive(false);
            float posx = 1;
            float inc = 0;

            InvokeRepeating("DropCollectable", 2, 10f);

            foreach (int number in players)
            {
                GameObject soldier = Instantiate(Soldiers[number]);
                posx += inc * 0.2f * Mathf.Pow(-1, inc);
                soldier.transform.position = new Vector2(posx, 0.1f);
                soldier.tag = "Player";
                soldier.layer = 8;
                inc++;
                if (inc > 8)
                {
                    inc = 0;
                    posx = 1.1f;
                }
            }
            posx = 1;
            inc = 0;

            foreach (int number in enemies)
            {
                GameObject soldier = Instantiate(Soldiers[number]);
                posx += inc * 0.2f * Mathf.Pow(-1, inc);
                soldier.transform.position = new Vector2(posx, 1.9f);
                soldier.tag = "Enemy";
                soldier.layer = 9;
                inc++;
                if (inc > 8)
                {
                    inc = 0;
                    posx = 1.1f;
                }
            }
        }
        else
        {
            CancelInvoke("DropCollectable");
        }

    }

    // Update is called once per frame
    void Update()
    {
        map = UpdateMap();
        if (onGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GamePaused)
                    Resume();
                else
                    Pause();
            }
            
            if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
            {
                
                if (LoseImage != null)
                {
                    Time.timeScale = 0f;
                    LoseImage.SetActive(true); //Pierde
                    MainCamera.GetComponent<AudioSource>().enabled = false;
                    if (CanSound)
                    {
                        CanSound = false;
                        AudioManager.PlaySound(AudioManager.Sound.DERROTA);
                    }
                }
                    
            }
            else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                if (VictoryImage != null)
                {
                    Time.timeScale = 0f;
                    VictoryImage.SetActive(true); //Gana
                    MainCamera.GetComponent<AudioSource>().enabled = false;
                    if (CanSound)
                    {
                        CanSound = false;
                        AudioManager.PlaySound(AudioManager.Sound.VICTORIA);
                    }
                }
            }
        }
    }

    private void DropCollectable()
    {
        int X = 0;
        int Y = 0;
        while (map[X][Y] == 'X')
        {
            X = Random.Range(1, 19);
            Y = Random.Range(1, 19);
        }

        GameObject Collectable = Instantiate(Collectables[Random.Range(0, Collectables.Length)]);
        Collectable.transform.position = new Vector2(X/10f, Y/10f);
        OnCollectablePlaced.Invoke();

    }

    public void Resume()
    {
        if (PauseImage)
            PauseImage.SetActive(false);
        if (MainCamera)
            MainCamera.GetComponent<AudioSource>().UnPause();
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause()
    {
        PauseImage.SetActive(true);
        MainCamera.GetComponent<AudioSource>().Pause();
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void MainMenuScene()
    {
        GameManager.LoadScene("MainTitle");
    }

    public void SelectionScene()
    {
        onGame = false;
        GameManager.LoadScene("Selection");
        Resume();
    }

    public void LevelsScene()
    {
        onGame = false;
        GameManager.LoadScene("Levels");
        Resume();
    }

    public void LoadLevelScene(int level)
    {
        players = new List<int>();
        enemies = new List<int>();
        GameManager.LoadScene("Level" + level);
        onGame = true;
    }

    public void ReloadLevel()
    {
        GameManager.ReLoadLevel();
    }

    public void Portada()
    {
        GameManager.LoadScene("Portada");
    }

    public void Clear()
    {
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Player"))
        {
            persona.tag = "Selectable";
            persona.GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            persona.tag = "Selectable";
            persona.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ChangeGreenCount(int change)
    {
        if (GreenTeamCount + change >= 0 && GreenTeamCount + change + RedTeamCount <= 16)
        {
            GreenTeamCount += change;
        }
        GreenTeamCountText.text = "" + GreenTeamCount;
    }

    public void ChangeRedCount(int change)
    {
        if (RedTeamCount + change >= 0 && GreenTeamCount + change + RedTeamCount <= 16)
        {
            RedTeamCount += change;
        }
        RedTeamCountText.text = "" + RedTeamCount;
    }

    public void RandomizeTeams()
    {
        Clear();
        List<GameObject> personas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Selectable"));
        int nEnemigos = RedTeamCount;
        if (RedTeamCount == 0)
        {
            nEnemigos = Random.Range(1, personas.Count - GreenTeamCount);
        }
        
        for (int i = 0; i < nEnemigos; i++)
        {
            int ind = Random.Range(0, personas.Count);
            personas[ind].tag = "Enemy";
            personas[ind].GetComponent<SpriteRenderer>().color = new Color32(255, 83, 83, 255);
            personas.Remove(personas[ind]);

        }
        personas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Selectable")); ;

        int nPlayers = GreenTeamCount;
        if (GreenTeamCount == 0)
        {
            nPlayers = Random.Range(1, personas.Count);
        }
        for (int i = 0; i < nPlayers; i++)
        {
            int ind = Random.Range(0, personas.Count);
            personas[ind].tag = "Player";
            personas[ind].GetComponent<SpriteRenderer>().color = new Color32(129, 255, 133, 255);
            personas.Remove(personas[ind]);
        }
    }

    public void StartGameScene()
    {
        players = new List<int>();
        enemies = new List<int>();
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(persona.GetComponent<Selectable>().myNumber);
        }
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(persona.GetComponent<Selectable>().myNumber);
        }


        if (players.Count > 0 && enemies.Count > 0)
        {
            onGame = true;
            GameManager.LoadScene("CustomizedBattle");
        }
            
    }


    public void AvtivarIA()
    {
        if (Selected != null)
        {
            Selected.IA = true;
        }
    }

    public void DesvtivarIA()
    {
        if (Selected != null)
        {
            Selected.IA = false;
            Selected.state = null;
        }
    }

    public void IAParaTodosHijosDePuta()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<Soldier>().IA = true;
        }
    }


    public string[] GetMap()
    {
        return map;
    }

    private string[] GetOriginalMap()
    {

        switch (myMap)
        {
            case MapType.TEMPLE:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "X   X    XX X   X   X",
                        "X   X           X   X",
                        "X   X    XX X   X   X",
                        "X   X    X  X   X   X",
                        "X        X  X       X",
                        "X   X    X  X   X   X",
                        "X   XX  XX   X  X   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X                   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X   XX  XX   X  X   X",
                        "X   X    X  X   X   X",
                        "X        X  X       X",
                        "X   X    X  X   X   X",
                        "X   X    XX X   X   X",
                        "X   X           X   X",
                        "X   X    XX X   X   X",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            case MapType.HELL:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXX   XXXXXXXXX",
                        "XXXXXXX      XXXXXXXX",
                        "XXXXX          XXXXXX",
                        "XXXX            XXXXX",
                        "XXXX    X   X    XXXX",
                        "XXX   XXX   XX   XXXX",
                        "XXX   XXX   XXX   XXX",
                        "XX   XXX     XX   XXX",
                        "X                   X",
                        "X                   X",
                        "X                   X",
                        "XX   XXX     XX   XXX",
                        "XXX   XXX   XXX   XXX",
                        "XXX   XXX   XX   XXXX",
                        "XXXX    X   X    XXXX",
                        "XXXX            XXXXX",
                        "XXXXX          XXXXXX",
                        "XXXXXXX      XXXXXXXX",
                        "XXXXXXXXX   XXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            case MapType.PRISION:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "X  XXXXXX XXXXX   XXX",
                        "X  XXXXX  XX        X",
                        "X   XXXXX  X  X   X X",
                        "X   XXXXX  X  XXXXX X",
                        "X  XXXXXX  X  XXXXX X",
                        "X          X        X",
                        "X          X        X",
                        "X                   X",
                        "X                   X",
                        "X                   X",
                        "X          X        X",
                        "X          X        X",
                        "X  XXXXXX  XXXXXXXX X",
                        "X   XXXXX  XXXXXXXX X",
                        "X   X XXX  XXXXXXXX X",
                        "X     XXX  XXXXXXXX X",
                        "X  XX     XXXXXXXXX X",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            default:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "X   X    XX X   X   X",
                        "X   X           X   X",
                        "X   X    XX X   X   X",
                        "X   X    X  X   X   X",
                        "X        X  X       X",
                        "X   X    X  X   X   X",
                        "X   XX  XX   X  X   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X                   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X   XX  XX   X  X   X",
                        "X   X    X  X   X   X",
                        "X        X  X       X",
                        "X   X    X  X   X   X",
                        "X   X    XX X   X   X",
                        "X   X           X   X",
                        "X   X    XX X   X   X",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
        }
        /*return new string[]
        {  //0         1         2      
           //012345678901234567890
            "XXXXXXXXXXXXXXXXXXXXX",
            "X                   X",
            "X                   X",
            "X                   X",
            "XXXXX XX     XX XXXXX",
            "X       XX XX       X",
            "X       XX XX       X",
            "X      X     X      X",
            "XX XXXX       XXXX XX",
            "X                   X",
            "XX X             X XX",
            "XX XXXXX     XXXXX XX",
            "X      XXX XXX      X",
            "X       XX XX       X",
            "X       XX XX       X",
            "X      XXX XXX      X",
            "XXXXX XX     XX XXXXX",
            "X                   X",
            "X                   X",
            "X                   X",
            "XXXXXXXXXXXXXXXXXXXXX",
        };*/



        //return new string[]
        //{  //0         1         2      
        //     //012345678901234567890
        //    "XXXXXXXXXXXXXXXXXXXXX",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "XXXXXXXXXXXXXXXXXXXXX",
        //};






    }

    private string[] UpdateMap()
    {
        string[] mapa = GetOriginalMap();
        List<GameObject> gente = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject soldier in GameObject.FindGameObjectsWithTag("Player"))
        {
            gente.Add(soldier);
        }
        foreach (GameObject soldier in GameObject.FindGameObjectsWithTag("Miniom"))
        {
            gente.Add(soldier);
        }

        foreach (GameObject enemy in gente)
        {
            int X = Mathf.RoundToInt(enemy.transform.position.x * 10f);
            int Y = Mathf.RoundToInt(enemy.transform.position.y * 10f);
            mapa = TransformMap(mapa, X, Y);
        }
        return mapa;
    }

    private string[] TransformMap(string[] map, int x, int y)
    {
        string[] newMap = new string[21];
        char[] newRowMap = new char[21];
        StringBuilder sb;
        for (int i = 0; i < 21; i++)
        {
            sb = new StringBuilder();
            for (int j = 0; j < map[0].Length; j++)
            {
                if (i == x && j == y)
                    newRowMap[j] = 'X';
                else
                    newRowMap[j] = map[i][j];
            }
            sb.Append(newRowMap);
            newMap[i] += sb;
        }
        return newMap;
    }
}
