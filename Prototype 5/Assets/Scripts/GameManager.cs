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

    private static int highScore = 0;
    public TextMeshProUGUI highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnTarget");
        // UpdateScore(0);
        scoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + highScore;
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
        StartCoroutine(ScoreAnimation(scoreToAdd, 0));
        if (score > highScore)
        {
            StartCoroutine(ScoreAnimation(score - highScore, 1));
        }
    }

    IEnumerator ScoreAnimation(int scoreToAdd, int scoreType)  // 0: Score; 1: High Score
    {
        int tempScore = score;
        if (scoreType == 0)
        {
            score += scoreToAdd;
        }
        for (int i = 1; i <= Mathf.Abs(scoreToAdd); i++)
        {
            if (scoreToAdd > 0)
            {
                if (scoreType == 0)
                {
                    tempScore++;
                }
                else
                {
                    highScore++;
                }
            }
            else
            {
                tempScore--;
            }
            if (scoreType == 0)
            {
                scoreText.text = "Score: " + tempScore;
            }
            else
            {
                highScoreText.text = "High Score: " + highScore;
            }
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