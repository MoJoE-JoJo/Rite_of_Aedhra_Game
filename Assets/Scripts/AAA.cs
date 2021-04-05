using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAA : MonoBehaviour
{
    public RangeSensor rangeSensor;

    public SteeringRig steeringRig;

    private void Update()
    {
        RangeSensor();

    }

    void RangeSensor()
    {
        var detected = rangeSensor.GetNearest();

        if (detected != null && RockTest.getRockStatus())
        {
            {
                steeringRig.DestinationTransform = detected.gameObject.transform;

                Chase(detected);
            }
        }
    }

    void Chase(GameObject target)
    {
        transform.LookAt(target.transform);

        transform.position += transform.forward * 1.25f * Time.deltaTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Throwable")
        {
            rangeSensor.IgnoreList.Add(collision.gameObject);
        }
    }
}
