using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCollision : MonoBehaviour
{
    public Golem golem;

    public bool collisionStatus;

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collisionStatus = true;
                golem.chasingPlayer = false;
                break;
            case "Throwable":
                golem.rangeSensor.IgnoreList.Add(collision.gameObject);
                golem.chasingThrowable = false;
                break;
        }
    }
    public bool CollisionStatus()
    {
        return collisionStatus;
    }

    public bool SetCollisionStatus(bool status)
    {
        return collisionStatus = status;
    }
}
