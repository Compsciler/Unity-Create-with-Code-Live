using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using MEC;
using TMPro;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    internal static AdManager instance;
    internal static bool isInitialized = false;

    public GameObject adMenu;
    public GameObject adClock;
    public TMP_Text descriptionErrorText;
    private bool isTestMode = false;
    private bool isTimeScaleZeroDuringAd;
    private string placement = "rewardedVideo";

    internal bool isAdCompleted = false;
    internal int adsWatchedTotal = 0;
    internal int maxAdsWatchedPerGame = 1;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        isTimeScaleZeroDuringAd = isTestMode;
        if (!isInitialized)
        {
            isInitialized = true;
            Advertisement.AddListener(this);  // Was being added twice before isInitialized
            if (Constants.platform == Constants.Platform.iOS)
            {
                Advertisement.Initialize(Constants.appleGameId, isTestMode);  // iOS SPECIFIC
            }
            else
            {
                Advertisement.Initialize(Constants.androidGameId, isTestMode);  // Android SPECIFIC
            }
        }

        if (GameManager.instance.areSymptomsDelayed && GameManager.instance.isResettingDelayedSymptoms)
        {
            descriptionErrorText.text = "Watch ad to reset infection timers and reveal all infected people";
        }
        else
        {
            descriptionErrorText.text = "Watch ad to reset infection timers";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator<float> ShowAd()
    {
        isAdCompleted = false;
        if (isTimeScaleZeroDuringAd)
        {
            Time.timeScale = 0;
        }
        if (!Advertisement.IsReady())
        {
            yield return Timing.WaitForOneFrame;
        }
        Advertisement.Show(placement);
        Debug.Log("Ad shown");
    }

    public void ShowAdStartCoroutine()
    {
        Timing.RunCoroutine(ShowAd());
    }

    public void CloseAdMenu()
    {
        Debug.Log("Is adMenu null: " + (adMenu == null));
        Debug.Log(adMenu.GetComponent<RectTransform>().pivot);
        adMenu.SetActive(false);
    }

    public IEnumerator<float> InfiniteWaitToBreakFrom()
    {
        Debug.Log("STARTED");
        while (true)
        {
            yield return Timing.WaitForOneFrame;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("OnUnityAdsFinish");
        if (isTimeScaleZeroDuringAd)
        {
            Time.timeScale = 1;
        }
        if (showResult == ShowResult.Finished)
        {
            isAdCompleted = true;
            adsWatchedTotal++;
            CloseAdMenu();
            Debug.Log("Ad finished");
        }
        else
        {
            descriptionErrorText.text = "<color=#FF4040>Error loading or finishing ad! Check your internet connection.</color>";
            adClock.GetComponent<AdProgressBar>().progressTimer = 0;
            Debug.Log("Ad error!");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        descriptionErrorText.text = "<color=#FF4040>Error loading ad! Check your internet connection.</color>";
        Debug.Log("OnUnityAdsDidError");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // throw new System.NotImplementedException();
    }

    void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}