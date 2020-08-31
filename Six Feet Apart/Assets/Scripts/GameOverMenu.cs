using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    // public Text averageFPS_Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        Timing.KillCoroutines();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        // ResetStaticVariables() delegate in GameManager.cs on scene unload
    }

    public void GoToMainMenu()
    {
        Timing.KillCoroutines();
        SceneManager.LoadSceneAsync(Constants.mainMenuBuildIndex);
        // ResetStaticVariables() delegate in GameManager.cs on scene unload
    }
}
