using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int scoreGoal;
    public GameObject winCanvas;
    private bool levelComplete = false;

    public GameObject player;
    public Player pc;
    public string nextSceneName;

    void Awake()
    {
        if (!gm)
            gm = this;
        else
            Destroy(this);
    }

    void Start()
    {
        if (player != null)
            pc = player.GetComponent<Player>();

        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    void Update()
    {
        if (!levelComplete && ScoringSystem.theScore >= scoreGoal)
        {
            levelComplete = true;
            LevelPassed();
        }
    }

    void LevelPassed()
    {
        Time.timeScale = 0f;
        winCanvas.SetActive(true);
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is empty!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}
