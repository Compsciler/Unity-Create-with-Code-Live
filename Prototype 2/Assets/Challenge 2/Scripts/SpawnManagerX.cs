using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] ballPrefabs;

    private float spawnLimitXLeft = -22;
    private float spawnLimitXRight = 7;
    private float spawnPosY = 30;

    private float startDelay = 1.0f;
    private float spawnInterval = 4.0f;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        // InvokeRepeating("SpawnRandomBall", startDelay, spawnInterval);
        timer = startDelay;
    }

    void Update()
    {
        if (timer <= 0)
        {
            SpawnRandomBall();
            timer = Random.Range(0.5f, 2f);
        } 
        else
        {
            timer -= Time.deltaTime;
        }
        
        
    }

    // Spawn random ball at random x position at top of play area
    void SpawnRandomBall ()
    {
        // Generate random ball index and random spawn position
        Vector3 spawnPos = new Vector3(Random.Range(spawnLimitXLeft, spawnLimitXRight), spawnPosY, 0);

        // instantiate ball at random spawn location
        GameObject ballPrefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];
        Instantiate(ballPrefab, spawnPos, ballPrefab.transform.rotation);
    }

}
