﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class GameController : MonoBehaviour {
    private string[] map;
    public GameObject[] Soldiers;
    public GameObject[] Collectables;
    public static bool onGame = false;
    public static List<int> players = new List<int>();
    public static List<int> enemies = new List<int>();
    public bool GamePaused = false;
    public GameObject LoseImage;    //Pantalla de derrota
    public GameObject VictoryImage; //Pantalla de victoria
    public static int[] levels = new int[20];

    private void Start()
    {
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
                }
                    
            }
            else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                if (VictoryImage != null)
                {
                    Time.timeScale = 0f;
                    VictoryImage.SetActive(true); //Gana
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

        GameObject Collectable = Instantiate(Collectables[Random.Range(0, Collectables.Length - 1)]);
        Collectable.transform.position = new Vector2(X/10f, Y/10f);
    }

    public void Resume()
    {
        //PausedMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void MainMenuScene()
    {
        GameManager.MainMenu();
    }

    public void SelectionScene()
    {
        onGame = false;
        GameManager.Selection();
        Resume();
    }

    public void LevelsScene()
    {
        onGame = false;
        GameManager.Levels();
        Resume();
    }

    public void LoadLevelScene(int level)
    {
        players = new List<int>();
        enemies = new List<int>();
        GameManager.LoadLevel(level);
        onGame = true;
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
            GameManager.CustomedBattle();
        }
            
    }

    public string[] GetMap()
    {
        return map;
    }

    private string[] GetOriginalMap()
    {
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

    private string[] UpdateMap()
    {
        string[] mapa = GetOriginalMap();
        List<GameObject> gente = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject soldier in GameObject.FindGameObjectsWithTag("Player"))
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
