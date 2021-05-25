using System;
using System.Collections;
using System.Collections.Generic;
using Game_Systems;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class ProximityDetection : MonoBehaviour
{
    [SerializeField] private Golem golem;
    [SerializeField] private Crawler crawler;

    private void Start()
    {
         golem = null;
                crawler = null;
                var g = GetComponentInParent<Golem>();
                if (g != null && g.enabled)
                    golem = g;
                var c = GetComponentInParent<Crawler>();
                if (c != null && c.enabled)
                    crawler = c;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || GameManager.Instance.playerIsDead) return;
        golem?.ForceChasePlayer(other.gameObject);
        crawler?.ForceChasePlayer(other.gameObject);
    }
}
