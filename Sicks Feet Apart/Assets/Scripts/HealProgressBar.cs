using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealProgressBar : MonoBehaviour
{
    private float healTime = 10f;  // Another constant that should be written in a separate script
    private float healTimer = 0;
    public Image fill;
    // public Color fillColor;

    internal static bool isNewlyOccupied = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNewlyOccupied && GameManager.instance.isGameActive)
        {
            healTimer += Time.deltaTime;
            FillBar();
            if (healTimer > healTime)
            {
                healTimer = 0;
                FillBar();
                isNewlyOccupied = false;
            }
        }
    }

    void FillBar()
    {
        float fillAmount = healTimer / healTime;
        fill.fillAmount = fillAmount;
        // fill.color = fillColor;
    }
}
