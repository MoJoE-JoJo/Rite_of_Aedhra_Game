using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioArea : MonoBehaviour
{
    public bool activeArea;
    public AudioSource audioSource;
    public float volume = 1f;

    [SerializeField] private AudioFade  fadeIn;
    [SerializeField] private AudioFade fadeOut;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(bool fade)
    {
        if (fade)
        {
            fadeIn?.StartFade();
        }
        else if (!fade)
        {
            audioSource.Play();
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
            audioSource.Stop();
        }
    }
}
