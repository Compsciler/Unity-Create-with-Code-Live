using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreLogger : MonoBehaviour
{
    internal static HighScoreLogger instance;
    [SerializeField] internal int gameMode = -1;

    internal string[] highScoreStrings = {"NormalHighScore", "DifficultHighScore", "ExtremeHighScore", "QuickHighScore", "BreakneckHighScore"};
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[] GetHighScores()
    {
        int[] highScores = new int[highScoreStrings.Length];
        for (int i = 0; i < highScores.Length; i++)
        {
            highScores[i] = PlayerPrefs.GetInt(highScoreStrings[i], 0);
        }
        return highScores;
    }

    public void UpdateHighScore(int newScore)
    {
        int highScore = PlayerPrefs.GetInt(highScoreStrings[gameMode]);
        if (newScore > highScore)
        {
            PlayerPrefs.SetInt(highScoreStrings[gameMode], newScore);
            Debug.Log(highScoreStrings[gameMode] + " changed from " + highScore + " to " + newScore);
        }
    }

    public void ResetHighScores()
    {
        for (int i = 0; i < highScoreStrings.Length; i++)
        {
            PlayerPrefs.SetInt(highScoreStrings[i], 0);
        }
        Debug.Log("High scores reset!");
    }
}