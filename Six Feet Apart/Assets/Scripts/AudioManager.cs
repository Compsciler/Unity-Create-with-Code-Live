using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    internal static AudioManager instance;
    internal AudioSource SFX_Source;
    internal AudioSource musicSource;

    public GameObject SFX_MuteButton;
    public GameObject musicMuteButton;
    public Sprite SFX_UnmutedSprite;
    public Sprite SFX_MutedSprite;
    public Sprite musicUnmutedSprite;
    public Sprite musicMutedSprite;

    // private bool isSFX_Muted;
    // private bool isMusicPaused;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SFX_Source = GetComponents<AudioSource>()[0];
        musicSource = GetComponents<AudioSource>()[1];

        // Time.timeScale = 1;  // Moved back to GameManager.ResetStaticVariables()

        /*
        SFX_Source.mute = (PlayerPrefs.GetInt("IsSFX_Muted", 0) == 1);
        Debug.Log("SFX starts muted: " + SFX_Source.mute);
        // isSFX_Muted = (PlayerPrefs.GetInt("IsSFX_Muted", 0) == 0);
        // SFX_Source.mute = isSFX_Muted;
        if (PlayerPrefs.GetInt("IsMusicMuted", 0) == 1)
        {
            musicSource.mute = true;
            musicSource.Pause();
        }
        */
        DisplayCorrectSFX();
        DisplayCorrectMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleSFX()
    {
        if (PlayerPrefs.GetInt("IsSFX_Muted") == 0)
        {
            PlayerPrefs.SetInt("IsSFX_Muted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IsSFX_Muted", 0);
        }
        DisplayCorrectSFX();
    }

    public void ToggleMusic(bool unpauseOnUnmute)
    {
        if (PlayerPrefs.GetInt("IsMusicMuted") == 0)
        {
            PlayerPrefs.SetInt("IsMusicMuted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IsMusicMuted", 0);
        }
        DisplayCorrectMusic();
        if (PlayerPrefs.GetInt("IsMusicMuted") == 0 && unpauseOnUnmute)
        {
            musicSource.UnPause();
        }
    }

    public void DisplayCorrectSFX()
    {
        if (PlayerPrefs.GetInt("IsSFX_Muted", 0) == 1)
        {
            SFX_Source.mute = true;
            SFX_MuteButton.GetComponent<Image>().sprite = SFX_MutedSprite;
        }
        else
        {
            SFX_Source.mute = false;
            SFX_MuteButton.GetComponent<Image>().sprite = SFX_UnmutedSprite;
        }
    }

    public void DisplayCorrectMusic()
    {
        if (PlayerPrefs.GetInt("IsMusicMuted", 0) == 1)
        {
            musicSource.mute = true;  // Muting so don't have to check for all pausing and unpausing scenarios in GameScene
            musicSource.Pause();
            musicMuteButton.GetComponent<Image>().sprite = musicMutedSprite;
        }
        else
        {
            musicSource.mute = false;
            // musicSource.UnPause();
            musicMuteButton.GetComponent<Image>().sprite = musicUnmutedSprite;
        }
    }
}
