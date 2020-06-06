using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveProgressBar : MonoBehaviour
{
    [SerializeField] float maxValue;
    [SerializeField] float currentValue;
    public Image mask;
    public Image fill;
    public Color fillHealthyColor;
    public Color fillInfectedColor;
    private SpawnPeople spawnPeopleScript;

    // Start is called before the first frame update
    void Start()
    {
        spawnPeopleScript = GameObject.Find("Spawn Manager").GetComponent<SpawnPeople>();
    }

    // Update is called once per frame
    void Update()
    {
        maxValue = spawnPeopleScript.repeatRate;
        currentValue = maxValue - spawnPeopleScript.timer;
        FillBar();
    }

    void FillBar()
    {
        float fillAmount = currentValue / maxValue;
        mask.fillAmount = fillAmount;
        if (spawnPeopleScript.isInfectedWave)
        {
            fill.color = fillInfectedColor;
        } else
        {
            fill.color = fillHealthyColor;
        }
    }
}
