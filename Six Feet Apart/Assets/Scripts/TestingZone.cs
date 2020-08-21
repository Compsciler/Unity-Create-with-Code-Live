using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestingZone : MonoBehaviour
{
    public TMP_InputField inputFieldComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string inputFieldText = inputFieldComponent.text;

        // Format: UHS:2=5;
        if (inputFieldText.Length > 0 && inputFieldText[inputFieldText.Length - 1] == ';')
        {
            inputFieldText = inputFieldText.Remove(inputFieldText.Length - 1);
            string[] parsedStringArr = new string[3];
            if (inputFieldText.Contains(":"))
            {
                parsedStringArr[0] = inputFieldText.Split(':')[0];
                parsedStringArr[1] = inputFieldText.Split(':')[1];
                if (parsedStringArr[1].Contains("="))
                {
                    string keyValuePair = parsedStringArr[1];
                    parsedStringArr[1] = keyValuePair.Split('=')[0];
                    parsedStringArr[2] = keyValuePair.Split('=')[1];

                    // Debug.Log(parsedStringArr[0] + ", " + parsedStringArr[1] + ", " + parsedStringArr[2]);
                }
                try
                {
                    switch (parsedStringArr[0])
                    {
                        case "UHS":  // Update high score
                            HighScoreLogger.instance.gameMode = int.Parse(parsedStringArr[1]);
                            HighScoreLogger.instance.UpdateHighScore(int.Parse(parsedStringArr[2]), true);
                            break;
                        case "UAGM":  // Unlock all game modes
                            HighScoreLogger.instance.UnlockAllGameModes(int.Parse(parsedStringArr[1]));
                            Debug.Log("Unlocked all game modes");
                            break;
                        case "CU":  // Change username (only for testing, doesn't modify database)
                            Debug.Log("Username changed from " + PlayerPrefs.GetString("Username") + " to " + parsedStringArr[1]);
                            PlayerPrefs.SetString("Username", parsedStringArr[1]);
                            break;
                        case "DU":  // Delete username (only for testing, doesn't modify database)
                            PlayerPrefs.SetString("Username", null);
                            Debug.Log("Deleted username");
                            break;
                        case "RSRRT":  // Reset store review request total
                            PlayerPrefs.SetInt("StoreReviewRequestTotal", 0);
                            Debug.Log("Reset store review request total");
                            break;
                    }
                }
                catch
                {
                    Debug.Log("Error!");
                }
                inputFieldComponent.text = "";
            }
        }
    }
}
