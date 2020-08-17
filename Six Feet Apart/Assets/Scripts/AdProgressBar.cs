using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdProgressBar : MonoBehaviour
{
    public float progressTime = 5f;
    internal float progressTimer = 0;
    public bool isFillReversed;
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        progressTimer += Time.deltaTime;
        FillBar();
        if (progressTimer > progressTime)
        {
            AdManager.instance.CloseAdMenu();
        }
    }

    void FillBar()
    {
        float fillAmount = progressTimer / progressTime;
        if (isFillReversed)
        {
            fillAmount = 1 - fillAmount;
        }
        fill.fillAmount = fillAmount;
    }
}