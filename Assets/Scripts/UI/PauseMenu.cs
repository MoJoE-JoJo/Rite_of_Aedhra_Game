using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (controlsMenuUI.activeInHierarchy)
            {
                return;
            }
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Controls()
    {
        controlsMenuUI.SetActive(true);
    }

    public void ExitGame ()
    {
        Application.Quit();
    }
}
