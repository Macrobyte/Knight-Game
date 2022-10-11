using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        Debug.Log("Credits Selected");
    }

    public void OptionsButton()
    {
        Debug.Log("Options Selected");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
