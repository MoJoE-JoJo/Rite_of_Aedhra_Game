using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class ProximityDetection : MonoBehaviour
{
    [SerializeField] private Golem golem;

    private void Start()
    {
        golem = GetComponentInParent<Golem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Assert(golem != null, nameof(golem) + " != null");
        golem.ForceChasePlayer(other.gameObject);
    }
}
