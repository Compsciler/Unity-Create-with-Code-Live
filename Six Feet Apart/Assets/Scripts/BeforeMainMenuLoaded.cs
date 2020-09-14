using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeMainMenuLoaded : MonoBehaviour
{
    public GameObject mainMenu;

    internal bool isReadyToLoadMainMenu = false;
    public GameObject usernameCreationMenu;

    internal static bool isFirstTimeLoadingSinceAppOpened = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyToLoadMainMenu)
        {
            Debug.Log("Username: " + PlayerPrefs.GetString("Username"));
            isReadyToLoadMainMenu = false;
            if (PlayerPrefs.GetString("Username", "").Equals("") && !LeaderboardManager.isPlayingAsGuest)
            {
                StartCoroutine(usernameCreationMenu.GetComponent<UsernameCreation>().CreateUsername());
            }
            else
            {
                if (LeaderboardManager.isPlayingAsGuest)
                {
                    LeaderboardManager.username = "Guest";
                }
                else
                {
                    LeaderboardManager.username = PlayerPrefs.GetString("Username");
                }
                if (PlayerPrefs.GetInt("IsAllClear", 0) == 1 || !usernameCreationMenu.GetComponent<UsernameCreation>().isCheckingIfAllClear)
                {
                    mainMenu.SetActive(true);
                    AudioManager.instance.musicSource.Play();
                }
                else
                {
                    SceneManager.LoadScene(Constants.bonusGameBuildIndex);
                }
            }
        }
    }
}
