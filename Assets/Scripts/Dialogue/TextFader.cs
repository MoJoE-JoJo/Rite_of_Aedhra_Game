using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour
{
    private Text text;

    [SerializeField] private float fadeDuration;
    private float fadeCounter = 0f;

    private bool fading = false;

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
        fading = false;
        var newColor = text.color;
        newColor.a = 1;
        text.color = newColor;
    }
}
