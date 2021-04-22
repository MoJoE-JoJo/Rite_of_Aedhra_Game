using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shout : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip[] _audioClips;
    private AudioSource _audioSource;
    private int _lastClip = -1;

    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayShout()
    {
        int randomClip = _lastClip;
        while (randomClip == _lastClip)
            randomClip = Random.Range(0, _audioClips.Length);
        _audioSource.PlayOneShot(_audioClips[randomClip]);
    }
}