using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    private string linkUrl = "https://www.cdc.gov/coronavirus/2019-ncov/prevent-getting-sick/social-distancing.html";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLink()
    {
        Application.OpenURL(linkUrl);
    }
}
