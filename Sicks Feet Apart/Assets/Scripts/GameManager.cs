using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class GameManager : MonoBehaviour
{
    internal static GameManager instance;
    private bool isGameActive = true;  // Change to false later
    // private float gameTimer = 0;  // Could use Time.timeSinceLevelLoad with Time.timeScale instead for simple functionality
    public float infectionSpreadRate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Timing.RunCoroutine(InfectionSpread());
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            // gameTimer += Time.deltaTime;
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        Debug.Log("Game Over!");
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
                Timing.RunCoroutine(infectionCylinder.GetComponent<InfectionCylinder>().ExpandRadius());
            }
            yield return Timing.WaitForSeconds(infectionSpreadRate);
        }
    }
}