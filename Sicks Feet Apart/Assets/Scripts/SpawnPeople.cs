using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    internal float timer;
    internal float repeatRate;
    internal bool isInfectedWave = false;
    private int wave = 0;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI gameOverScoreText;

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
            UpdateWaveText();
            if (isInfectedWave)
            {
                SpawnPerson(true);
            } else
            {
                SpawnPerson(false);
            }
            isInfectedWave = (wave % infectedWaveInterval == infectedWaveInterval - 1);
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
        if (GameManager.instance.isGameActive)
        {
            timer -= Time.deltaTime;
        }
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

    void UpdateWaveText()  // Include animation?
    {
        waveText.text = "Wave " + wave;
    }

    public void UpdateGameOverScoreText()
    {
        gameOverScoreText.text = "You reached Wave " + wave;
    }
}
