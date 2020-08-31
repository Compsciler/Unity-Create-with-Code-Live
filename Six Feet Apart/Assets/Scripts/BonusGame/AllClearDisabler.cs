using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllClearDisabler : MonoBehaviour
{
    private bool isAllClear;

    public GameObject[] enableIfNotAllClear;
    public GameObject[] disableIfNotAllClear;

    // Start is called before the first frame update
    void Start()
    {
        isAllClear = (PlayerPrefs.GetInt("IsAllClear", 1) == 1);
        if (!isAllClear)
        {
            foreach (GameObject go in enableIfNotAllClear)
            {
                go.SetActive(true);
            }
            foreach (GameObject go in disableIfNotAllClear)
            {
                go.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
