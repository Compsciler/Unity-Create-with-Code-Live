using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using MEC;
using TMPro;

public class AdManager2 : MonoBehaviour, IUnityAdsListener
{
    internal static AdManager2 instance;
    internal static bool isInitialized = false;

    private bool isTestMode = false;
    private string placement = "rewardedVideo";

    internal bool isAdCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (!isInitialized)
        {
            isInitialized = true;
            Advertisement.AddListener(this);
            if (Constants.platform == Constants.Platform.iOS)
            {
                Advertisement.Initialize(Constants.appleGameId, isTestMode);  // iOS SPECIFIC
            }
            else
            {
                Advertisement.Initialize(Constants.androidGameId, isTestMode);  // Android SPECIFIC
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator<float> ShowAd()
    {
        AudioManager.instance.SFX_Source.Stop();

        isAdCompleted = false;
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

    public void ShowAdIfNotAllClear()
    {
        if (PlayerPrefs.GetInt("IsAllClear") == 0 && Constants.isMobilePlatform)
        {
            Timing.RunCoroutine(ShowAd());
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("OnUnityAdsFinish");
        if (showResult == ShowResult.Finished)
        {
            isAdCompleted = true;
            Debug.Log("Ad finished");
        }
        else
        {
            Debug.Log("Ad error!");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
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