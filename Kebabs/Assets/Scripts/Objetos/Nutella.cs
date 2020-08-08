using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nutella : MonoBehaviour {

    public string targetTag;
    private SpriteRenderer sr;
    private float timer = 0;
    // Use this for initialization
    void Start()
    {
        sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    void Awake()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x * 10) * 0.1f, Mathf.RoundToInt(transform.position.y * 10) * 0.1f);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 - 50);
        timer += Time.deltaTime;
        if (timer > 15f)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
            {

                if (Vector2.Distance(enemy.transform.position, transform.position) < 0.1f && enemy.GetComponent<Soldier>() != null)
                {

                    enemy.GetComponent<Soldier>().GetOriginalSpeed();
                }
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().speed = 0.1f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().GetOriginalSpeed();
            }
        }
    }
}
