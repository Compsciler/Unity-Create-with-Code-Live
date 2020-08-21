using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using TMPro;
using System;

public class LogoAnimation : MonoBehaviour
{
    public GameObject svgImageGO;
    private SVGImage svgImage;
    public bool isShowingLogoScreen;
    public float fadeInTime;
    public float startImageAdditionalDelay;
    public float individualImageTime;
    public bool isDisablingBetweenImages;
    public float disabledBetweenImagesTime;
    public float endDelay;
    public bool isDisplayingTextOnLastImage;
    public Sprite[] logoImages;
    public GameObject textGO;

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
            Timing.RunCoroutine(AnimationProcess());
        }
        else
        {
            // logoScreen.SetActive(false);
            // gameObject.SetActive(true);
            beforeMainMenuLoadedScript.isReadyToLoadMainMenu = true;
        }
        BeforeMainMenuLoaded.isFirstTimeLoadingSinceAppOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator<float> AnimationProcess()
    {
        svgImage.sprite = logoImages[0];
        Color svgImageColor = svgImage.color;
        svgImage.color = new Color(svgImageColor.r, svgImageColor.g, svgImageColor.b, 0);
        float timer = 0;
        while (timer < fadeInTime)
        {
            svgImage.color = new Color(svgImageColor.r, svgImageColor.g, svgImageColor.b, Mathf.Lerp(0, 1, timer / fadeInTime));
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        svgImage.color = new Color(svgImageColor.r, svgImageColor.g, svgImageColor.b, 1);

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
                textGO.SetActive(true);
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
}
