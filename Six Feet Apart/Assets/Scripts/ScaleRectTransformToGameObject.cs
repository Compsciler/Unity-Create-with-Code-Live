using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRectTransformToGameObject : MonoBehaviour
{
    public bool isScalingWidth;
    public bool isScalingHeight;
    public GameObject scaleToGO;

    private float newWidth;
    private float newHeight;
    private RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        if (isScalingWidth)
        {
            newWidth = scaleToGO.GetComponent<RectTransform>().sizeDelta[0];
        }
        else
        {
            newWidth = GetComponent<RectTransform>().sizeDelta[0];
        }
        if (isScalingHeight)
        {
            newHeight = scaleToGO.GetComponent<RectTransform>().sizeDelta[1];
        }
        else
        {
            newHeight = GetComponent<RectTransform>().sizeDelta[1];
        }
        Debug.Log(newWidth);
        Debug.Log(newHeight);
        Debug.Log(scaleToGO.GetComponent<RectTransform>().sizeDelta[0]);
        rect.sizeDelta = new Vector2(newWidth, newHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
