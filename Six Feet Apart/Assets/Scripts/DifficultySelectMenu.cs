using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MEC;
using TMPro;

public class DifficultySelectMenu : MonoBehaviour
{
    public GameObject[] difficultyButtons;
    public GameObject descriptionTextsHolder;
    private GameObject[] descriptionTexts;

    public AudioClip playButtonSound;
    public GameObject fadingMask;
    public float fadeTime;
    public GameObject[] enableAfterFading;

    public GameObject[] enableOnFirstTimePlaying;

    // https://stackoverflow.com/questions/5849548/is-this-array-initialization-incorrect
    /*
    internal static int[][,] gameModeUnlockReqs = new int[][,]{
        new int[,] {{}},
        new int[,] {{}},
        new int[,] {{0, 30}},
        new int[,] {{1, 35}},
        new int[,] {{0, 30}},
        new int[,] {{3, 35}},
        new int[,] {{0, 30}},
        new int[,] {{5, 25}},  // Changed from 35
        new int[,] {{2, 30}, {4, 20}, {6, 40}}  // Changed from {2, 40} to {2, 20}
    };
    */
    // EASIER
    internal static int[][,] gameModeUnlockReqs = new int[][,]{
        new int[,] {{}},
        new int[,] {{}},
        new int[,] {{0, 20}},  // Changed from 25 to 20
        new int[,] {{1, 25}},  // Changed from 30 to 25
        new int[,] {{0, 20}},  // Changed from 25 to 20
        new int[,] {{3, 25}},  // Changed from 30 to 25
        new int[,] {{0, 20}},  // Changed from 25 to 20
        new int[,] {{5, 25}},  // Changed from 30 to 25
        new int[,] {{2, 30}, {4, 20}, {6, 35}}  // Changed from {2, 40} to {2, 20}
    };

    // Start is called before the first frame update
    void Start()
    {
        descriptionTexts = descriptionTextsHolder.GetChildren();

        if (PlayerPrefs.GetInt("IsFirstTimePlaying", 1) == 1)
        {
            foreach (GameObject go in enableOnFirstTimePlaying)
            {
                go.SetActive(true);
            }
            PlayerPrefs.SetInt("IsFirstTimePlaying", 0);
        }
    }

    void OnEnable()
    {
        SetUpUnlocksAndScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(int gameMode)
    {
        Timing.RunCoroutine(PlayStartCoroutine());
        AudioManager.instance.musicSource.Stop();

        HighScoreLogger.instance.gameMode = gameMode;
    }

    IEnumerator<float> PlayStartCoroutine()
    {
        fadingMask.SetActive(true);
        CoroutineHandle fadeBackgroundCoroutine = Timing.RunCoroutine(FadeBackground());
        AudioManager.instance.SFX_Source.PlayOneShot(playButtonSound);
        yield return Timing.WaitUntilDone(fadeBackgroundCoroutine);
        foreach (GameObject go in enableAfterFading)
        {
            go.SetActive(true);
        }
        SceneManager.LoadSceneAsync(Constants.gameSceneBuildIndex);
    }

    IEnumerator<float> FadeBackground()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            Color maskColor = fadingMask.GetComponent<Image>().color;
            fadingMask.GetComponent<Image>().color = new Color(maskColor.r, maskColor.g, maskColor.b, Mathf.Lerp(0, 1, timer / fadeTime));
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

    public void ResetMenuPresses()
    {
        foreach (GameObject button in difficultyButtons)
        {
            // button.transform.Find("Description Text").gameObject.SetActive(false);
            button.transform.Find("Pressed Button Image").gameObject.SetActive(false);
            try
            {
                button.transform.Find("Start Button").gameObject.SetActive(false);
            }
            catch (NullReferenceException)
            {
                
            }
        }
        foreach (GameObject descriptionText in descriptionTexts)
        {
            descriptionText.SetActive(false);
        }
    }

    private void SetUpUnlocksAndScores()
    {
        int[] highScores = HighScoreLogger.instance.GetHighScores(false);

        for (int i = 0; i < gameModeUnlockReqs.Length; i++)
        {
            GameObject difficultyButton = difficultyButtons[i];
            int[,] currentUnlockReqs = gameModeUnlockReqs[i];
            bool currentUnlockReqsMet = true;
            for (int j = 0; j < currentUnlockReqs.Length / 2; j++)  // Foreach loop doesn't work somehow, probably because C# Length property returns total number of integers in array
            {
                int highScoreForReq = highScores[currentUnlockReqs[j, 0]];
                int minScoreReq = currentUnlockReqs[j, 1];
                if (highScoreForReq < minScoreReq)
                {
                    currentUnlockReqsMet = false;
                }
            }
            if (currentUnlockReqsMet || PlayerPrefs.GetInt("AreAllGameModesUnlocked", 0) == 1)
            {
                difficultyButton.transform.Find("Lock Icon").gameObject.SetActive(false);
                try
                {
                    difficultyButton.transform.Find("Start Button").GetComponent<Button>().interactable = true;
                }
                catch (NullReferenceException)
                {

                }
                if (i >= 1 && i < highScores.Length + 1)  // To not try to access scores of Tutorial and Custom Mode
                {
                    int highScore = highScores[i - 1];
                    if (highScore > 0)
                    {
                        TMP_Text scoreText = difficultyButton.transform.Find("Score Text").GetComponent<TMP_Text>();
                        scoreText.text = highScore.ToString();
                    }
                }
            }
        }
    }
}
