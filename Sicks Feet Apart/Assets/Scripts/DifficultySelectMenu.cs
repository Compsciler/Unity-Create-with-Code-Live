using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelectMenu : MonoBehaviour
{
    public GameObject[] difficultyButtons;
    public GameObject descriptionTextsHolder;
    private GameObject[] descriptionTexts;

    public AudioClip playButtonSound;

    // Start is called before the first frame update
    void Start()
    {
        descriptionTexts = descriptionTextsHolder.GetChildren();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayNormal()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        // Camera.main.GetComponent<AudioSource>().PlayOneShot(playButtonSound);
    }

    public void ResetMenu()
    {
        foreach (GameObject button in difficultyButtons)
        {
            // button.transform.Find("Description Text").gameObject.SetActive(false);
            button.transform.Find("Pressed Button Image").gameObject.SetActive(false);
            try
            {
                button.transform.Find("Play Button").gameObject.SetActive(false);
            }
            catch (NullReferenceException e)
            {
                
            }
        }
        foreach (GameObject descriptionText in descriptionTexts)
        {
            descriptionText.SetActive(false);
        }
    }
}
