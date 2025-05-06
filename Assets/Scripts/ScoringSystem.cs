using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public GameObject scoreText;
    public static int theScore;
    //public AudioSource collectSound;


    void Update()
    {
        //collectSound.Play();
        //theScore += 1;
        scoreText.GetComponent<Text>().text = "Score: " + theScore;

    }


}
