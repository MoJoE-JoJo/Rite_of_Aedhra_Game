using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shout : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip[] spottedSfx;
    [SerializeField] private AudioClip[] lostSightSfx;
    [SerializeField] private AudioClip[] killSfx;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySpottedSfx()
    {
        int randomClip = Random.Range(0, spottedSfx.Length);
        _audioSource.PlayOneShot(spottedSfx[randomClip]);
    }
    
    public void PlayLostSightSfx()
    {
        int randomClip = Random.Range(0, lostSightSfx.Length);
        _audioSource.PlayOneShot(lostSightSfx[randomClip]);
    }
    
    public void PlayKillSfx()
    {
        int randomClip = Random.Range(0, killSfx.Length);
        _audioSource.PlayOneShot(killSfx[randomClip]);
    }
}