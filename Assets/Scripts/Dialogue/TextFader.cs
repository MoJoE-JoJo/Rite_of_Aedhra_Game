using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour
{
    private Text text;

    public float fadeDuration;
    private float fadeCounter = 0f;
    public bool fadeIn = true;

    public bool fading = false;

    // Start is called before the first frame update
    void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (fadeIn)
            {
                fadeCounter += Time.deltaTime;
                var alpha = Mathf.SmoothStep(0, 1, fadeCounter / fadeDuration);
                if (fadeCounter / fadeDuration >= 1f)
                {
                    fading = false;
                    fadeCounter = 0f;
                }
                var newColor = text.color;
                newColor.a = alpha;
                text.color = newColor;
            }

            else
            {
                fadeCounter += Time.deltaTime;
                var alpha = Mathf.SmoothStep(1, 0, fadeCounter / fadeDuration);
                if (fadeCounter / fadeDuration >= 1f)
                {
                    fading = false;
                    fadeCounter = 0f;
                }
                var newColor = text.color;
                newColor.a = alpha;
                text.color = newColor;
            }

        }
    }

    public void Init()
    {
        var newColor = text.color;
        newColor.a = 0;
        text.color = newColor;
    }

    public void Fade()
    {
        fading = true;
    }

    public void SkipFade()
    {
        if (fadeIn)
        {
            fading = false;
            var newColor = text.color;
            newColor.a = 1;
            text.color = newColor;
        }
        else
        {
            fading = false;
            var newColor = text.color;
            newColor.a = 0;
            text.color = newColor;
        }
        
    }
}
