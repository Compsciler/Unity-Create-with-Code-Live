using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class RateGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RequestStoreReview()
    {
        Device.RequestStoreReview();  // iOS SPECIFIC, may also have this pop up after first tutorial complete
    }
}
