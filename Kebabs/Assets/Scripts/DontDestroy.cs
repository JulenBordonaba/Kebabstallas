using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class DontDestroy : MonoBehaviour
{

    private string a;
    private List<string> scenes = new List<string> { "MainTitle", "Selection", "Levels", "Portada", "Configuracion" };

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
        if (!scenes.Contains(a))
        {
            Destroy(this.gameObject);
        }
    }
}
