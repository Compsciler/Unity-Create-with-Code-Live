using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerMatch : MonoBehaviour
{
    public float match;
    private float defaultAspectRatio = 16f / 9f;
    private float aspectRatio;

    // Start is called before the first frame update
    void Start()
    {
        aspectRatio = (float)Screen.width / Screen.height;
        if (aspectRatio < defaultAspectRatio)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = match;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
