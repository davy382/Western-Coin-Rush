using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinAmount = 5;
    public Transform spawnPointParent; // Parent containing all spawn point GameObjects

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        // Get all child spawn points
        List<Transform> availableSpawnPoints = new List<Transform>();
        foreach (Transform child in spawnPointParent)
        {
            availableSpawnPoints.Add(child);
        }

        // Randomly spawn coins at selected points
        for (int i = 0; i < coinAmount && availableSpawnPoints.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform chosenPoint = availableSpawnPoints[randomIndex];

            Instantiate(coinPrefab, chosenPoint.position, Quaternion.identity);
            availableSpawnPoints.RemoveAt(randomIndex); // avoid duplicates
        }
    }
}
