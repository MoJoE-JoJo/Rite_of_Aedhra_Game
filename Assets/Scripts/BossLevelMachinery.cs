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
    private GameObject[] lavaFlows;

    // Start is called before the first frame update
    void Start()
    {
        bool isOn = GameManager.Instance.GetPuzzleStatus(puzzleName);
        foreach (GameObject machine in machines)
        {
            Animation animation = machine.GetComponent<Animation>();
            if (animation && isOn)
            {
                animation.Play();
            }
            else
            {
                animation.Stop();
            }
        }
        foreach (GameObject lavaFlow in lavaFlows)
        {
            lavaFlow.SetActive(isOn);
        }
    }
}
