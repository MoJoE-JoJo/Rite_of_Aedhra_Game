using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer gameAudioMixer;
    [SerializeField] private AudioArea startArea;
    public AudioArea currentArea;

    [SerializeField] private float masterVolume = 0f;
    [SerializeField] private float musicVolume = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startArea.activeArea = true;
        startArea.PlayAudio(true);
        currentArea = startArea;
        //startArea.audioSource.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        gameAudioMixer.SetFloat("MasterVolume", masterVolume);
        gameAudioMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void ActivateArea(AudioArea area)
    {
        currentArea.activeArea = false;
        currentArea.StopAudio(true);

        currentArea = area;

        currentArea.activeArea = true;
        currentArea.PlayAudio(true);
    }
}
