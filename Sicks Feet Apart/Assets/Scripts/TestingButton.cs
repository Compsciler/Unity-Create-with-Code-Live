using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestingFunction()
    {
        // Debug.Log("Hospital occupied: " + HospitalTile.isOccupied);
        // Debug.Log(Time.timeScale);
        GameObject.Find("Pause Button").GetComponent<Button>().interactable = false;
    }
}
