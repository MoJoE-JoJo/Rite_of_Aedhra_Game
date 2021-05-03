using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndoDontDestroyOnLoad : MonoBehaviour
{
    public GameObject dialogueParent;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
