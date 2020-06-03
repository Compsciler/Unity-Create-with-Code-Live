using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelectMenu : MonoBehaviour
{
    public GameObject[] difficultyButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayNormal()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetMenu()
    {
        foreach (GameObject button in difficultyButtons)
        {
            button.transform.Find("Description Text").gameObject.SetActive(false);
            button.transform.Find("Pressed Button Image").gameObject.SetActive(false);
            try
            {
                button.transform.Find("Play Button").gameObject.SetActive(false);
            }
            catch (NullReferenceException e)
            {
                
            }
        }
    }
}
