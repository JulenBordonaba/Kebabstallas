using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiaAzul : MonoBehaviour {

    public string targetTag = "Player";
    public float daño = 120;
    private float timer = 0;
    


    void Awake()
    {
        
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > 1.1f)
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
