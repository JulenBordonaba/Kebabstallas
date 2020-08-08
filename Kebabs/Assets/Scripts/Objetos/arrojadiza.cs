using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrojadiza : MonoBehaviour {
    public Vector3 start;
    public Vector3 targetPos;
    public GameObject target;
    public string targetTag;
    public float daño = 10;
    float timer;
    private SpriteRenderer sr;
    [Range(0f,1f)]
    public float accuracy = 0.4f;
    // Use this for initialization


    void Awake () {
        start = transform.position;
        timer = 0.0f;
        sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20)+1;
        if (target != null)
        {
            timer += Time.deltaTime;
            if (timer < 1f)
            {
                if (timer < accuracy)
                {
                    targetPos = target.transform.position;
                }
                float t = timer / 1f;
                transform.position = Vector3.Lerp(start, targetPos, t);
            }
            else if (timer <1.65)
            {
                this.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                Destroy(this.gameObject);
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
                other.GetComponent<Soldier>().RecibirDaño(daño);
            }
            else if (other.GetComponent<Pato>() != null)
            {
                other.GetComponent<Pato>().RecibirDaño(daño);
            }
        }
    }



}
