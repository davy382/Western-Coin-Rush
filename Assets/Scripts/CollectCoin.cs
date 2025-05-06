using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectCoin : MonoBehaviour
{

    //public AudioSource collectSound;
    public int scoreAmount;
    public float spinSpeed = 90f;


    private void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //collectSound.Play();
            ScoringSystem.theScore += scoreAmount;
            Destroy(gameObject);
        }
    }
}