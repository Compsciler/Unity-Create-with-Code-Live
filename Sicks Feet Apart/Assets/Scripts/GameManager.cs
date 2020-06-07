using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;

public class GameManager : MonoBehaviour
{
    internal static GameManager instance;
    [SerializeField] internal bool isGameActive = true;  // Time.timeScale will be used to check for pausing  // Change to false during start countdown
    internal bool isGameActivePreviousFrame = true;
    // private float gameTimer = 0;  // Could use Time.timeSinceLevelLoad with Time.timeScale instead for simple functionality
    public bool isUsingGameOver = true;
    public float infectionSpreadRate = 3f;

    internal Dictionary<GameObject, float> infectedPathDistances;  // Disabled for now
    public GameObject gameOverMenu;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // Timing.RunCoroutine(InfectionSpread());
        infectedPathDistances = new Dictionary<GameObject, float>();
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
            isGameActive = false;
            gameOverMenu.SetActive(true);
            GameObject.Find("Spawn Manager").GetComponent<SpawnPeople>().UpdateGameOverScoreText();
            Timing.PauseCoroutines();  // Not perfect solution if second chance used, hopefully no coroutines will be used during Game Over screen
            Debug.Log("Game Over!");
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

    public void ResetStaticVariables()
    {
        HospitalTile.isOccupied = false;
        HealProgressBar.isNewlyOccupied = false;
        Time.timeScale = 1;  // Resetting time scale here as well when restarting or quitting game
    }
}