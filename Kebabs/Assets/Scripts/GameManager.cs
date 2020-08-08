using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;

public static class GameManager {
    
    public static void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static void Selection()
    {
        SceneManager.LoadScene(1);
    }

    public static void CustomedBattle()
    {
        SceneManager.LoadScene(2);
    }

    public static void Levels()
    {
        SceneManager.LoadScene(3);
    }

    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene(level + 4);
    }
}
