using MEC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsEasterEgg : MonoBehaviour
{
    private bool isAllShowing = false;

    private bool isShowingPerfect = false;
    private bool isShowingSpot = false;
    private bool isShowingFor = false;
    private bool isShowingAn = false;
    private bool isShowingEaster = false;
    // private bool isShowingEgg = false;

    private Toggle toggleComponent;
    private Scrollbar scrollbarComponent;
    private Slider sliderComponent;
    private TMP_InputField inputFieldComponent;
    private TMP_Text yearTextComponent;
    private TMP_Text backButtonTextComponent;

    private string defaultInputText;
    public float backButtonHoldTime = 1f;
    private float backButtonHoldTimer;

    public GameObject[] enableOnEasterEgg;
    public GameObject[] disableOnEasterEgg;

    private TouchScreenKeyboard keyboard;

    private string linkUrl = "https://www.who.int/emergencies/diseases/novel-coronavirus-2019/advice-for-public";

    public AudioClip playButtonSound;
    public GameObject fadingMask;
    public float fadeTime;
    public GameObject[] enableAfterFading;

    // Start is called before the first frame update
    void Start()
    {
        Transform optionsMenuTransform = GameObject.Find("Options Menu").transform;
        toggleComponent = optionsMenuTransform.Find("Toggle").gameObject.GetComponent<Toggle>();
        scrollbarComponent = optionsMenuTransform.Find("Scrollbar").gameObject.GetComponent<Scrollbar>();
        sliderComponent = optionsMenuTransform.Find("Slider").gameObject.GetComponent<Slider>();
        inputFieldComponent = optionsMenuTransform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>();
        yearTextComponent = optionsMenuTransform.Find("Year Text").gameObject.GetComponent<TMP_Text>();
        backButtonTextComponent = optionsMenuTransform.Find("Back Button").GetComponentInChildren<TMP_Text>();

        defaultInputText = inputFieldComponent.text;  // Should be 36 spaces followed by "an"
        backButtonHoldTimer = backButtonHoldTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAllShowing)
        {
            isShowingPerfect = toggleComponent.isOn;
            isShowingSpot = (scrollbarComponent.value > 0.95f);  // 0.95 is threshold
            isShowingFor = (sliderComponent.value == 4);
            string inputFieldText = inputFieldComponent.text;
            isShowingAn = (inputFieldText.Trim().Equals("an") && inputFieldText.Length <= 34);  // 34 is maximum length of valid string
            isShowingEaster = yearTextComponent.text.Equals("Easter");

            if (backButtonTextComponent.text.Equals("Egg"))
            {
                backButtonHoldTimer -= Time.deltaTime;
                if (backButtonHoldTimer < 0)
                {
                    isAllShowing = isShowingPerfect && isShowingSpot && isShowingFor && isShowingAn && isShowingEaster;
                    if (isAllShowing)
                    {
                        foreach (GameObject go in enableOnEasterEgg)
                        {
                            go.SetActive(true);
                        }
                        foreach (GameObject go in disableOnEasterEgg)
                        {
                            go.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                backButtonHoldTimer = backButtonHoldTime;
            }

            if (inputFieldText.Equals(""))
            {
                inputFieldComponent.text = defaultInputText;  // Resets input text if all deleted
            }
        }
        /*
        if (!TouchScreenKeyboard.visible && keyboard != null)
        {
            if (keyboard.done)
            {
                string x = keyboard.text;
                TMP_Text t = GetComponentInChildren<TMP_Text>();
                t.text = "Bonjour " + x;
            }
        }
        */
    }

    public void ShowValues()
    {
        Debug.Log("Perfect: " + isShowingPerfect);
        Debug.Log("Spot: " + isShowingSpot);
        Debug.Log("For: " + isShowingFor);
        Debug.Log("An: " + isShowingAn);
        Debug.Log("Easter: " + isShowingEaster);
        // Debug.Log("Egg: " + isShowingEgg);
    }

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    public void OpenLink()
    {
        Application.OpenURL(linkUrl);
    }

    public void PlayBonusGame()
    {
        Timing.RunCoroutine(PlayBonusGameStartCoroutine());
        AudioManager.instance.musicSource.Stop();
    }

    IEnumerator<float> PlayBonusGameStartCoroutine()
    {
        fadingMask.SetActive(true);
        CoroutineHandle fadeBackgroundCoroutine = Timing.RunCoroutine(FadeBackground());
        AudioManager.instance.SFX_Source.PlayOneShot(playButtonSound);
        yield return Timing.WaitUntilDone(fadeBackgroundCoroutine);
        foreach (GameObject go in enableAfterFading)
        {
            go.SetActive(true);
        }
        SceneManager.LoadSceneAsync(Constants.bonusGameBuildIndex);
    }

    IEnumerator<float> FadeBackground()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            Color maskColor = fadingMask.GetComponent<Image>().color;
            fadingMask.GetComponent<Image>().color = new Color(maskColor.r, maskColor.g, maskColor.b, Mathf.Lerp(0, 1, timer / fadeTime));
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }
}
