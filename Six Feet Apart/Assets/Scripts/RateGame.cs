using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class RateGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.isReadyToRequestStoreReview)
        {
            Device.RequestStoreReview();
            GameManager.isReadyToRequestStoreReview = false;
            PlayerPrefs.SetInt("StoreReviewRequestTotal", 1);
            Debug.Log("Requeseted store review!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RequestStoreReview()
    {
        Device.RequestStoreReview();  // iOS SPECIFIC, will change to link to store page
    }
}
