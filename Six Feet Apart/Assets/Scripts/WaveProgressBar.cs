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
    public Color fillUnknownColor;
    private SpawnPeople spawnPeopleScript;

    public GameObject spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnPeopleScript = spawnManager.GetComponent<SpawnPeople>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.hasGameStarted)
        {
            maxValue = spawnPeopleScript.repeatRate;
            currentValue = maxValue - spawnPeopleScript.timer;
            FillBar();
        }
    }

    void FillBar()
    {
        float fillAmount = currentValue / maxValue;
        mask.fillAmount = fillAmount;
        if (GameManager.instance.areSymptomsDelayed)
        {
            fill.color = fillUnknownColor;
        }
        else
        {
            if (spawnPeopleScript.isInfectedWave)
            {
                fill.color = fillInfectedColor;
            }
            else
            {
                fill.color = fillHealthyColor;
            }
        }
    }
}
