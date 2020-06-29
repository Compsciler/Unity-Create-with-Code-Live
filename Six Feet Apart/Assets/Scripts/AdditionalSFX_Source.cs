using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalSFX_Source : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().mute = (PlayerPrefs.GetInt("IsSFX_Muted") == 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
