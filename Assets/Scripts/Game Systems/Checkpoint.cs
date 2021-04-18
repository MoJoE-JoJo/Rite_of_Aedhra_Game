using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Vector3 spawnOffset = new Vector3(1, 0, 1);
    private bool Activated { get; set; } = false;
    private Vector3 SpawnPosition { get; set; }

    private static List<Checkpoint> _checkpointList;

    // Start is called before the first frame update
    private void Start()
    {
        _checkpointList = GameObject.FindGameObjectsWithTag("Checkpoint").Select((go) => go.GetComponent<Checkpoint>()).ToList();
        SpawnPosition = transform.position + spawnOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            ActivateCheckpoint();
    }

    private void ActivateCheckpoint()
    {
        foreach (Checkpoint cp in _checkpointList)
        {
            cp.Activated = false;
            cp.transform.GetChild(0).gameObject.SetActive(false);
        }

        Activated = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public static Vector3 GetActiveCheckpointPosition()
    {
        if (_checkpointList == null) return new Vector3(0, 0, 0);
        foreach (Checkpoint cp in _checkpointList.Where(cp => cp.GetComponent<Checkpoint>().Activated))
            return cp.SpawnPosition;
        return new Vector3(0,0,0);
    }
}
