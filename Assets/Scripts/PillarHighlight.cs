using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarHighlight : MonoBehaviour
{
    [SerializeField]
    private LockPosition[] lockPositions;
    [SerializeField]
    private GameObject[] pillars;

    void Start()
    {
        foreach(GameObject go in pillars)
        {
            go.AddComponent<UpDownPillar>();
        }
    }

    private void CheckConditions()
    {
        int count = 0;
        for(int i = 0; i < lockPositions.Length; i++)
        {
            Lock l = lockPositions[i].go.GetComponent<Lock>();
            if (!l)
            {
                Debug.LogError("GameObject has no Lock component.");
                return;
            }
            if (l.GetIsOn())
            {
                count += (int)Mathf.Pow(2, i);
            }
        }

        for (int i = 0; i < pillars.Length; i++)
        {
            pillars[i].GetComponent<UpDownPillar>().TogglePillar(i < count);
        }
    }

    void OnEnable()
    {
        Lock.LockChangedEvent += CheckConditions;
    }

    void OnDisable()
    {
        Lock.LockChangedEvent -= CheckConditions;
    }

}
