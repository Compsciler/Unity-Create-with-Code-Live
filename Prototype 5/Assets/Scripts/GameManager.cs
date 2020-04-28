using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public float spawnRate = 1f;

    private int score = 0;
    public TextMeshProUGUI scoreText;

    public float scoreAnimationLength;

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
        while (true)
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
}