using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject controlsMenuUI;

    public void PlayGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame ()
    {
        Application.Quit();
    }

    public void Controls ()
    {
        controlsMenuUI.SetActive(true);
    }
}
