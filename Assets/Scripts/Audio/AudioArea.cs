using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioArea : MonoBehaviour
{
    public bool activeArea;
    public AudioSource audioSource;
    public float volume = 1f;
    public float audioProgress = 0f;

    [SerializeField] private AudioFade  fadeIn;
    [SerializeField] private AudioFade fadeOut;
    private AudioManager audioManager;
    

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(bool fade)
    {
        //audioManager.currentArea = this;
        if (fade)
        {
            fadeIn?.StartFade();
        }
        else if (!fade)
        {
            audioSource.volume = volume;
            audioSource.Play();
            audioSource.time = audioProgress;
        }
    }

    public void StopAudio(bool fade)
    {
        if (fade)
        {
            fadeOut?.StartFade();
        }
        else if (!fade)
        {
            audioProgress = audioSource.time;
            audioSource.volume = 0f;
            audioSource.Stop();
        }
    }
}
