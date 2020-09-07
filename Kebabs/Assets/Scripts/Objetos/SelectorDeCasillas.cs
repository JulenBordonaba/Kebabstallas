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

        int X = Mathf.Clamp(Mathf.RoundToInt(v3.x * 10), 5, 15);
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

    
}
