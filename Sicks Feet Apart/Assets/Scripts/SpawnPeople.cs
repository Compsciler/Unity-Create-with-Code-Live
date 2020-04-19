using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPeople : MonoBehaviour
{
    public GameObject healthyPerson;
    public GameObject infectedPerson;
    public Vector3[] spawnPosList;  // Note: Unsymmetrical with 1.0 radius bake with +1.0 on one side and +1.33 on other

    public int infectedWaveInterval = 5;
    public float startDelay = 0f;
    public float startRepeatRate = 25f;
    public int repeatRateDecreaseWaveInterval = 5;
    public float repeatRateDecrease = 2f;
    public float minimumRepeatRate = 5f;
    // This would produce 0s, 25x4, 23x5, 21x5, 19x5, ... , 9x5, 7x5, 5x5, 5x5...
    private float timer;
    private float repeatRate;
    private int wave = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = startDelay;
        repeatRate = startRepeatRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            wave++;
            if (wave % infectedWaveInterval == 0)
            {
                SpawnPerson(true);
            } else
            {
                SpawnPerson(false);
            }
            if (wave % repeatRateDecreaseWaveInterval == 0)
            {
                repeatRate -= repeatRateDecrease;
                if (repeatRate < minimumRepeatRate)
                {
                    repeatRate = minimumRepeatRate;
                }
            }
            timer = repeatRate;
            Debug.Log(Time.time);
        }
        timer -= Time.deltaTime;
    }

    void SpawnPerson(bool isInfected)
    {
        Vector3 spawnPos = spawnPosList[Random.Range(0, spawnPosList.Length)];
        if (isInfected)
        {
            Instantiate(infectedPerson, spawnPos, infectedPerson.transform.rotation);
        }
        else
        {
            Instantiate(healthyPerson, spawnPos, healthyPerson.transform.rotation);
        }
    }
}
