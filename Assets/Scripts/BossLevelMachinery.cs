using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game_Systems;

public class BossLevelMachinery : MonoBehaviour
{
    [SerializeField]
    private string puzzleName;
    [SerializeField]
    private GameObject[] machines;
    [SerializeField]
    private AudioSource machineSound;
    [SerializeField]
    private GameObject[] lavaFlows;
    [SerializeField]
    private GameObject transition;

    // Start is called before the first frame update
    void Start()
    {
        bool isOn = GameManager.Instance.GetPuzzleStatus(puzzleName);
        foreach (GameObject machine in machines)
        {
            Animation animation = machine.GetComponent<Animation>();
            if (animation && isOn)
            {
                machineSound.Play();
                animation.Play();
            }
            else
            {
                machineSound.Stop();
                animation.Stop();
            }
        }
        foreach (GameObject lavaFlow in lavaFlows)
        {
            lavaFlow.SetActive(isOn);
        }
        transition.SetActive(!isOn);
    }
}
