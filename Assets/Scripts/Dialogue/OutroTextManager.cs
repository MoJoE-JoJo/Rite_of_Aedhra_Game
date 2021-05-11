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
    private float fadingCounter = 0f;
    private bool fading = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (TextFader tf in textFaders)
        {
            tf.Init();
        }
        textFaders[textIndex].Fade();
        fading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (textIndex == textFaders.Count - 1)
        {
            continueButton.SetActive(false);
            endButton.SetActive(true);
        }
        else if (fading)
        {
            if(fadingCounter >= textFaders[textIndex].fadeDuration)
            {
                fading = false;
                fadingCounter = 0f;
                if (!textFaders[textIndex].fadeIn) NextText();
            }
            else fadingCounter += Time.deltaTime;
        }

    }
    public void EndGame()
    {
        SceneManager.LoadScene((int)SceneIndex.Menu);
    }
    public void NextText()
    {
        if (!fading)
        {
            //if (textIndex == textFaders.Count - 1) textIndex++;
            textFaders[++textIndex].Fade();
            fadingCounter = 0f;
            fading = true;
        }
    }
}
