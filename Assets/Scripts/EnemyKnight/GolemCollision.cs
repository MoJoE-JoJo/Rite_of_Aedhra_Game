using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCollision : MonoBehaviour
{
    public Golem golem;

    private bool collisionStatus;

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
        if (collision.gameObject.tag == "Throwable")
        {
            //Destroy throwable?
            //Destroy(collision.gameObject);
            //steeringRig.IgnoreList.Add(collision.gameObject);
            golem.rangeSensor.IgnoreList.Add(collision.gameObject);
            golem.chasingThrowable = false;           
        }
    }
    public bool CollisionStatus()
    {
        return collisionStatus;
    }
}