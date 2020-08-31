using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public float maxValue;
    private float currentValue;
    public Image mask;
    public Image fill;
    public bool isHueCyclingOnce;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager2.instance.isGameActive)
        {
            currentValue -= Time.deltaTime;
            if (currentValue < 0)
            {
                currentValue = 0;
                GameManager2.instance.GameOver();
            }
            FillBar();
        }
    }

    void FillBar()
    {
        float fillAmount = currentValue / maxValue;
        mask.fillAmount = fillAmount;
        if (isHueCyclingOnce)
        {
            fill.color = Color.HSVToRGB(Mathf.Lerp(0f, 1f, fillAmount), 1, 1);
        }
    }
}
