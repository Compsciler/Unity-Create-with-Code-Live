using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionCylinder : MonoBehaviour
{
    public float startRadius;
    public float maxRadius;
    public float expandingDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator<float> ExpandRadius()
    {
        Vector3 startScale = new Vector3(startRadius, gameObject.transform.localScale.y, startRadius);
        Vector3 endScale = new Vector3(maxRadius, gameObject.transform.localScale.y, maxRadius);
        gameObject.SetActive(true);
        float timer = 0;
        while (timer < expandingDuration)
        {
            Debug.Log(gameObject.transform.localScale);
            gameObject.transform.localScale = Vector3.Lerp(startScale, endScale, timer / expandingDuration);
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        gameObject.SetActive(false);
        gameObject.transform.localScale = startScale;
    }
}
