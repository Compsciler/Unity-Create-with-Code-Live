using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    internal static AudioManager instance;
    internal AudioSource SFX_Source;
    internal AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SFX_Source = GetComponents<AudioSource>()[0];
        musicSource = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
