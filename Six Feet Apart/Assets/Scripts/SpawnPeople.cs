using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.UIWidgets.foundation;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPeople : MonoBehaviour
{
    public GameObject healthyPerson;
    public GameObject healthyUnboundPerson;
    public GameObject infectedPerson;
    public Vector3[] spawnPosList;  // Note: Unsymmetrical with 1.0 radius bake with +1.0 on one side and +1.33 on other

    public int infectedWaveInterval = 5;
    public float startDelay = 0f;
    public float startRepeatRate = 25f;
    public int repeatRateDecreaseWaveInterval = 5;
    public float repeatRateDecrease = 2f;
    public float minimumRepeatRate = 5f;
    // This would produce 0s, 25x4, 23x5, 21x5, 19x5, ... , 9x5, 7x5, 5x5, 5x5...
    public bool areWavesRandom = false;
    public int infectedWavesPerGroup = 2;
    public int randomWaveGroupSize = 10;
    private List<int> randomInfectedWaves = new List<int>();

    internal float timer;
    internal float repeatRate;
    internal bool isInfectedWave = false;
    private int wave = 0;

    public TMP_Text waveText;
    public TMP_Text gameOverScoreText;
    public TMP_Text unlockedModeText;

    public AudioClip spawnSound;
    public float spawnSoundVolume;
    public bool isFirstSpawnSoundDisabled;

    // Start is called before the first frame update
    void Start()
    {
        timer = startDelay;
        repeatRate = startRepeatRate;

        if (areWavesRandom)
        {
            UpdateRandomIsInfectedWave();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
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
            if (!isFirstSpawnSoundDisabled || wave != 1)
            {
                AudioManager.instance.SFX_Source.PlayOneShot(spawnSound, spawnSoundVolume);
            }
            if (areWavesRandom)  // Put at the end to make it easier for WaveProgressBar.cs
            {
                UpdateRandomIsInfectedWave();
            }
            else
            {
                isInfectedWave = (wave % infectedWaveInterval == infectedWaveInterval - 1);
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
            // Debug.Log(Time.time);
        }
        if (GameManager.instance.isGameActive && GameManager.instance.hasGameStarted)
        {
            timer -= Time.deltaTime;
        }
    }

    void SpawnPerson(bool isInfected)
    {
        Vector3 spawnPos = spawnPosList[Random.Range(0, spawnPosList.Length)];
        GameObject person;
        if (isInfected)
        {
            person = Instantiate(infectedPerson, spawnPos, infectedPerson.transform.rotation);
        }
        else
        {
            if (GameManager.instance.canHealthyHeal)
            {
                person = Instantiate(healthyUnboundPerson, spawnPos, healthyUnboundPerson.transform.rotation);
            }
            else
            {
                person = Instantiate(healthyPerson, spawnPos, healthyPerson.transform.rotation);
            }
        }
        person.name = "Person " + wave;
    }
    void RandomizeInfectedWaves()
    {
        List<int> nextWaveGroup = Enumerable.Range(wave + 1, randomWaveGroupSize).ToList();
        // Debug.Log(ExtensionMethods.ListToString(nextWaveGroup));
        for (int i = 0; i < infectedWavesPerGroup; i++)
        {
            int randomIndex = Random.Range(0, nextWaveGroup.Count);
            randomInfectedWaves.Add(nextWaveGroup[randomIndex]);
            nextWaveGroup.RemoveAt(randomIndex);
        }
        randomInfectedWaves.Sort();
        // Debug.Log(ExtensionMethods.ListToString(randomInfectedWaves));
    }
    void UpdateRandomIsInfectedWave()
    {
        if (wave % randomWaveGroupSize == 0)
        {
            RandomizeInfectedWaves();
        }
        isInfectedWave = (randomInfectedWaves.Count > 0 && randomInfectedWaves[0] == (wave + 1));
        if (isInfectedWave)
        {
            randomInfectedWaves.RemoveAt(0);
        }
    }
    void UpdateWaveText()  // Include animation?
    {
        waveText.text = "Wave " + wave;
    }

    public void UpdateGameOverScoreText()
    {
        // gameOverScoreText.text = "Your score is " + CalculateScore();
        gameOverScoreText.text = "You reached Wave " + wave;
    }

    public int CalculateScore()
    {
        return wave;  // Different for Breakneck Mode?
    }

    public void UpdateUnlockedModeText(int prevHighScore)
    {
        //string debugString = "(" + prevHighScore + ")";
        int[] highScores = HighScoreLogger.instance.GetHighScores();

        List<int> unlockedModes = new List<int>();
        for (int i = 0; i < DifficultySelectMenu.gameModeUnlockReqs.Length; i++)
        {
            int[,] currentUnlockReqs = DifficultySelectMenu.gameModeUnlockReqs[i];
            bool currentUnlockReqsMet = true;
            bool requiresCurrentModeAndScore = false;
            // debugString += " <" + i + ">";
            for (int j = 0; j < currentUnlockReqs.Length / 2; j++)  // Foreach loop doesn't work somehow, probably because C# Length property returns total number of integers in array
            {
                // int newScore = wave;
                int minScoreReq = currentUnlockReqs[j, 1];
                // debugString += "[" + j + "]";
                if (currentUnlockReqs[j, 0] == HighScoreLogger.instance.gameMode)
                {
                    int newScore = wave;
                    if (newScore >= minScoreReq && prevHighScore < minScoreReq)
                    {
                        requiresCurrentModeAndScore = true;
                    }
                    // debugString += "A: " + minScoreReq + ", " + requiresCurrentModeAndScore;
                }
                else
                {
                    int highScore = highScores[currentUnlockReqs[j, 0]];
                    if (highScore < minScoreReq)
                    {
                        currentUnlockReqsMet = false;
                    }
                    // debugString += "B: " + minScoreReq + ", " + currentUnlockReqsMet;
                }
            }
            if (currentUnlockReqsMet && requiresCurrentModeAndScore)
            {
                unlockedModes.Add(i);
                // debugString += "|" + i;
            }
        }
        if (unlockedModes.Count > 0)
        {
            // debugString += "====" + ExtensionMethods.ListToString(unlockedModes) + "====";
            unlockedModeText.gameObject.SetActive(true);
            if (unlockedModes.Count == 1)
            {
                string unlockedModeName = HighScoreLogger.instance.highScoreStrings[unlockedModes[0] - 1].Replace("HighScore", "");
                unlockedModeText.text = "You have unlocked " + unlockedModeName + " Mode!";
            }
            else
            {
                unlockedModeText.text = "You have unlocked " + unlockedModes.Count + " game modes!";
            }
        }
        // Debug.Log(debugString);
    }
}
