using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCollision : MonoBehaviour
{
    public Golem golem;
    public Crawler crawler;

    public bool collisionStatus;


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
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collisionStatus = true;
                if(golem)
                    golem.chasingPlayer = false;
                if(crawler)
                    crawler.chasingPlayer = false;
                break;
            case "Throwable":
                if (golem)
                {
                    golem.rangeSensor.IgnoreList.Add(collision.gameObject);
                    golem.chasingThrowable = false;
                }
                
                if (crawler)
                {
                    crawler.rangeSensor.IgnoreList.Add(collision.gameObject);
                    crawler.chasingThrowable = false;
                }
               
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
