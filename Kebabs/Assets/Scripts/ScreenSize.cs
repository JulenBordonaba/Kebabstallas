using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    public const float res = 320f / 512f;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = res/ screenAspectRatio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 screenRes
    {
        get { return new Vector2(Screen.width, Screen.height); }
    }

    public float screenAspectRatio
    {
        get { return screenRes.x / screenRes.y; }
    }
}
