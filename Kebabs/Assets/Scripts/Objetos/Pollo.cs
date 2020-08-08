using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollo : MonoBehaviour {
    public string targetTag;
    public float daño = 5;
    public Vector3 direccion;
    private float speed = 0.015f;
    float timer;
    private SpriteRenderer sr;
    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        timer = 0.0f;
        sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20);
        timer += Time.deltaTime;
        if (timer > 0.6f)
        {
            Destroy(this.gameObject);
        }
        if (direccion != null)
        {
            transform.Translate(direccion * speed);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().GetDamage(daño);
            }
            else if (other.GetComponent<Pato>() != null)
            {
                other.GetComponent<Pato>().RecibirDaño(daño);
            }
            Destroy(this.gameObject);
        }
    }
}
