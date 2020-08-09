using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fart : MonoBehaviour
{
    public string targetTag;
    private float daño = 15;
    float timer;
    private SpriteRenderer sr;

    void Awake()
    {
        timer = 0.0f;
        sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 + Vector2.Angle(Vector2.up, transform.position) * 0.01f + 0.1f);
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            Destroy(this.gameObject);
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
        }
    }
}
