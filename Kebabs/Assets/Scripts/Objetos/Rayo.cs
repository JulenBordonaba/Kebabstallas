using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayo : MonoBehaviour {
    public string targetTag;
    public float daño = 20;
    private SpriteRenderer sr;
    private float timer = 0;
    // Use this for initialization
    void Start () {
        sr = transform.GetComponentInChildren<SpriteRenderer>();
    }

    void Awake()
    {
        timer = 0.0f;
        AudioManager.PlaySound(AudioManager.Sound.RAYONEGRO);
    }

    // Update is called once per frame
    void Update () {
        sr.sortingOrder = Mathf.RoundToInt(100 - transform.position.y * 20 +5);
        timer += Time.deltaTime;
        if (timer > 0.6f)
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
