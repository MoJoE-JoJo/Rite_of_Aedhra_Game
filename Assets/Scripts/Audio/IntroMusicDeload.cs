using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game_Systems;
using UnityEngine.SceneManagement;

public class IntroMusicDeload : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Audio").Length == 1) DontDestroyOnLoad(this);
        else Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneIndex.Shrine) Destroy(this.gameObject);
    }
}
