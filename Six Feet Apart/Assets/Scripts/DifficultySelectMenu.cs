using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MEC;

public class DifficultySelectMenu : MonoBehaviour
{
    public GameObject[] difficultyButtons;
    public GameObject descriptionTextsHolder;
    private GameObject[] descriptionTexts;

    public AudioClip playButtonSound;
    public GameObject fadingMask;
    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        descriptionTexts = descriptionTextsHolder.GetChildren();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayNormal()  // This will be changed to a generic play function later that has specific parameter to call coroutine
    {
        Timing.RunCoroutine(PlayNormalCoroutine());
        AudioManager.instance.musicSource.Stop();
    }

    IEnumerator<float> PlayNormalCoroutine()
    {
        fadingMask.SetActive(true);
        CoroutineHandle fadeBackgroundCoroutine = Timing.RunCoroutine(FadeBackground());
        AudioManager.instance.SFX_Source.PlayOneShot(playButtonSound);
        yield return Timing.WaitUntilDone(fadeBackgroundCoroutine);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
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
