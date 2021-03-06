﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hueso : MonoBehaviour {

    public string targetTag;
    public float daño = 10;
    private float speed = 0.4f;
    float timer;
    private SpriteRenderer sr;
    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        timer = 0.0f;
        sr = transform.GetComponentInChildren<SpriteRenderer>();
        AudioManager.PlaySound(AudioManager.Sound.HUESO);
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 );
        timer += Time.deltaTime;
        if (timer > 0.6f)
        {
            Destroy(this.gameObject);
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().GetDamage(daño);
            }
        }
    }
}
