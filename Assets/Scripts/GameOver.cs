using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string  MainMenu = "MainMenu";

    public void PlayGame()
    {
        SceneManager.LoadScene(MainMenu);
    }
}
