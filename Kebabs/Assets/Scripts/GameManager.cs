using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;

public static class GameManager {
    
    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    public static void ReLoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
