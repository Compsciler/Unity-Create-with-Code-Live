using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsTextCommands : MonoBehaviour
{
    private TMP_InputField inputFieldComponent;

    // Start is called before the first frame update
    void Start()
    {
        Transform optionsMenuTransform = GameObject.Find("Options Menu").transform;
        inputFieldComponent = optionsMenuTransform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        string inputFieldText = inputFieldComponent.text;
    }
}
