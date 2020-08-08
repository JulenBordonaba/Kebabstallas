using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaCorazon : MonoBehaviour {

    public string targetTag;
    public float daño = 30;
    private SpriteRenderer sr;
    private float timer = 0;

    public Vector3 start;
    public Vector3 targetPos;


    void Awake()
    {
        sr = transform.GetComponentInChildren<SpriteRenderer>();
        timer = 0.0f;
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos != null)
        {
            timer += Time.deltaTime;
            if (timer < 0.6f)
            {
                float t = timer / 0.6f;
                transform.position = Vector3.Lerp(start, targetPos, t);
            }
            if (timer > 2.07f)
            {
                Destroy(this.gameObject);
            }
            else if (timer > 1.6)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            else if (timer > 1.5)
            {
                GetComponent<BoxCollider>().enabled = true;
            }
        }
        else
        {
            if (timer >= 0.001)
            {
                Destroy(this.gameObject);
            }
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
