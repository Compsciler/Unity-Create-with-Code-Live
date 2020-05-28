using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionCylinder : MonoBehaviour
{
    public float minRadius;
    public float maxRadius;
    public float scalingDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator<float> ExpandRadius()  // Runs once
    {
        Vector3 startScale = new Vector3(minRadius, transform.localScale.y, minRadius);
        Vector3 endScale = new Vector3(maxRadius, transform.localScale.y, maxRadius);
        gameObject.SetActive(true);
        float timer = 0;
        while (timer < scalingDuration)
        {
            Debug.Log(transform.localScale);
            transform.localScale = Vector3.Lerp(startScale, endScale, timer / scalingDuration);
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        gameObject.SetActive(false);
        gameObject.transform.localScale = startScale;
    }

    public IEnumerator<float> SinusoidalRadius()  // Runs continuously
    {
        Vector3 maxScale = new Vector3(maxRadius, transform.localScale.y, maxRadius);
        float midRadius = (minRadius + maxRadius) / 2;
        Vector3 midScale = new Vector3(midRadius, transform.localScale.y, midRadius);
        gameObject.SetActive(true);
        while (true)
        {
            yield return Timing.WaitForOneFrame;
            float sineTime = Mathf.Sin(Time.timeSinceLevelLoad * Mathf.PI * 2);
            transform.localScale = Vector3.LerpUnclamped(midScale, maxScale, sineTime);
        }
    }
}
