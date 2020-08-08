using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampoDebilitador : MonoBehaviour {
    public string targetTag;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().defensa = 3;
            }
            else if (other.GetComponent<Pato>() != null)
            {
                other.GetComponent<Pato>().defensa = 3;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (other.GetComponent<Soldier>() != null)
            {
                other.GetComponent<Soldier>().defensa = 1;
            }
            else if (other.GetComponent<Pato>() != null)
            {
                other.GetComponent<Pato>().defensa = 1;
            }
        }
    }
}
