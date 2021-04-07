using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTest : MonoBehaviour
{
    private static bool rockStatus;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rockStatus = true;          
        }
    }

    public static bool getRockStatus()
    {
        return rockStatus;   
    }
}
