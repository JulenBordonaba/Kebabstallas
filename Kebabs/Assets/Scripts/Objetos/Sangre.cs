using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sangre : MonoBehaviour {
    private float timer = 0;
    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            Destroy(this.gameObject);
        }
    }
}
