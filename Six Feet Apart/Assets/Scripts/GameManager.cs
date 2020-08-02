using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    internal static GameManager instance;
    [SerializeField] internal bool isGameActive = true;  // Time.timeScale will be used to check for pausing  // Change to false during start countdown?
    internal bool isGameActivePreviousFrame = true;
    // private float gameTimer = 0;  // Could use Time.timeSinceLevelLoad with Time.timeScale instead for simple functionality
    public bool isUsingGameOver = true;
    public float infectionSpreadRate = 3f;
    internal bool hasGameStarted = false;
    public float gameCountdownTime = 3f;
    public GameObject spawnManager;
    public GameObject countdownGameMask;
    public GameObject[] disableAfterCountdown;

    internal Dictionary<GameObject, float> infectedPathDistances;  // Disabled for now
    public GameObject gameOverMenu;
    public AudioClip countdownEndSound;
    public float countdownEndSoundVolume;
    public AudioClip gameOverSound;
    public float gameOverSoundVolume;

    // internal static int gameMode;  // Probably shouldn't make this static, but this is just for accessing from MainMenu scene
    private int defaultGameMode = 0;

    [Header("Game Settings")]
    [SerializeField] internal float infectionDeathDuration = 40f;
    [SerializeField] internal bool canHealthyHeal = false;
    [SerializeField] internal bool areSymptomsDelayed = false;
    [SerializeField] internal float symptomDelayTime = 15f;  // No effect if areSymptomsDelayed is false

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        try  // When starting game in GameScene scene
        {
            ApplyGameModeSettings(HighScoreLogger.instance.gameMode);
        }
        catch (NullReferenceException)
        {
            ApplyGameModeSettings(defaultGameMode);
        }

        // Timing.RunCoroutine(InfectionSpread());
        infectedPathDistances = new Dictionary<GameObject, float>();
        countdownGameMask.SetActive(true);
        Timing.RunCoroutine(BeginAfterCountdown());

        // SceneManager.sceneUnloaded -= OnSceneUnloaded;  // Why can't the delegate be reset here?
        SceneManager.sceneUnloaded += OnSceneUnloaded;  // Adding OnSceneUnloaded() to delegate call when scene unloaded
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            /*
            float minPathDistance = float.MaxValue;
            GameObject closestInfected = null;
            foreach (GameObject infected in infectedPathDistances.Keys)  // Remove dictionary if only used here
            {
                float pathDistance = infected.GetComponent<PersonController>().hospitalTileDistance;
                if (pathDistance < minPathDistance)
                {
                    minPathDistance = pathDistance;
                    closestInfected = infected;
                }
                // infectedPathDistances[infected] = infected.GetComponent<PersonController>().hospitalTileDistance;
            }
            // gameTimer += Time.deltaTime;
            */
        }
    }
    void LateUpdate()
    {
        if (isGameActivePreviousFrame != isGameActive)
        {
            Debug.Log("isGameActive was set to false");
        }
        isGameActivePreviousFrame = isGameActive;  // isGameActive set to false in GameOver(), which happens from InfectionProcess() coroutine; hopefully coroutines always execute before LateUpdate()
    }

    public void GameOver()
    {
        if (isUsingGameOver)
        {
            // JUST BEFORE REVIVE SCREEN
            isGameActive = false;
            Timing.PauseCoroutines();  // Not perfect solution if second chance used, hopefully no coroutines will be used during Game Over screen

            // GAME OVER SCREEN
            gameOverMenu.SetActive(true);
            spawnManager.GetComponent<SpawnPeople>().UpdateGameOverScoreText();
            AudioManager.instance.musicSource.Pause();
            AudioManager.instance.SFX_Source.PlayOneShot(gameOverSound, gameOverSoundVolume);
            Debug.Log("Game Over!");

            int newScore = spawnManager.GetComponent<SpawnPeople>().CalculateScore();
            HighScoreLogger.instance.UpdateHighScore(newScore, false);
        }
    }

    IEnumerator<float> BeginAfterCountdown()
    {
        yield return Timing.WaitForSeconds(gameCountdownTime);
        spawnManager.GetComponent<GenerateWalls>().BeginGeneration();
        hasGameStarted = true;
        AudioManager.instance.SFX_Source.PlayOneShot(countdownEndSound, countdownEndSoundVolume);
        AudioManager.instance.musicSource.enabled = true;
        foreach (GameObject go in disableAfterCountdown)
        {
            go.SetActive(false);
        }
    }

    IEnumerator<float> InfectionSpread()  // PauseCoroutine() if game paused
    {
        while (true)
        {
            GameObject[] infectedPeople = GameObject.FindGameObjectsWithTag("Infected");
            foreach (GameObject person in infectedPeople)
            {
                GameObject infectionCylinder = person.transform.GetChild(0).gameObject;
                // infectionCylinder.GetComponent<InfectionCylinder>().expandRadius();
                Timing.RunCoroutine(infectionCylinder.GetComponent<InfectionCylinder>().ExpandRadius());  // Does not work well with SinusoidalRadius()
            }
            yield return Timing.WaitForSeconds(infectionSpreadRate);
        }
    }

    void ApplyGameModeSettings(int gameMode)
    {
        switch (gameMode)
        {
            case 0:
                break;
            case 1:
                canHealthyHeal = true;
                spawnManager.GetComponent<GenerateWalls>().wallTotal = 36;  // 35?
                break;
            case 2:
                infectionDeathDuration = 35f;  // [15, 35] or [10, 40]?
                canHealthyHeal = true;
                areSymptomsDelayed = true;
                spawnManager.GetComponent<GenerateWalls>().wallTotal = 32;  // 35?
                spawnManager.GetComponent<SpawnPeople>().areWavesRandom = true;
                break;
            case 3:
                spawnManager.GetComponent<SpawnPeople>().startRepeatRate = 15f;
                spawnManager.GetComponent<SpawnPeople>().repeatRateDecrease = 1f;
                break;
            case 4:
                spawnManager.GetComponent<SpawnPeople>().areSpawningMultiple = true;
                spawnManager.GetComponent<SpawnPeople>().repeatRateDecreaseWaveInterval = 2;
                break;
            case 5:
                spawnManager.GetComponent<SpawnPeople>().spawnPosListIndex = 1;
                break;
            case 6:
                spawnManager.GetComponent<GenerateWalls>().wallTotal = 35;
                spawnManager.GetComponent<TileDisabler>().tileSetEnables = new bool[]{false, true};
                break;
        }
    }
    
    public void ResetStaticVariables()
    {
        HospitalTile.isOccupied = false;
        HealProgressBar.isNewlyOccupied = false;
        HealProgressBar.isOccupiedByInfectedPerson = true;
        PersonController.infectedPeopleTotal = 0;
        Time.timeScale = 1;  // Resetting time scale when restarting or quitting game
        Debug.Log("Static variables reset!");
    }

    public void OnSceneUnloaded(Scene currentScene)
    {
        ResetStaticVariables();
        SceneManager.sceneUnloaded -= OnSceneUnloaded;  // Resets delegate
    }
}