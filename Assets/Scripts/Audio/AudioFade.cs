using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FadeType { FADE_IN, FADE_OUT }
public class AudioFade : MonoBehaviour
{
    [SerializeField] private AudioArea audioArea;
    private AudioSource audioSource;
    [SerializeField] private FadeType fade;
    [SerializeField] private float fadeDuration;
    private float fadeCounter = 0f;

    private float targetVolume;
    private float startVolume;
    private bool fading = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = audioArea.audioSource;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            fadeCounter += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, fadeCounter / fadeDuration);
            if (fadeCounter / fadeDuration >= 1f)
            {
                fading = false;
                fadeCounter = 0f;
                audioArea.audioProgress = audioSource.time;
                if (audioArea.activeArea == false && fade == FadeType.FADE_OUT) audioSource.Stop();
            }
        }
    }

    public void StartFade()
    {
        fading = true;
        startVolume = audioSource.volume;
        if(fadeCounter != 0f) fadeCounter = fadeDuration - fadeCounter;
        if (audioArea.activeArea == true && fade == FadeType.FADE_IN)
        {
            targetVolume = audioArea.volume;
            audioSource.Play();
            audioSource.time = audioArea.audioProgress;
        }
        else if (fade == FadeType.FADE_OUT) targetVolume = 0f;
    }
}
