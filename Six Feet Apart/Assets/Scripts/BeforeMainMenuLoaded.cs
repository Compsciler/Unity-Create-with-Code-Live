using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeMainMenuLoaded : MonoBehaviour
{
    public GameObject mainMenu;

    internal bool isReadyToLoadMainMenu = false;
    public GameObject usernameCreationMenu;

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
            if (PlayerPrefs.GetString("Username", "").Equals(""))
            {
                StartCoroutine(usernameCreationMenu.GetComponent<UsernameCreation>().CreateUsername());
            }
            else
            {
                LeaderboardManager.username = PlayerPrefs.GetString("Username");
                mainMenu.SetActive(true);
                AudioManager.instance.musicSource.Play();
            }
        }
    }
}
