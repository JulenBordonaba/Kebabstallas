using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {


    public enum Type
    { EXPLOSION }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Soldier>() != null)
        {
            var TargetTag = other.GetComponent<Soldier>().opositeTag;
        }
        
    }
}
