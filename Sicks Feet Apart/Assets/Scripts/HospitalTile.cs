using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using MEC;

public class HospitalTile : MonoBehaviour
{
    public float waitTime;
    internal static bool isOccupied = false;
    public GameObject hospitalBarriers;
    // public GameObject[] barriers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator<float> HospitalQueue()
    {
        // hospitalBarriers.SetActive(true);
        isOccupied = true;
        yield return Timing.WaitForSeconds(waitTime);
        // hospitalBarriers.SetActive(false);
        isOccupied = false;
    }

    /*
    void SetBarriersActive(bool value)
    {
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(value);
        }
    }
    */
}
