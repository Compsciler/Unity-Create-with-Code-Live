using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Threading;

public class CoroutineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Timing.RunCoroutine(HideObject());
        Debug.Log("Checkpoint 1");
    }
    IEnumerator<float> HideObject()
    {
        yield return Timing.WaitForSeconds(5f);
        gameObject.SetActive(false);
        Debug.Log("Checkpoint 2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
