using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://answers.unity.com/questions/1658534/camera-fov-and-distance-based-on-mobile-screen-siz.html

public class CameraFOV : MonoBehaviour
{
    private Camera mainCamera;
    
    private Vector3 defaultTransformPos;
    private float defaultTransformWidth; // Changed from 55f;
    private float defaultTransformHeight = 98.4f;
    private float defaultFOV;

    // private int defaultScreenPixelWidth = 1920;
    private float defaultAspectRatio = 16f / 9f;
    private float aspectRatio;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        mainCamera = GetComponent<Camera>();
        defaultTransformPos = transform.position;
        defaultFOV = mainCamera.fieldOfView;
        aspectRatio = (float)Screen.width / Screen.height;
        yield return null;
        defaultTransformWidth = (Mathf.Tan(defaultFOV / Mathf.Rad2Deg / 2f) * defaultTransformPos.y) * 2f;  // 57.73503f
        if (aspectRatio < defaultAspectRatio)
        {
            ScaleFOV();
            Debug.Log("Scaled FOV");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScaleFOV()
    {
        float width = (defaultAspectRatio / aspectRatio) * defaultTransformWidth;
        mainCamera.fieldOfView = Mathf.Atan((width / 2f) / defaultTransformPos.y) * Mathf.Rad2Deg * 2f;
        // Debug.Log(mainCamera.fieldOfView);
    }
}
