using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuevoPersonaje : MonoBehaviour
{

    private SpriteRenderer sr;
    private float timer;
    public GameObject Nombre;

    // Start is called before the first frame update
    void Awake()
    {
        sr = transform.GetComponent<SpriteRenderer>();
        sr.color = Color.black;
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 5){
            
            if (timer > 2)
            {
                
                float t = (timer - 2) / 3;
                sr.color = Color.Lerp(Color.black, Color.white, t);
            }
        }else if(timer > 7)
        {
            transform.GetComponent<Animator>().enabled = true;
            Nombre.SetActive(true);
        }
        
    }
}
