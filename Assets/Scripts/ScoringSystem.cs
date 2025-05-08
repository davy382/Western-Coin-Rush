using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public GameObject scoreText;
    public static int theScore;
    public string LvlGoal;
    //public AudioSource collectSound;


    private void Start()
    {
        theScore = 0;
    }

    void Update()
    {
        //collectSound.Play();
        //theScore += 1;
        scoreText.GetComponent<Text>().text = "" + theScore + "/" + LvlGoal;

    }


}
