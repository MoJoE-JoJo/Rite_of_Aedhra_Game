using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCollision : MonoBehaviour
{
    private static bool collisionStatus;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collisionStatus = true;
        }
        else
        {
            collisionStatus = false;
        }
    }
    public static bool CollisionStatus()
    {
        return collisionStatus;
    }
}
