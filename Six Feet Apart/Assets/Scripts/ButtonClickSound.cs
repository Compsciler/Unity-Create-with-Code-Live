using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound1;
    public AudioClip clickSound2;
    public AudioClip clickSound3;

    private AudioSource SFX_Source;

    // Start is called before the first frame update
    void Start()
    {
        SFX_Source = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(int soundNum)
    {
        switch (soundNum)
        {
            case 1:
                SFX_Source.PlayOneShot(clickSound1);
                break;
            case 2:
                SFX_Source.PlayOneShot(clickSound2);
                break;
            case 3:
                SFX_Source.PlayOneShot(clickSound3);
                break;
        }
    }
}
