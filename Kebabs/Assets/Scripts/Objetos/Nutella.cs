using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nutella : MonoBehaviour {

    public string targetTag;
    private SpriteRenderer sr;
    private float timer = 0;
    private List<GameObject> Nutellados = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    void Awake()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x * 10) * 0.1f, Mathf.RoundToInt(transform.position.y * 10) * 0.1f, 0.02f);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 - 50);
        timer += Time.deltaTime;
        if (timer > 15f)
        {
            foreach (GameObject enemy in Nutellados)
            {
                enemy.GetComponent<Soldier>().nutellaCount -= 1;
                enemy.GetComponent<Soldier>().CheckNutella();
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                Nutellados.Add(other.gameObject);
                other.GetComponent<Soldier>().nutellaCount += 1;
                other.GetComponent<Soldier>().CheckNutella();
                //other.GetComponent<Soldier>().stats.speedReplace = 0.1f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                Nutellados.Remove(other.gameObject);
                //other.GetComponent<Soldier>().stats.speedReplace=-1;
                other.GetComponent<Soldier>().nutellaCount -= 1;
                other.GetComponent<Soldier>().CheckNutella();
            }
        }
    }
}
