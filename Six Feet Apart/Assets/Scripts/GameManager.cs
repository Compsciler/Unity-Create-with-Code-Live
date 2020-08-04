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
    private Camera mainCamera;
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

    private GenerateWalls generateWallsScript;
    private SpawnPeople spawnPeopleScript;
    private TileDisabler tileDisablerScript;

    internal Dictionary<GameObject, float> infectedPathDistances;  // Disabled for now
    public GameObject gameOverMenu;
    public AudioClip countdownEndSound;
    public float countdownEndSoundVolume;
    public AudioClip gameOverSound;
    public float gameOverSoundVolume;

    private int defaultGameMode = 0;

    [Header("Additional Game Settings")]
    [SerializeField] internal float infectionDeathDuration = 40f;
    [SerializeField] internal bool canHealthyHeal = false;
    [SerializeField] internal bool areSymptomsDelayed = false;
    [SerializeField] internal float symptomDelayTime = 15f;  // No effect if areSymptomsDelayed is false
    [SerializeField] internal bool isChangingBackgroundColor = true;
    public Color[] backgroundColors;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        mainCamera = Camera.main;
        generateWallsScript = spawnManager.GetComponent<GenerateWalls>();
        spawnPeopleScript = spawnManager.GetComponent<SpawnPeople>();
        tileDisablerScript = spawnManager.GetComponent<TileDisabler>();

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
            spawnPeopleScript.UpdateGameOverScoreText();
            AudioManager.instance.musicSource.Pause();
            AudioManager.instance.SFX_Source.PlayOneShot(gameOverSound, gameOverSoundVolume);
            Debug.Log("Game Over!");

            int newScore = spawnPeopleScript.CalculateScore();
            HighScoreLogger.instance.UpdateHighScore(newScore, false);
        }
    }

    IEnumerator<float> BeginAfterCountdown()
    {
        yield return Timing.WaitForSeconds(gameCountdownTime);
        generateWallsScript.BeginGeneration();
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
                generateWallsScript.wallTotal = 35;
                break;
            case 2:
                infectionDeathDuration = 35f;  // [15, 35] or [10, 40]?
                canHealthyHeal = true;
                areSymptomsDelayed = true;
                generateWallsScript.wallTotal = 35;  // 32?
                spawnPeopleScript.areWavesRandom = true;
                break;
            case 3:
                spawnPeopleScript.startRepeatRate = 15f;
                spawnPeopleScript.repeatRateDecrease = 1f;
                break;
            case 4:
                spawnPeopleScript.areSpawningMultiple = true;
                spawnPeopleScript.repeatRateDecreaseWaveInterval = 2;
                break;
            case 5:
                spawnPeopleScript.spawnPosListIndex = 1;
                break;
            case 6:
                generateWallsScript.wallTotal = 35;
                tileDisablerScript.tileSetEnables = new bool[]{false, true};
                break;
            case 7:
                infectionDeathDuration = 35f;
                canHealthyHeal = true;
                // areSymptomsDelayed = true;
                spawnPeopleScript.spawnPosListIndex = 2;
                // tileDisablerScript.tileSetEnables = new bool[] {true, false};
                generateWallsScript.wallTotal = 40;

                spawnPeopleScript.startRepeatRate = 25f;
                spawnPeopleScript.repeatRateDecreaseWaveInterval = 4;
                // spawnPeopleScript.areWavesRandom = true;
                spawnPeopleScript.randomWaveGroupSize = 8;
                spawnPeopleScript.areSpawningMultiple = true;
                spawnPeopleScript.multipleSpawnWaves = new int[4, 2]{{1, 0}, {1, 0}, {1, 0}, {2, 1}};
                break;
        }
        if (isChangingBackgroundColor)
        {
            mainCamera.backgroundColor = backgroundColors[gameMode];
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