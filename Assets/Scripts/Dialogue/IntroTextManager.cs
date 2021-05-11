using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game_Systems;

public class IntroTextManager : MonoBehaviour
{
    [SerializeField] private int textIndex = 0;
    [SerializeField] private List<TextFader> textFaders;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject startButton;

    // Start is called before the first frame update
    void Start()
    {
        foreach(TextFader tf in textFaders)
        {
            tf.Init();
        }
        textFaders[textIndex++].Fade();
    }

    // Update is called once per frame
    void Update()
    {

        if(textIndex == textFaders.Count)
        {
            continueButton.SetActive(false);
            startButton.SetActive(true);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene((int)SceneIndex.Shrine);
    }
    public void NextText()
    {
        textFaders[textIndex - 1].SkipFade();
        textFaders[textIndex++].Fade();
        /*
        if (textIndex == textFaders.Count)
        {
            SceneManager.LoadScene((int)SceneIndex.Shrine);
        }
        
        
        else
        {
            textFaders[textIndex-1].SkipFade();
            textFaders[textIndex++].Fade();
        }
        */
    }
}
