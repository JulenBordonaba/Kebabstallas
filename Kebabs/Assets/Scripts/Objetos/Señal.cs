using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Señal : MonoBehaviour {

    public string targetTag;
    public float daño = 5;
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
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 + Vector2.Angle(Vector2.up, transform.position)*0.01f + 0.1f );
        timer += Time.deltaTime;
        if (timer > 0.6f)
        {
            Destroy(this.gameObject);
        }else if (timer > 0.3f)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().RecibirDaño(daño);
            }
            else if (other.GetComponent<Pato>() != null)
            {
                other.GetComponent<Pato>().RecibirDaño(daño);
            }
        }
    }
}
