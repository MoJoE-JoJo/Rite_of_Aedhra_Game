using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroTextManager : MonoBehaviour
{
    [SerializeField] private int textIndex = 0;
    [SerializeField] private List<TextFader> textFaders;
    [SerializeField] private Text continueButtonText;

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
            continueButtonText.text = "Start";
        }
    }

    public void NextText()
    {
        if (textIndex == textFaders.Count)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            textFaders[textIndex-1].SkipFade();
            textFaders[textIndex++].Fade();
        }
    }
}
