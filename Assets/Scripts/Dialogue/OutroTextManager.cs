using Game_Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroTextManager : MonoBehaviour
{
    [SerializeField] private int textIndex = 0;
    [SerializeField] private List<TextFader> textFaders;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject endButton;
    private float fadeOutCounter = 0f;
    private bool fadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (TextFader tf in textFaders)
        {
            tf.Init();
        }
        textFaders[textIndex++].Fade();
    }

    // Update is called once per frame
    void Update()
    {
        if (textIndex == textFaders.Count)
        {
            continueButton.SetActive(false);
            endButton.SetActive(true);
        }
        else if (fadeOut)
        {
            if(fadeOutCounter >= textFaders[textIndex].fadeDuration)
            {
                fadeOut = false;
                fadeOutCounter = 0f;
                NextText();
            }
            else fadeOutCounter += Time.deltaTime;
        }

    }
    public void EndGame()
    {
        SceneManager.LoadScene((int)SceneIndex.Menu);
    }
    public void NextText()
    {
        textFaders[textIndex - 1].SkipFade();
        textFaders[textIndex].Fade();
        if (!textFaders[textIndex].fadeIn) fadeOut = true;
        textIndex++;
    }
}
