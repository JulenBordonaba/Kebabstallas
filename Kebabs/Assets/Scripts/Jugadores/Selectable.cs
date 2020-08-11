﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
    
    private SpriteRenderer sr;
    public int myNumber;

    // Use this for initialization
    void Start () {
        sr = transform.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        transform.localScale *= 1.3f;
    }

    private void OnMouseExit()
    {
        transform.localScale /= 1.3f;
    }

    private void OnMouseDown()
    {

        switch (tag)
        {
            case "Player":
                {
                    sr.color = sr.color = new Color32(255, 83, 83, 255);
                    tag = "Enemy";
                    break;
                }
            case "Selectable":
                {
                    sr.color = new Color32(129, 255, 133, 255);
                    tag = "Player";
                    break;
                }
            default:
                {
                    sr.color = Color.white;
                    tag = "Selectable";
                    break;
                }
        }
    }
}
