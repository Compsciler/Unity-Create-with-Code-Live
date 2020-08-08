﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

public class UsernameCreation : MonoBehaviour
{
    const string privateCode = "gj0QR61YOUyLiGs_ZVIdFw8m_l4jEEW0SytEJWpGyD0g";
    const string publicCode = "5f2ba017eb371809c4afd909";

    const string webURL = "http://dreamlo.com/lb/";

    public GameObject mainMenu;
    public TMP_InputField inputField;
    public TMP_Text errorText;
    private BeforeMainMenuLoaded beforeMainMenuLoadedScript;

    private string inputUsername;
    private bool isDoneWritingUsername = false;
    private IEnumerator checkIfUsernameIsUniqueIEnumerator;
    private bool checkIfUsernameIsUniqueFinished = false;
    private bool isConnectionTimedOut = false;

    private int minLength = 3;
    private int maxLength = 20;
    private bool[] reqMetArr = {true, false, false};

    // Start is called before the first frame update
    void Start()
    {
        beforeMainMenuLoadedScript = GameObject.Find("Background").GetComponent<BeforeMainMenuLoaded>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CreateUsername()  // Usernames may only contain [A-Z][a-z][0-9]_ and must be unique by lowercase
    {
        mainMenu.SetActive(false);
        gameObject.SetActive(true);

        do
        {
            isDoneWritingUsername = false;
            checkIfUsernameIsUniqueFinished = false;
            isConnectionTimedOut = false;

            yield return new WaitUntil(() => isDoneWritingUsername);
            errorText.text = "";
            inputUsername = inputField.text;
            reqMetArr[1] = AreCharactersValid();
            reqMetArr[2] = IsLengthValid();
            if (reqMetArr[1] && reqMetArr[2])
            {
                checkIfUsernameIsUniqueIEnumerator = CheckIfUsernameIsUnique();  // Last check
                StartCoroutine(checkIfUsernameIsUniqueIEnumerator);
                yield return new WaitUntil(() => checkIfUsernameIsUniqueFinished);
                if (isConnectionTimedOut || !string.IsNullOrEmpty(errorText.text))
                {
                    errorText.text = "Check your internet connection and try again.";
                    reqMetArr[0] = false;
                    continue;
                }
            }
            errorText.text = "Requirement(s) ";
            for (int i = 0; i < reqMetArr.Length; i++)
            {
                if (!reqMetArr[i])
                {
                    errorText.text += (i + 1) + ", ";
                }
            }
            errorText.text = errorText.text.Substring(0, errorText.text.Length - 2);  // Removes last ", "
            errorText.text += " failed!";
        }
        while (!reqMetArr.All(b => b));
        errorText.gameObject.SetActive(false);
        StartCoroutine(UploadAndSetNewUsername());

        // mainMenu.SetActive(true);
        // mainMenu.GetComponent<BeforeMainMenuLoaded>().isReadyToLoadMainMenu = true;
    }

    IEnumerator UploadAndSetNewUsername()
    {
        UnityWebRequest request = UnityWebRequest.Get(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(inputUsername.ToLower()) + "/" + 1);
        yield return request.SendWebRequest();

        if (string.IsNullOrEmpty(request.error))
        {
            PlayerPrefs.SetString("Username", inputUsername);
            LeaderboardManager.username = inputUsername;
            Debug.Log("Upload username successful!");
            gameObject.SetActive(false);
            beforeMainMenuLoadedScript.isReadyToLoadMainMenu = true;
        }
        else
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Error uploading username! Restart app and try again.";
        }
    }

    IEnumerator ConnectionTimeout(IEnumerator runningIEnumerator)
    {
        yield return new WaitForSeconds(Constants.connectionTimeoutTime);
        StopCoroutine(runningIEnumerator);
        
        if (runningIEnumerator == checkIfUsernameIsUniqueIEnumerator)
        {
            isConnectionTimedOut = true;
            checkIfUsernameIsUniqueFinished = true;
        }
    }

    public void SetIsDoneWritingUsername(bool value)
    {
        isDoneWritingUsername = value;
    }

    IEnumerator CheckIfUsernameIsUnique()
    {
        UnityWebRequest request = UnityWebRequest.Get(webURL + publicCode + "/pipe-get/" + inputUsername.ToLower());  // Gets "score" for inputUsername if inputUsername exists
        request.timeout = Constants.connectionTimeoutTime;
        // StartCoroutine(ConnectionTimeout(checkIfUsernameIsUniqueIEnumerator));
        yield return request.SendWebRequest();
        // StopCoroutine(ConnectionTimeout(checkIfUsernameIsUniqueIEnumerator));

        errorText.text = request.error;
        reqMetArr[0] = (string.IsNullOrEmpty(request.downloadHandler.text));
        checkIfUsernameIsUniqueFinished = true;
    }

    bool AreCharactersValid()
    {
        Regex re = new Regex("^[A-Za-z0-9_]*$");
        return re.IsMatch(inputUsername);
    }

    bool IsLengthValid()
    {
        return (inputUsername.Length >= minLength && inputUsername.Length <= maxLength);
    }
}
