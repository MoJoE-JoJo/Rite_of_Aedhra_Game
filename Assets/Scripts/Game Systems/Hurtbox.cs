using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (!other.gameObject.CompareTag("Player")) return;
        Debug.Log("Just die dude");
        other.gameObject.GetComponent<PlayerController>().KillPlayer();
    }
}
