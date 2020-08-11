using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llama : MonoBehaviour
{
    public string targetTag;
    public float daño = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.back, 150f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().GetDamage(daño);
            }else if (other.GetComponent<Pato>() != null)
            {
                other.GetComponent<Pato>().RecibirDaño(daño);
            }
        }
    }
}
