using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SelectorDeCasillas : MonoBehaviour {
    string[] map;
    private SpriteRenderer sr;
    GameController GC;
    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        GC = GetComponentInParent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        map = GC.GetMap();
        var v3 = Input.mousePosition;
        v3.z = 10.0f;
        v3 = Camera.main.ScreenToWorldPoint(v3);

        int X = Mathf.Clamp(Mathf.RoundToInt(v3.x * 10), 1, 19);
        int Y = Mathf.Clamp(Mathf.RoundToInt(v3.y * 10), 1, 19);

        this.transform.position = new Vector2(X/10f, Y/10f-0.02f);

        if (map[X][Y] == 'X')
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.green;
        }
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
            if (soldier != this.gameObject)
            {
                gente.Add(soldier);
            }
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
