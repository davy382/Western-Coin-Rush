using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CountDownTimer : MonoBehaviour
{
    public Text timer; 
    public float time;
    private string loadLevel;
    public string GameOver = "GameOver";

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;

            string minutes = Mathf.Floor(time / 60).ToString("00");
            string seconds = Mathf.Floor(time % 60).ToString("00");
            string fraction = Mathf.Floor((time * 100) % 100).ToString("00");
            timer.text = "" + minutes + ":" + seconds + ":" + fraction;
        }
        else
        {
            SceneManager.LoadScene(GameOver);
        }
    }
}
