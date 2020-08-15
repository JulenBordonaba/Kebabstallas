using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class DontDestroy : MonoBehaviour
{

    private string a;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

    }

    void Update()
    {
        a = SceneManager.GetActiveScene().name;
        if (!(a == "MainTitle" || a =="Selection" || a=="Levels" || a=="Portada"))
        {
            Destroy(this.gameObject);
        }
    }
}
