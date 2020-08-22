using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using TMPro;
using System;

public class LogoAnimation : MonoBehaviour
{
    public GameObject svgImageGO;
    private SVGImage svgImage;
    public bool isShowingLogoScreen;
    private bool isLogoScreenSpedUp;
    public float fadeInTime;
    public float fastFadeInTime;
    public float startImageAdditionalDelay;
    public float individualImageTime;
    public bool isDisablingBetweenImages;
    public float disabledBetweenImagesTime;
    public float endDelay;
    public bool isDisplayingTextOnLastImage;
    public Sprite[] logoImages;
    public TMP_Text nameText;

    public GameObject logoSpeedButton;
    public Sprite logoScreenSpedUpSprite;
    public Sprite logoScreenNormalSpeedSprite;

    public GameObject logoScreen;

    private BeforeMainMenuLoaded beforeMainMenuLoadedScript;

    // Current animation time: 1.2 + 7 * (0.35 + 0.05) + 2

    // Start is called before the first frame update
    void Start()
    {
        svgImage = svgImageGO.GetComponent<SVGImage>();
        beforeMainMenuLoadedScript = GameObject.Find("Background").GetComponent<BeforeMainMenuLoaded>();

        Timing.RunCoroutine(ImmediatelyPauseMusic());
        if (isShowingLogoScreen && BeforeMainMenuLoaded.isFirstTimeLoadingSinceAppOpened)
        {
            gameObject.SetActive(false);
            logoScreen.SetActive(true);
            isLogoScreenSpedUp = (PlayerPrefs.GetInt("IsLogoScreenSpedUp", 0) == 1);
            Timing.RunCoroutine(AnimationProcess());
        }
        else
        {
            // logoScreen.SetActive(false);
            // gameObject.SetActive(true);
            beforeMainMenuLoadedScript.isReadyToLoadMainMenu = true;
        }
        BeforeMainMenuLoaded.isFirstTimeLoadingSinceAppOpened = false;

        DisplayCorrectLogoScreenSpeedUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator<float> AnimationProcess()
    {
        float newFadeInTime;
        if (isLogoScreenSpedUp)
        {
            svgImage.sprite = logoImages[logoImages.Length - 1];
            if (isDisplayingTextOnLastImage)
            {
                nameText.gameObject.SetActive(true);
            }
            newFadeInTime = fastFadeInTime;
        }
        else
        {
            svgImage.sprite = logoImages[0];
            newFadeInTime = fadeInTime;
        }
        Color svgImageColor = svgImage.color;
        svgImage.color = new Color(svgImageColor.r, svgImageColor.g, svgImageColor.b, 0);
        float timer = 0;
        while (timer < newFadeInTime)
        {
            svgImage.color = new Color(svgImageColor.r, svgImageColor.g, svgImageColor.b, Mathf.Lerp(0, 1, timer / newFadeInTime));
            if (isLogoScreenSpedUp)
            {
                nameText.color = new Color(svgImageColor.r, svgImageColor.g, svgImageColor.b, Mathf.Lerp(0, 1, timer / newFadeInTime));
            }
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        svgImage.color = new Color(svgImageColor.r, svgImageColor.g, svgImageColor.b, 1);

        if (!isLogoScreenSpedUp)
        {
            yield return Timing.WaitForSeconds(startImageAdditionalDelay);
            for (int i = 1; i < logoImages.Length; i++)
            {
                yield return Timing.WaitForSeconds(individualImageTime);
                if (isDisablingBetweenImages)
                {
                    svgImageGO.SetActive(false);
                    yield return Timing.WaitForSeconds(disabledBetweenImagesTime);
                    svgImageGO.SetActive(true);
                }
                svgImage.sprite = logoImages[i];
                if (i == (logoImages.Length - 1) && isDisplayingTextOnLastImage)
                {
                    nameText.gameObject.SetActive(true);
                }
            }
        }

        yield return Timing.WaitForSeconds(endDelay);
        logoScreen.SetActive(false);
        beforeMainMenuLoadedScript.isReadyToLoadMainMenu = true;
        // gameObject.SetActive(true);
    }

    IEnumerator<float> ImmediatelyPauseMusic()
    {
        while (true)
        {
            try
            {
                if (AudioManager.instance.musicSource.isPlaying)
                {
                    AudioManager.instance.musicSource.Pause();
                    break;
                }
            }
            catch (NullReferenceException)
            {

            }
            yield return Timing.WaitForOneFrame;
        }
    }

    public void ToggleLogoScreenSpeed()
    {
        if (PlayerPrefs.GetInt("IsLogoScreenSpedUp") == 0)
        {
            PlayerPrefs.SetInt("IsLogoScreenSpedUp", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IsLogoScreenSpedUp", 0);
        }
        DisplayCorrectLogoScreenSpeedUI();
    }

    public void DisplayCorrectLogoScreenSpeedUI()
    {
        if (PlayerPrefs.GetInt("IsLogoScreenSpedUp", 0) == 1)
        {
            logoSpeedButton.GetComponent<Image>().sprite = logoScreenSpedUpSprite;
        }
        else
        {
            logoSpeedButton.GetComponent<Image>().sprite = logoScreenNormalSpeedSprite;
        }
    }
}
