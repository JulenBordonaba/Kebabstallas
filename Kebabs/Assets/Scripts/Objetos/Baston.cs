using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baston : MonoBehaviour {

    
    private float timer = 0;
    public GameObject owner;
    private Vector3 start;
    private bool vuelta = true;
    // Use this for initialization
    void Start()
    {
        //sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    void Awake()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 + 5);
        timer += Time.deltaTime;
        if (timer > 2.3f)
        {
            Destroy(this.gameObject);
        }
        else if (timer > 1.6f)
        {
            if (vuelta)
            {
                vuelta = false;
                start = transform.position;
            }
            float t = (timer - 1.6f) / 0.7f;
            if (owner != null)
                transform.position = Vector3.Lerp(start, owner.transform.position, t);
        }
    }

    
}
