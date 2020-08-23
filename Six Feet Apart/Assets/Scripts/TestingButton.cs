using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TestingButton : MonoBehaviour
{
    public TMP_InputField inputField;

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
        // GameObject.Find("Pause Button").GetComponent<Button>().interactable = false;
        // Application.OpenURL("https://www.cdc.gov/coronavirus/2019-ncov/prevent-getting-sick/social-distancing.html");

        // GameObject person = GameObject.Find("Person " + inputField.text);
        // Debug.Log("Speed: " + person.GetComponent<NavMeshAgent>().velocity.magnitude);
        // person.GetComponent<NavMeshAgent>().avoidancePriority = 45;
        // Debug.Log("Remaining Distance: " + person.GetComponent<NavMeshAgent>().remainingDistance);

        // Debug.Log(PersonController.infectedPeopleTotal);
        // GameManager.instance.ResetStaticVariables();

        // Debug.Log(Screen.width + "x" + Screen.height);
        if (Time.timeScale > 1)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 10;
        }
    }
}
