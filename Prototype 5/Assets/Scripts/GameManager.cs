using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
// using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public float spawnRate = 1f;

    private int score = 0;
    public TextMeshProUGUI scoreText;
    public float scoreAnimationLength;
    public TextMeshProUGUI gameOverText;

    internal bool isGameActive = true;

    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnTarget");
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        // score += scoreToAdd;
        // scoreText.text = "Score: " + score;
        StartCoroutine(ScoreAnimation(scoreToAdd));
    }

    IEnumerator ScoreAnimation(int scoreToAdd)
    {
        for (int i = 1; i <= Mathf.Abs(scoreToAdd); i++)
        {
            if (scoreToAdd > 0)
            {
                score++;
            }
            else
            {
                score--;
            }
            scoreText.text = "Score: " + score;
            yield return new WaitForSeconds(scoreAnimationLength / Mathf.Abs(scoreToAdd));
        }
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}