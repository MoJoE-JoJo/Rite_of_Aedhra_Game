using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            gameObject.SetActive(false);
    }
}
