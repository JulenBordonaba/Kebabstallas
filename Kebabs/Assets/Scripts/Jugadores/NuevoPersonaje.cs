using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuevoPersonaje : MonoBehaviour
{

    private SpriteRenderer sr;
    private float timer;
    public GameObject Nombre;
    private float startTime;

    // Start is called before the first frame update
    void Awake()
    {
        startTime = Random.Range(1f, 4f);
        sr = transform.GetComponent<SpriteRenderer>();
        sr.color = Color.black;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < startTime + 2){
            
            if (timer > startTime)
            {
                
                float t = (timer - startTime) / 2;
                sr.color = Color.Lerp(Color.black, Color.white, t);
            }
        }else if(timer > 5)
        {
            transform.GetComponent<Animator>().enabled = true;
            //Nombre.SetActive(true);
        }
        
    }
}
